using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	public class MudCodeHighlight : MudComponentBase
	{
		private ElementReference _ref;

		/// <summary>
		/// Code text to render
		/// </summary>
		[Parameter]
		public string Text { get; set; } = string.Empty;

		/// <summary>
		/// Language of the <see cref="Text"/>
		/// </summary>
		[Parameter]
		public string Language { get; set; } = string.Empty;

		[Inject]
		private IJSRuntime? Js { get; init; }

		protected override bool ShouldRender() =>
			!string.IsNullOrEmpty(Text);

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			var i = 0;

			builder.OpenElement(i++, "pre");
			builder.OpenElement(i++, "code");

			builder.AddElementReferenceCapture(i++, x => _ref = x);
			builder.AddContent(i++, Text);

			builder.CloseElement();
			builder.CloseElement();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			if (!firstRender || Js == null)
				return;

			await Js.InvokeVoidAsync("highlightCodeElement", _ref, Language)
				.ConfigureAwait(false);
		}
	}
}
