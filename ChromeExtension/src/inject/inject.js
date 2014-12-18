(function() {

	function getChats(kind) {
		return Array.prototype.slice.call(document.querySelectorAll("#channels_scroller li." + kind));
	}

	setInterval(function() {

		var allChats = getChats("channel");
		allChats = allChats.concat(getChats("group"));
		allChats = allChats.concat(getChats("member"));
		var chatStatus = allChats.map(function(element) {
			return {
				name: element.querySelector(".overflow-ellipsis").innerText.trim().replace("# ", "#"),
				unread: element.classList.contains("unread"),
				mention: element.classList.contains("mention")
			}
		});

		chrome.extension.sendMessage(chatStatus);

	}, 1000)
})();