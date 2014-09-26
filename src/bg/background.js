(function() {

    chrome.browserAction.onClicked.addListener(function () {
        var opt = {
            type: "basic",
            title: "Primary Title",
            message: "Primary message to display",
            iconUrl: "../icons/slack.png"
        }

        chrome.notifications.create("", opt, function () {

        });
    });

	chrome.extension.onMessage.addListener(
	  function(request, sender, sendResponse) {
	  	if(request.alertLevel === 2) {
			chrome.browserAction.setBadgeBackgroundColor({color:[255, 0, 0, 255]});
	  	} else if(request.alertLevel === 1) {
			chrome.browserAction.setBadgeBackgroundColor({color:[255, 255, 255, 255]});
	  	}

		if(request.alertLevel > 0) {
			chrome.browserAction.setBadgeText({text:" "});
		} else {
			chrome.browserAction.setBadgeText({text:""});
		}

	  });
})();