(function () {

    var notificationId = "SlackMessage";
    var notificationOptions = {
        type: "basic",
        title: "Incomimg message in Slack",
        message: "",
        iconUrl: "../icons/icon128.png"
    };

    var lastAlertLevel = null;
    var lastSender = null;
    var websocket;

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
        setBadge("Disconnected", "000000");

        websocket = new WebSocket("ws://localhost:4649/Slack");
        websocket.onopen = function (evt) {
            setBadge("Connected");
            if(lastAlertLevel !== null) {
                // Tray app reconnected - Update the badges and send it the last alert level
                onLastAlertLevelChanged();
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

    function onLastAlertLevelChanged() {
        if (lastAlertLevel === 2) {
            setBadge("Important unread", "ff0000");
        } else if (lastAlertLevel === 1) {
            setBadge("Unread", "ffffff");
        } else {
            setBadge("All read");
        }

        websocket.send(lastAlertLevel);
    }

    chrome.extension.onMessage.addListener(
        function (request, sender, sendResponse) {
            if (lastAlertLevel === request.alertLevel) {
                return;
            }

            lastAlertLevel = request.alertLevel;

            // Send only if connected and don't touch badge
            if(websocket.readyState === 1) {
                lastSender = sender;
                onLastAlertLevelChanged();
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


})();