namespace MudBlazor;

/// <summary>
/// A node produced by an <see cref="ICodeHighlighter"/>.<br/>
/// Either raw <see cref="CodeText"/> or a <see cref="CodeSpan"/> that wraps children in a
/// <c>&lt;span&gt;</c> element carrying a token CSS class.
/// </summary>
internal abstract class CodeNode;

/// <summary>
/// Plain text that is rendered (and HTML-escaped) as-is.
/// </summary>
internal sealed class CodeText(string value) : CodeNode
{
	public string Value { get; } = value;
}

/// <summary>
/// A highlighted token rendered as <c>&lt;span class="<see cref="ClassName"/>"&gt;<see cref="Children"/>&lt;/span&gt;</c>.
/// </summary>
internal sealed class CodeSpan(string className, IReadOnlyList<CodeNode> children) : CodeNode
{
	public string ClassName { get; } = className;

	public IReadOnlyList<CodeNode> Children { get; } = children;
}
