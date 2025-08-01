using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

internal sealed class MudCodeHighlightCopyButton : ComponentBase
{
	[Parameter]
	public string? Class { get; init; }

	[Parameter]
	public string? Text { get; init; }

	[Inject]
	private IJSRuntime Js { get; init; } = null!;

	[Inject]
	private IServiceProvider? ServiceProvider { get; init; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var elementIndex = 0;

		builder.OpenComponent<MudIconButton>(elementIndex++);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Icon), Icons.Material.Rounded.ContentCopy);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Variant), Variant.Filled);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Color), Color.Primary);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Size), Size.Medium);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Class), Class);
		builder.AddComponentParameter(elementIndex, nameof(MudIconButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, CopyTextToClipboardAsync));
		builder.CloseComponent();
	}

	private async Task CopyTextToClipboardAsync(MouseEventArgs args)
	{
		if (string.IsNullOrEmpty(Text))
			return;

		var ok = await Js.CopyTextToClipboardAsync(Text)
			.ConfigureAwait(false);

		if (ok)
			return;

		var clipboardService = ServiceProvider?.GetService<IMudMarkdownClipboardService>();

		if (clipboardService != null)
		{
			await clipboardService.CopyToClipboardAsync(Text)
				.ConfigureAwait(false);
		}
	}
}
