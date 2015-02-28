(function () {

    var notificationId = "SlackMessage";
    var notificationOptions = {
        type: "basic",
        title: "Incomimg message in Slack",
        message: "",
        iconUrl: "../icons/icon128.png"
    };

    var lastMessage = null;
    var lastSender = null;
    var websocket;
    var lastAlertMessageTime = Date.now();

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

    function focusOnLastSender() {
        if (lastSender) {
            chrome.tabs.update(lastSender.tab.id, {selected: true});
        } else {
            alert("No active Slack tab is found. Please open/reload one manually.")
        }
    }

    function connect() {
        setBadge("Disconnected from tray icon", "000000");

        websocket = new WebSocket("ws://localhost:4649/Slack");
        websocket.onopen = function (evt) {
            setBadge("Connected");
            if(lastMessage !== null) {
                // Tray app reconnected - Update the badges and send it the last alert level
                sendLastMessage();
            }
        };
        websocket.onclose = function (evt) {
            //try to reconnect in 5 seconds
            setTimeout(function () {connect() }, 5000);
        };
    }

    // color - Color hex value - If not given, the badge is removed
    function setBadge(tooltip, color) {
        if(color) {
            var rgb = hexToRgb(color);
            chrome.browserAction.setBadgeBackgroundColor({color: [rgb.r, rgb.g, rgb.b, 255]});
            chrome.browserAction.setBadgeText({text: " "});
        } else {
            chrome.browserAction.setBadgeText({text: ""});
        }

        chrome.browserAction.setTitle({title: tooltip});
    }

    chrome.browserAction.onClicked.addListener(function () {
        focusOnLastSender();
    });

    chrome.notifications.onClicked.addListener(function () {
        focusOnLastSender();
    });

    function sendLastMessage() {
        websocket.send(lastMessage);
    }

    chrome.extension.onMessage.addListener(
        function (message, sender, sendResponse) {
            lastAlertMessageTime = Date.now();

            var stringMessage = JSON.stringify(message);
            if (lastMessage === stringMessage) {
                return;
            }

            lastMessage = stringMessage;

            // Send only if connected
            if(websocket.readyState === 1) {
                lastSender = sender;
                sendLastMessage();
            }
        });

    // This will try to connect to a server and reconnect if needed
    connect();

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

    setInterval(function () {
        if(Date.now() - lastAlertMessageTime >= 5000) {
            setBadge("Disconnected from Slack tab", "000000");
            lastMessage = null;
        }
    }, 5000);


})();