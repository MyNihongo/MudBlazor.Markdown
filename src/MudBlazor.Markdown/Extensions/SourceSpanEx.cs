using Markdig.Syntax;

namespace MudBlazor;

internal static class SourceSpanEx
{
	public static string TryGetText(this SourceSpan @this, in string originalText)
	{
		if (@this.Start >= originalText.Length)
			return string.Empty;

		var length = Math.Min(@this.Length, originalText.Length - @this.Start);
		return originalText.Substring(@this.Start, length);
	}
}
