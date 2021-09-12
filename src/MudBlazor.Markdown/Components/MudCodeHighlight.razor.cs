using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	public sealed class MudCodeHighlight : MudComponentBase
	{
		private ElementReference _ref;

		[Inject]
		private IJSRuntime? Js { get; init; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			var i = 0;

			builder.OpenElement(i++, "pre");
			builder.OpenElement(i++, "code");

			builder.AddElementReferenceCapture(i++, x => _ref = x);
			builder.AddContent(i++, "var a = 124;");

			builder.CloseElement();
			builder.CloseElement();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender || Js == null)
				return;

			await Js.InvokeVoidAsync("highlightCodeElement", _ref, "cs")
				.ConfigureAwait(false);
		}
	}
}
