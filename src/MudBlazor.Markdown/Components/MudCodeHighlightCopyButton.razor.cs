using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

internal sealed class MudCodeHighlightCopyButton : ComponentBase
{
	private static readonly TimeSpan IsCopiedResetTimeSpan = TimeSpan.FromSeconds(1.25d);
	private bool _isCopied;

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
		if (_isCopied)
		{
            builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Icon), Icons.Material.Rounded.DoneAll);
            builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Color), Color.Success);
		}
		else
		{
            builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Icon), Icons.Material.Rounded.ContentCopy);
            builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Color), Color.Primary);
            builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, CopyTextToClipboardAsync));
        }

		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Variant), Variant.Filled);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Size), Size.Medium);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Class), Class);
		builder.CloseComponent();
	}

	private async Task CopyTextToClipboardAsync(MouseEventArgs args)
	{
		if (string.IsNullOrEmpty(Text) || _isCopied)
			return;

		var ok = await Js.CopyTextToClipboardAsync(Text)
			.ConfigureAwait(false);

		if (!ok)
		{
			var clipboardService = ServiceProvider?.GetService<IMudMarkdownClipboardService>();

			if (clipboardService != null)
			{
				await clipboardService.CopyToClipboardAsync(Text)
					.ConfigureAwait(false);
			}
		}

		_isCopied = true;
		await InvokeAsync(StateHasChanged);
		await Task.Delay(IsCopiedResetTimeSpan);

		_isCopied = false;
		await InvokeAsync(StateHasChanged);
	}
}
