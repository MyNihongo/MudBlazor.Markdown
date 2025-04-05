using Markdig.Syntax.Inlines;

namespace MudBlazor;

internal static class EmphasisInlineEx
{
	public static bool TryGetEmphasisElement(this EmphasisInline emphasis, out string value)
	{
		const string italics = "i", bold = "b", strikethrough = "del";

		value = emphasis.DelimiterChar switch
		{
			'*' => emphasis.DelimiterCount switch
			{
				1 => italics,
				2 => bold,
				_ => string.Empty,
			},
			'_' => italics,
			'~' => emphasis.DelimiterCount switch
			{
				2 => strikethrough,
				_ => string.Empty,
			},
			_ => string.Empty,
		};

		return !string.IsNullOrEmpty(value);
	}
}
