namespace MudBlazor;

internal static class JsRuntimeEx
{
	private const string JsNamespace = "MudBlazorMarkdown";

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
}
