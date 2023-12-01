namespace MudBlazor.Markdown.Maui.Services;

public sealed class MauiClipboardService : IMudMarkdownClipboardService
{
	public async ValueTask CopyToClipboardAsync(string text)
	{
		await Clipboard.Default.SetTextAsync(text)
			.ConfigureAwait(false);
	}
}
