window.scrollToElementId = function (elementId) {
	const element = document.getElementById(elementId);
	if (element) {
		const elementIdHref = `#${elementId}`;
		if (!window.location.pathname.endsWith(elementIdHref)) {
			history.replaceState(null, "", window.location.pathname + elementIdHref);
		}

		element.scrollIntoView({
			behavior: "smooth",
			block: "start",
			inline: "nearest"
		});
	}
}