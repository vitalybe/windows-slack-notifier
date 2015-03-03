(function () {

    var notificationId = "SlackMessage";
    var notificationOptions = {
        type: "basic",
        title: "Incomimg message in Slack",
        message: "",
        iconUrl: "../icons/icon128.png"
    };

    var websocket;
    var appConnectionPromise = null;
    var connectedPorts = {};

    // Logging
    chrome.runtime.onMessage.addListener(function (request, sender, sendResponse) {
        if (request.type === "log" && request.message) {
            console.log("[TAB] " + request.message);
        }
    });
    
    function log(message) {
        console.log("[BCK] " + message);
    }


    function hexToRgb(hex) {
        // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
        var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
        hex = hex.replace(shorthandRegex, function(m, r, g, b) {
            return r + r + g + g + b + b;
        });

        var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
        return result ? {
            r: parseInt(result[1], 16),
            g: parseInt(result[2], 16),
            b: parseInt(result[3], 16)
        } : null;
    }

    // color - Color hex value - If not given, the badge is removed
    function setBadge(tooltip, color) {
        if (color) {
            var rgb = hexToRgb(color);
            chrome.browserAction.setBadgeBackgroundColor({ color: [rgb.r, rgb.g, rgb.b, 255] });
            chrome.browserAction.setBadgeText({ text: " " });
        } else {
            chrome.browserAction.setBadgeText({ text: "" });
        }

        chrome.browserAction.setTitle({ title: tooltip });
    }

    function onAppMessage(evt) {
        log("onAppMessage: " + evt.data);

        var message = JSON.parse(evt.data);
        log("onAppMessage - Sending to tabPort: " + message.tabPortName);

        if (connectedPorts[message.tabPortName]) {
            connectedPorts[message.tabPortName].postMessage(message);
        } else {
            log("onAppMessage - tabPort not found, was probably closed");
        }
    }

    function onAppDisconnect() {
        log("onAppDisconnect - Disconnecting all connected tab ports");
        appConnectionPromise = null;

        _.each(connectedPorts, function (port, portName) {
            log("onAppDisconnect - Disconnecting: " + portName);
            port.disconnect();
        });
    }

    function onTabPortDisconnect(tabPort) {
        log("onTabPortDisconnect: " + tabPort.name);

        connectedPorts[tabPort.name] = null;
    }

    function onTabPortMessage(tabPort, message) {
        message.tabPortName = tabPort.name;
        var stringMessage = JSON.stringify(message);
        log("onTabPortMessage - Sending to app: " + stringMessage);

        if (websocket.readyState === 1) {
            websocket.send(stringMessage);
        } else {
            log("onTabPortMessage - Websocket is closed, sending failed");
        }
    }

    function appConnect() {
        log("appConnect - Connecting to app...");

        var deferred = Q.defer();
        setBadge("Disconnected from tray app", "000000");

        websocket = new WebSocket("ws://localhost:4649/Slack");
        websocket.onopen = function (evt) {
            log("appConnect - Connected!");
            setBadge("Connected");
            deferred.resolve();
            websocket.onclose = onAppDisconnect;
        };
        websocket.onclose = function (evt) {
            log("appConnect - Connection failed");
            deferred.reject();
        };

        websocket.onmessage = onAppMessage;

        return deferred.promise;
    }

    chrome.browserAction.onClicked.addListener(function () {
    });

    chrome.runtime.onConnect.addListener(function (tabPort) {

        log("onConnect - New tabPort: " + tabPort.name);

        connectedPorts[tabPort.name] = tabPort;
        if (!appConnectionPromise) {
            log("onConnect - Starting a new conneciton ");
            appConnectionPromise = appConnect();
        }

        appConnectionPromise
        .then(function () {
            if (connectedPorts[tabPort.name]) {
                log("onConnect - Posting 'connect' message to tabPort");
                connectedPorts[tabPort.name].postMessage({ command: "connected", thread: null });
            } else {
                log("onConnect - tabPort closed, not posting new message");
            }
        })
        .catch(function () {
            log("onConnect - Failed to connect, disconnecting tabPort");
            appConnectionPromise = null;
            tabPort.disconnect();
        });

        tabPort.onDisconnect.addListener(onTabPortDisconnect);
        tabPort.onMessage.addListener(function (message) { onTabPortMessage(tabPort, message); });
    });

    // Reload all Slack tabs to make sure they get injected with the extension code
    chrome.windows.getAll({populate: true}, function (windows) {
        windows.forEach(function (window) {
            window.tabs.forEach(function (tab) {
                if (tab.url.indexOf("slack.com") >= 0) {
                    chrome.tabs.reload(tab.id);
                }
            });
        });
    });
    
})();