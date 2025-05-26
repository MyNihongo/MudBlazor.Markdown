import hljs from "highlight.js";
import hljsRazor from "highlightjs-cshtml-razor";

hljs.registerLanguage("cshtml", hljsRazor);
hljs.registerLanguage("razor", hljsRazor);
hljs.registerLanguage("razor-cshtml", hljsRazor);

const codeStylesDir = "code-styles";
const codeStylesSegment = `/MudBlazor.Markdown/${codeStylesDir}/`;

// HighlightJS
window.highlightCodeElement = function (element, text, language) {
	let result;
	
	try {
		result = language ? hljs.highlight(text, { language }) : hljs.highlightAuto(text);
	} catch (e) {
		console.error(e);
		result = hljs.highlightAuto(text);
	}
	
	element.innerHTML = result.value;
}

window.setHighlightStylesheet = function (stylesheetPath) {
	let isFound = false;
	const stylesheets = document.querySelectorAll("link[rel='stylesheet']");

	for (let i = 0; i < stylesheets.length; i++) {
		const href = stylesheets[i].getAttribute("href");

		if (!href) {
			continue;
		}

		const index = href.indexOf(codeStylesSegment);

		if (index !== -1) {
			if (!isFound) {
				isFound = true;
				const newHref = href.substring(0, index + codeStylesSegment.length) + stylesheetPath;
				stylesheets[i].setAttribute("href", newHref);
			} else {
				stylesheets[i].remove();
			}
		}
	}

	if (!isFound) {
		const link = document.createElement("link");
		link.rel = "stylesheet";
		link.href = `_content/MudBlazor.Markdown/${codeStylesDir}/${stylesheetPath}`;

		document.head.appendChild(link);
	}
}

// mathJAX
window.appendMathJaxScript = function (scriptId) {
	if (document.getElementById(scriptId)) {
		return;
	}

	const script = document.createElement("script");
	script.id = scriptId;
	script.type = "text/javascript";
	script.src = "_content/MudBlazor.Markdown/MudBlazor.Markdown.MathJax.min.js";

	document.head.appendChild(script);
}

window.refreshMathJaxScript = function () {
	try {
		MathJax.typeset();
	} catch (e) {
		// swallow since in some cases MathJax might not be initialized
	}
}

// MudBlazor.Markdown
window.MudBlazorMarkdown = {
	scrollToElementId: function (elementId) {
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
	},
	copyTextToClipboard: async function (text) {
		try {
			await navigator.clipboard.writeText(text);
			return true;
		} catch (e) {
			return false;
		}
	},
};
