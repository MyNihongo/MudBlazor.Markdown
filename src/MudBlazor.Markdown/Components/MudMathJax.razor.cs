namespace MudBlazor;

internal sealed class MudMathJax : ComponentBase
{
	private const string ScriptId = "mudblazor-markdown-mathjax";

	[Parameter]
	public string Delimiter { get; set; } = string.Empty;

	[Parameter]
	public StringSlice Value { get; set; }

	[Inject]
	private IJSRuntime Js { get; init; } = null!;

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var delimiter = GetDelimiter(Delimiter);

		var elementIndex = 0;
		builder.AddContent(elementIndex++, delimiter.Start);
		builder.AddContent(elementIndex++, Value);
		builder.AddContent(elementIndex, delimiter.End);
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await Js.InvokeVoidAsync("appendMathJaxScript", ScriptId)
				.ConfigureAwait(false);
		}

		await Js.InvokeVoidAsync("refreshMathJaxScript")
			.ConfigureAwait(false);
	}

	private static MathDelimiter GetDelimiter(in string delimiter) =>
		delimiter switch
		{
			"$" => new MathDelimiter("\\(", "\\)"),
			_ => new MathDelimiter(delimiter),
		};

	private readonly ref struct MathDelimiter
	{
		public readonly string Start, End;

		public MathDelimiter(string delimiter)
		{
			Start = End = delimiter;
		}

		public MathDelimiter(string start, string end)
		{
			Start = start;
			End = end;
		}
	}
}
