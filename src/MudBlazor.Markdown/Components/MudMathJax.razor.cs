namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private int _elementIndex;

	[Parameter]
	public StringSlice Value { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		_elementIndex = 0;

		builder.OpenElement(_elementIndex++, "mjx-container");
		builder.AddAttribute(_elementIndex++, "tabindex", "0");
		builder.AddAttribute(_elementIndex++, "class", "mud-markdown-mjx-container");

		builder.AddMarkupContent(_elementIndex++, "<mi>x</mi><mo>=</mo><mi>y</mi>");

		builder.CloseComponent();
	}


}
