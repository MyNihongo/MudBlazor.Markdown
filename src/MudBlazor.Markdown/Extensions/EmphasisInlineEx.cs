using Markdig.Syntax.Inlines;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	internal static class EmphasisInlineEx
	{
		public static bool TryGetEmphasisElement(this EmphasisInline emphasis, out string value)
		{
			value = emphasis.DelimiterChar switch
			{
				'*' => emphasis.DelimiterCount switch
				{
					1 => "i",
					2 => "b",
					_ => string.Empty
				},
				_ => string.Empty
			};

			return !string.IsNullOrEmpty(value);
		}
	}
}
