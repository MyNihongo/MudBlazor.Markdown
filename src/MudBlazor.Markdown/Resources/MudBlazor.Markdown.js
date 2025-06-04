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
		result = language ? hljs.highlight(text, {language}) : hljs.highlightAuto(text);
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
	scrollToElementId: function (elementId, dotNetReference) {
		const element = document.getElementById(elementId);
		if (!element) {
			return;
		}

		trySetActiveElementId(elementId);
		if (dotNetReference) {
			dotNetReference.invokeMethodAsync("OnActiveElementChangedAsync", elementId);
		}
		
		MudBlazorMarkdown.tableOfContents.scrollLock = true;

		element.scrollIntoView({
			behavior: "smooth",
			block: "start",
			inline: "nearest"
		});
		
		setTimeout(() => {
			MudBlazorMarkdown.tableOfContents.scrollLock = false;
		}, 1000);
	},
	copyTextToClipboard: async function (text) {
		try {
			await navigator.clipboard.writeText(text);
			return true;
		} catch (e) {
			return false;
		}
	},
	tableOfContents: {
		scrollLock: false,
		handleRefs: {},
		activeElementIds: {},
		startScrollSpy: function (elementId, dotNetReference) {
			if (!elementId) {
				return;
			}

			const element = document.getElementById(elementId);
			if (!element) {
				return;
			}

			const headingElements = element.querySelectorAll('.mud-markdown-toc-heading');
			if (!headingElements.length) {
				return;
			}

			const appBar = document.querySelector(".mud-appbar");
			const pageTop = appBar?.getBoundingClientRect().height ?? 0;

			const handler = () => {
				if (MudBlazorMarkdown.tableOfContents.scrollLock) {
					return;
				}
				
				let maxVisibility = -Number.MAX_VALUE, maxVisibilityElementId = undefined;
				for (const headingElement of headingElements) {
					const rect = headingElement.getBoundingClientRect();
					const relativeVisibility = rect.top - pageTop;

					if (relativeVisibility > 0 || relativeVisibility < maxVisibility) {
						continue;
					}

					maxVisibility = relativeVisibility;
					maxVisibilityElementId = headingElement.id;
				}

				if (!maxVisibilityElementId) {
					maxVisibilityElementId = headingElements[0]?.id;
				}

				const currentActiveElementId = this.activeElementIds[elementId];
				if (maxVisibilityElementId !== currentActiveElementId) {
					this.activeElementIds[elementId] = maxVisibilityElementId;
					trySetActiveElementId(maxVisibilityElementId);
					dotNetReference.invokeMethodAsync("OnActiveElementChangedAsync", maxVisibilityElementId);
				}
			};

			this.handleRefs[elementId] = handler;
			document.addEventListener('scroll', handler, true);
			document.addEventListener('resize', handler, true);
			handler();
		},
		stopScrollSpy: function (identifier) {
			if (!identifier) {
				return;
			}

			const handler = this.handleRefs[identifier];
			if (!handler) {
				return;
			}

			document.removeEventListener('scroll', handler, true);
			window.removeEventListener('resize', handler, true);
			delete this.handleRefs[identifier];
			delete this.activeElementIds[identifier];
		},
	},
};

function trySetActiveElementId(elementId) {
	const activeElementIdHref = `#${elementId}`;
	if (!window.location.pathname.endsWith(activeElementIdHref)) {
		history.replaceState(null, "", window.location.pathname + activeElementIdHref);
	}
}
