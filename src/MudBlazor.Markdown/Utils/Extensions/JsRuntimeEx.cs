namespace MudBlazor;

internal static class JsRuntimeEx
{
	public static async ValueTask ScrollToAsync(this IJSRuntime @this, string elementId)
	{
		await @this.InvokeVoidAsync("scrollToElementId", elementId)
			.ConfigureAwait(false);
	}
}
