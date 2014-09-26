(function () {

    var notificationId = "SlackMessage";
    var notificationOptions = {
        type: "basic",
        title: "Incomimg message in Slack",
        message: "",
        iconUrl: "../icons/icon128.png"
    };

    var lastAlertLevel = null;

    chrome.extension.onMessage.addListener(
        function (request, sender, sendResponse) {
            if(lastAlertLevel === request.alertLevel) {
                return;
            }

            lastAlertLevel = request.alertLevel;

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

        });
})();