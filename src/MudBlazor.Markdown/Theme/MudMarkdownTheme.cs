namespace MudBlazor;

/// <summary>
/// Groups the Markdown-specific theming types.<br/>
/// Currently holds the code-highlight colors (<see cref="CodeHighlight"/>); more may be added in the future.
/// </summary>
public static class MudMarkdownTheme
{
	/// <summary>
	/// Set of colors that map source-code tokens to the highlighting displayed in a code block.<br/>
	/// Ready-made palettes are available in <see cref="MudCodeHighlightThemes"/>.
	/// </summary>
	public class CodeHighlight
	{
		/// <summary>
		/// Background color of the code block.
		/// </summary>
		public required MudColor Background { get; set; }

		/// <summary>
		/// Default foreground color, used for any token without a more specific color.
		/// </summary>
		public required MudColor Text { get; set; }

		/// <summary>
		/// Color of language keywords (e.g. <c>if</c>, <c>return</c>, <c>class</c>, <c>public</c>).
		/// </summary>
		public required MudColor Keyword { get; set; }

		/// <summary>
		/// Color of string and character literals.
		/// </summary>
		public required MudColor String { get; set; }

		/// <summary>
		/// Color of comments.
		/// </summary>
		public required MudColor Comment { get; set; }

		/// <summary>
		/// Color of function, method and other callable names.
		/// </summary>
		public required MudColor Function { get; set; }

		/// <summary>
		/// Color of type names such as classes, structs, interfaces and enums.
		/// </summary>
		public required MudColor Type { get; set; }

		/// <summary>
		/// Color of punctuation such as brackets, braces, semicolons and operators.
		/// </summary>
		public required MudColor Punctuation { get; set; }
	}
}
