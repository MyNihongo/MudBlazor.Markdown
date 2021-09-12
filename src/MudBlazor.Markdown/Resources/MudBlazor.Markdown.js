import hljs from "highlight.js";

window.highlightCodeElement = function (element, lang) {
	hljs.highlightElement(element, { language: lang });
}

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