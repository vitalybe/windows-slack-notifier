(function() {
	setInterval(function() {
		var alertLevel = 0;
		if(document.querySelector("#channels_scroller .unread_highlight:not(.hidden)")) {
			alertLevel = 2;
		}

		if(!alertLevel && document.querySelector("#channels_scroller .unread")) {
			alertLevel = 1;
		}

		chrome.extension.sendMessage({alertLevel: alertLevel});

	}, 1000)
})();