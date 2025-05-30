namespace MudBlazor;

internal static class JsRuntimeEx
{
	private const string JsNamespace = "MudBlazorMarkdown", TableOfContentsNamespace = $"{JsNamespace}.tableOfContents";

	public static async ValueTask ScrollToAsync(this IJSRuntime @this, string elementId)
	{
		await @this.InvokeVoidAsync($"{JsNamespace}.scrollToElementId", elementId)
			.ConfigureAwait(false);
	}

	public static async ValueTask<bool> CopyTextToClipboardAsync(this IJSRuntime @this, string text)
	{
		return await @this.InvokeAsync<bool>($"{JsNamespace}.copyTextToClipboard", text)
			.ConfigureAwait(false);
	}

	public static async ValueTask StartScrollSpyAsync(this IJSRuntime @this, DotNetObjectReference<MudTableOfContentsNavMenu>? dotNetObjectReference, string elementId)
	{
		await @this.InvokeVoidAsync($"{TableOfContentsNamespace}.startScrollSpy", dotNetObjectReference, elementId)
			.ConfigureAwait(false);
	}

	public static async ValueTask StopScrollSpyAsync(this IJSRuntime @this, string identifier)
	{
		await @this.InvokeVoidAsync($"{TableOfContentsNamespace}.stopScrollSpy", identifier)
			.ConfigureAwait(false);
	}
}
