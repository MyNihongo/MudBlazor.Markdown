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
		builder.AddAttribute(_elementIndex++, "jax", "CHTML");
		builder.AddAttribute(_elementIndex++, "tabindex", "0");
		builder.AddAttribute(_elementIndex++, "class", "mud-markdown-mjx-container");

		builder.OpenElement(_elementIndex++, "mjx-math");
		builder.AddAttribute(_elementIndex, "class", "MJX-TEX");
		builder.AddAttribute(_elementIndex, "aria-hidden", "true");
		builder.CloseElement();

		builder.OpenElement(_elementIndex++, "mjx-assistive-mml");
		builder.AddAttribute(_elementIndex++, "unselectable", "on");
		builder.AddAttribute(_elementIndex, "class", "mud-markdown-mjx-assistive-mml");
		builder.CloseElement();

		builder.CloseComponent();
	}


}
