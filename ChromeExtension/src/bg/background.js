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

    function focusOnLastSender() {
        if (lastSender) {
            chrome.tabs.update(lastSender.tab.id, {selected: true});
        } else {
            alert("No active Slack tab is found. Please open/reload one manually.")
        }
    }

    var websocket;
    function connect() {
        websocket = new WebSocket("ws://localhost:4649/Slack");
        websocket.onopen = function(evt) { alert("open"); };
        websocket.onclose = function(evt) { alert("close") };
    }

    chrome.browserAction.onClicked.addListener(function () {
        focusOnLastSender();
    });

    chrome.notifications.onClicked.addListener(function () {
        focusOnLastSender();
    });

    chrome.extension.onMessage.addListener(
        function (request, sender, sendResponse) {
            if (lastAlertLevel === request.alertLevel) {
                return;
            }

            lastAlertLevel = request.alertLevel;
            lastSender = sender;

            if (request.alertLevel === 2) {
                chrome.browserAction.setBadgeBackgroundColor({color: [255, 0, 0, 255]});
                chrome.notifications.create(notificationId, notificationOptions, function () {
                });
            } else {
                chrome.notifications.clear(notificationId, function () {
                });
            }

            if (request.alertLevel === 1) {
                chrome.browserAction.setBadgeBackgroundColor({color: [255, 255, 255, 255]});
            }

            if (request.alertLevel > 0) {
                chrome.browserAction.setBadgeText({text: " "});
            } else {
                chrome.browserAction.setBadgeText({text: ""});
            }

            websocket.send(request.alertLevel);
        });

    connect();

})();