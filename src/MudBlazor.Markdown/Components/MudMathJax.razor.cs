namespace MudBlazor;

/// <summary>
/// Renders TeX math as MathML which is displayed by the browser natively
/// </summary>
internal sealed class MudMathJax : ComponentBase
{
	[Parameter]
	public string Delimiter { get; set; } = string.Empty;

	[Parameter]
	public StringSlice Value { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var isDisplay = Delimiter.Length > 1;
		TexToMathMl.Render(builder, 0, Value, isDisplay);
	}
}
