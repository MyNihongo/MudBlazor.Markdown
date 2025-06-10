using System.Buffers;
using System.Net;
using System.Text;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MudBlazor;

internal static class HeadingBlockEx
{
	private const char JoinChar = '-', SpaceChar = ' ';
	private static readonly SearchValues<char> SplitChars = SearchValues.Create([SpaceChar]);

	public static HeadingContent? BuildHeadingContent(this HeadingBlock @this)
	{
		if (@this.Inline == null)
			return null;

		StringBuilder headingId = new(), headingText = new();

		foreach (var inline in @this.Inline)
		{
			if (inline is not LiteralInline literalInline || literalInline.Content.IsEmpty)
				continue;

			var span = literalInline.Content.AsSpan();
			headingText.Append(span);

			while (!span.IsEmpty)
			{
				var endIndex = span.IndexOfAny(SplitChars);
				if (endIndex < 0)
					break;

				if (endIndex > 0)
				{
					headingId
						.AppendLowerCase(span, endIndex)
						.Append(JoinChar);
				}

				span = span[(endIndex + 1)..];
			}

			headingId.AppendLowerCase(span);
		}

		return new HeadingContent(
			id: WebUtility.UrlEncode(headingId.ToString()),
			text: headingText.ToString()
		);
	}
}
