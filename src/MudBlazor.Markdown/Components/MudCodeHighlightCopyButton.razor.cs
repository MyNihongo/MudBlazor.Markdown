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

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		var elementIndex1 = 0;

		if (_isCopied)
		{
			builder1.OpenComponent<MudTooltip>(elementIndex1++);
			builder1.AddComponentParameter(elementIndex1++, nameof(MudTooltip.Text), "Copied!");
			builder1.AddComponentParameter(elementIndex1++, nameof(MudTooltip.Arrow), true);
			builder1.AddComponentParameter(elementIndex1++, nameof(MudTooltip.Placement), Placement.Left);
			builder1.AddComponentParameter(elementIndex1++, nameof(MudTooltip.Visible), true);
            builder1.AddComponentParameter(elementIndex1++, nameof(MudTooltip.RootClass), Class);
            builder1.AddComponentParameter(elementIndex1, nameof(MudTooltip.ChildContent), (RenderFragment)(builder2 =>
			{
				var elementIndex2 = 0;
				builder2.OpenComponent<MudIconButton>(elementIndex2++);
				ApplyCopyButtonProperties(builder2, ref elementIndex2, Icons.Material.Rounded.DoneAll, Color.Success);
				builder2.CloseComponent();
			}));
			builder1.CloseComponent();
		}
		else
		{
			builder1.OpenComponent<MudIconButton>(elementIndex1++);
			builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, CopyTextToClipboardAsync));
            builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.Class), Class);
            ApplyCopyButtonProperties(builder1, ref elementIndex1, Icons.Material.Rounded.ContentCopy, Color.Primary);
			builder1.CloseComponent();
		}
	}

	private static void ApplyCopyButtonProperties(in RenderTreeBuilder builder, ref int elementIndex, in string icon, in Color color)
	{
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Icon), icon);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Color), color);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Variant), Variant.Filled);
		builder.AddComponentParameter(elementIndex++, nameof(MudIconButton.Size), Size.Medium);
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
