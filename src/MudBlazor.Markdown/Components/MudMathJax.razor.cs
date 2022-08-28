namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private const string ScriptId = "mudblazor-markdown-mathjax";

	[Parameter]
	public string Delimiter { get; set; } = string.Empty;

	[Parameter]
	public StringSlice Value { get; set; }

	[Inject]
	private IJSRuntime Js { get; init; } = default!;

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var elementIndex = 0;

		var delimiter = GetDelimiter(Delimiter);

		builder.AddContent(elementIndex++, delimiter.Start);
		builder.AddContent(elementIndex++, Value);
		builder.AddContent(elementIndex, delimiter.End);
	}

	private static MathDelimiter GetDelimiter(in string delimiter)
	{
		return delimiter switch
		{
			"$" => new MathDelimiter("\\(", "\\)"),
			"$$" => new MathDelimiter(delimiter),
			_ => new MathDelimiter(delimiter)
		};
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender)
			return;

		await Js.InvokeVoidAsync("appendMathJaxScript", ScriptId)
			.ConfigureAwait(false);
	}

	private readonly ref struct MathDelimiter
	{
		public MathDelimiter(string delimiter)
		{
			Start = End = delimiter;
		}

		public MathDelimiter(string start, string end)
		{
			Start = start;
			End = end;
		}

		public string Start { get; }
		
		public string End { get; }
	}
}
