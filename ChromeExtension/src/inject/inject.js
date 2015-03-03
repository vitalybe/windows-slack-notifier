(function() {

    var RECONNECT_AFTER_MS = 5000;

    var port = null;
    var waitingCommandRequests = [];
    var notificationElement = null;

    
    function log(message) {
        chrome.runtime.sendMessage({ type: "log", message: message }, function (response) {
            console.log(response.farewell);
        });
    }

    function getSubdomain() {
        var regexParse = new RegExp('[a-z\-0-9]{2,63}\.[a-z\.]{2,5}$');
        var urlParts = regexParse.exec(window.location.hostname);
        return window.location.hostname.replace(urlParts[0], '').slice(0, -1);
    }


    function notification(title, content) {
        if (!notificationElement) {
            notificationElement = document.createElement("div");
            var notificationClose = document.createElement("div");
            notificationClose.className = "close-notification";
            notificationClose.innerText = "x";
            notificationClose.onclick = function() {
                document.querySelector("body").removeChild(notificationElement);
            };

            notificationElement.id = "windows-tray-notification";
            notificationElement.innerHTML = '<div class="title"></div><div class="content"></div>';
            notificationElement.appendChild(notificationClose);
            document.querySelector("body").appendChild(notificationElement);
        }

        notificationElement.querySelector(".title").innerHTML = "<h3>" + title + " - Slack Windows Tray</h3>";
        notificationElement.querySelector(".content").innerHTML = content;
    }

    function getChats(kind) {
		return Array.prototype.slice.call(document.querySelectorAll("#channels_scroller li." + kind));
	}

    function generateUUID() {
        var d = new Date().getTime();
        var uuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = (d + Math.random() * 16) % 16 | 0;
            d = Math.floor(d / 16);
            return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
        return uuid;
    };

    function sendUpdate() {

        var allChats = getChats("channel");
        allChats = allChats.concat(getChats("group"));
        allChats = allChats.concat(getChats("member"));
        var chatStatus = allChats.map(function (element) {

            var channelName;
            try {
                channelName = element.querySelector(".overflow_ellipsis").innerText.trim().replace("# ", "#");
            } catch (err) {
                console.log("SlackWindwowsTray: Failed to get channel name: " + err);
                channelName = "EXT-ERROR";
            }

            return {
                name: channelName,
                unread: element.classList.contains("unread"),
                mention: element.classList.contains("mention")
            };
        });

        chrome.extension.sendMessage(chatStatus);

    }
    

    function sendCommandWaitReply(command) {
        log("sendCommandWaitReply - Command: " + command);
        if (port == null) {
            log("sendCommandWaitReply - Port disconnected, command will not execute");
            Q.reject("Disconnected");
        }
        
        var thread = generateUUID();
        port.postMessage({ command: command, thread: thread });
        
        return waitForCommand(command, thread);
    }

    // threadId - Optional
    function waitForCommand(command, thread) {
        thread = thread || null;

        var deferred = Q.defer();

        log("waitForCommand - Command: '" + command + "' Thread: '" + thread + "'");

        var waitFor = {
            command: command,
            thread: thread,
            deferred: deferred
        };

        waitingCommandRequests.push(waitFor);

        return deferred.promise;
    }
    
    function onPortDisconnect() {
        log("onPortDisconnect - Port disconnected, queuing to reconnect in " + RECONNECT_AFTER_MS + "ms");
        port = null;
        // Reconnect after X seconds
        setTimeout(connect, RECONNECT_AFTER_MS);

        _.each(waitingCommandRequests, function(request) {
            log("onPortDisconnect - Rejecting a waiting request: " + request);
            request.deferred.reject();
        });
    }

    function onPortMessage(message) {
        log("onPortMessage - Message:" + JSON.stringify(message));
        var waitingCommandRequest = _.find(waitingCommandRequests, { command: message.command, thread: message.thread });
        if (waitingCommandRequest) {
            log("onPortMessage - Found waiting command request. Resolving...");
            _.pull(waitingCommandRequests, waitingCommandRequest);
            waitingCommandRequest.deferred.resolve(message);
        }

        // In the future, we might handle command that we're not waiting for (that were initiated in the app)
    }

    // Sends a messages and resolves when connected
    function connect() {
        log("connect - Connecting...");
        port = chrome.runtime.connect({ name: getSubdomain() });
        var extensionVersion = chrome.runtime.getManifest().version;
        waitForCommand("connected")
            .then(function() {
                log("connect - Connected. Requesting version...");
                return sendCommandWaitReply("version").catch(function() {
                    log("connect - Disconnected after requesting a version, probably old app version");
                    // This will occur when the app closed the connection after the `version` command was sent
                    // It is probably that it will happen due to an old version
                    notification("Update possibly required",
                        "<p>Failed to get app version, this might be due to an older version.</p>" +
                        "<p><b>Extension version</b>: " + extensionVersion + "</p>" +
                        '<p>Download the latest version <a href="https://github.com/vitalybe/windows-slack-notifier/releases">here</a>, ' +
                        'run it and refresh Slack.</p>'
                        );
                });
            })
            .then(function(versionReply) {
                var appVersion = versionReply.body;
                log("connect - App version: " + appVersion);
                log("connect - Extension version: " + extensionVersion);
                if (appVersion !== extensionVersion) {
                    log("connect - Versions mismatch, showing notification");
                    notification("Update required", 
                        "<p><b>Extension version</b>: " + extensionVersion + "</p>" +
                        "<p><b>App version</b>: " + appVersion + "</p>"
                        );
                }
            });
        port.onDisconnect.addListener(onPortDisconnect);
        port.onMessage.addListener(onPortMessage);
    }

    connect();

})();