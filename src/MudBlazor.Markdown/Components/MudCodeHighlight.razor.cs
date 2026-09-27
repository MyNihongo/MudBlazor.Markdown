namespace MudBlazor;

/// <summary>
/// Represents a component that renders formatted, syntax-highlighted code blocks with optional copying and styling features.
/// </summary>
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

	/// <summary>
	/// Specifies the display behavior and visibility condition for the copy button within the code block.
	/// Defaults to <see cref="CodeBlockCopyButton.OnHover"/>.
	/// </summary>
	[Parameter]
	public CodeBlockCopyButton CopyButton { get; set; } = CodeBlockCopyButton.OnHover;

	/// <summary>
	/// Gets or sets the text displayed on the copy button after the code has been successfully copied to the clipboard.
	/// </summary>
	[Parameter]
	public string? CopyButtonDisplayTextCopied { get; set; }

	/// <summary>
	/// Gets or sets the typography variant used for styling the text content within the code block.
	/// </summary>
	[Parameter]
	public Typo Typo { get; set; }

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
		builder.OpenComponent<MudText>(elementIndex++);
		builder.AddAttribute(elementIndex++, nameof(MudText.HtmlTag), "pre");
		builder.AddAttribute(elementIndex++, nameof(MudText.Typo), Typo.h5);

		builder.AddComponentParameter(elementIndex, nameof(MudText.ChildContent), (RenderFragment)(builder1 =>
		{
			var elementIndex1 = 0;
			builder1.OpenElement(elementIndex1++, "code");
			builder1.AddAttribute(elementIndex1++, "class", CodeClasses);

			var highlighter = CodeHighlighterFactory.Create(Language);
			if (highlighter is not null && !string.IsNullOrEmpty(Text))
			{
				var nodes = highlighter.Highlight(Text);
				RenderNodes(builder1, ref elementIndex1, nodes);
			}
			else
			{
				builder1.AddContent(elementIndex1, Text);
			}

			builder1.CloseElement(); // "code"
		}));

		builder.CloseComponent(); // "pre"
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
