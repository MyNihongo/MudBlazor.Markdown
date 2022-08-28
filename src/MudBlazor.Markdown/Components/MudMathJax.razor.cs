namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private int _elementIndex;

	[Parameter]
	public StringSlice Value { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		_elementIndex = 0;

		
	}
}
