namespace MudBlazor;

public class MudCodeHighlight : MudComponentBase
{
	/// <summary>
	/// Code text to render
	/// </summary>
	[Parameter]
	public string? Text { get; set; }

	/// <summary>
	/// Language of the <see cref="Text"/>
	/// </summary>
	[Parameter]
	public string? Language { get; set; }

	/// <summary>
	/// Theme of the code block.<br/>
	/// Default is <see cref="CodeBlockTheme.Default"/>
	/// </summary>
	[Parameter]
	[Obsolete("`CodeBlockTheme` is obsolete and has no effect. Use `MudMarkdownThemeProvider` instead. For more details see https://github.com/MyNihongo/MudBlazor.Markdown/wiki/MudMarkdownThemeProvider")]
	public CodeBlockTheme Theme { get; set; }

	[Parameter]
	public CodeBlockCopyButton CopyButton { get; set; } = CodeBlockCopyButton.OnHover;

	[Parameter]
	public string? CopyButtonDisplayTextCopied { get; set; }

	private string CodeClasses => new CssBuilder()
		.AddClass("hljs")
		.AddClass(() => $"language-{Language}", () => !string.IsNullOrEmpty(Language))
		.Build();

	protected override bool ShouldRender() =>
		!string.IsNullOrEmpty(Text);

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var containerClass = "hljs mud-markdown-code-highlight";
		if (CopyButton == CodeBlockCopyButton.Sticky)
			containerClass += "-sticky";

		var elementIndex = 0;
		builder.OpenElement(elementIndex++, ElementNames.Div);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, containerClass);

		// Copy button
		if (CopyButton != CodeBlockCopyButton.None)
		{
			var copyButtonClass = "ma-2 mud-markdown-code-highlight-copybtn";

			if (CopyButton == CodeBlockCopyButton.Sticky)
				copyButtonClass += "-sticky";

			builder.OpenComponent<MudCodeHighlightCopyButton>(elementIndex++);
			builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlightCopyButton.Class), copyButtonClass);
			builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlightCopyButton.TextToCopy), Text);
			builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlightCopyButton.DisplayTextCopied), CopyButtonDisplayTextCopied);
			builder.CloseComponent();
		}

		// Code block
		builder.OpenElement(elementIndex++, "pre");
		builder.OpenElement(elementIndex++, "code");
		builder.AddAttribute(elementIndex++, "class", CodeClasses);

		var highlighter = CodeHighlighterFactory.Create(Language);
		if (highlighter is not null && !string.IsNullOrEmpty(Text))
		{
			var nodes = highlighter.Highlight(Text);
			RenderNodes(builder, ref elementIndex, nodes);
		}
		else
		{
			builder.AddContent(elementIndex, Text);
		}

		builder.CloseElement(); // "code"
		builder.CloseElement(); // "pre"

		builder.CloseElement(); // "div"
	}

	private static void RenderNodes(RenderTreeBuilder builder, ref int elementIndex, IReadOnlyList<CodeNode> nodes)
	{
		foreach (var node in nodes)
		{
			switch (node)
			{
				case CodeText text:
					builder.AddContent(elementIndex++, text.Value);
					break;
				case CodeSpan span:
					builder.OpenElement(elementIndex++, "span");
					builder.AddAttribute(elementIndex++, "class", span.ClassName);
					RenderNodes(builder, ref elementIndex, span.Children);
					builder.CloseElement();
					break;
			}
		}
	}
}
