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

		var stringBuilder = new StringBuilder();
		var headingId = BuildHeadingId(@this.Inline, stringBuilder);
		var headingText = BuildHeadingText(@this.Inline, stringBuilder);
		return new HeadingContent(headingId, headingText);
	}

	private static string BuildHeadingId(in ContainerInline containerInline, in StringBuilder stringBuilder)
	{
		stringBuilder.Clear();

		foreach (var inline in containerInline)
		{
			if (inline is not LiteralInline literalInline || literalInline.Content.IsEmpty)
				continue;

			var span = literalInline.Content.AsSpan();

			while (!span.IsEmpty)
			{
				var endIndex = span.IndexOfAny(SplitChars);
				if (endIndex < 0)
					break;

				if (endIndex > 0)
				{
					stringBuilder
						.AppendLowerCase(span, endIndex)
						.Append(JoinChar);
				}

				span = span[(endIndex + 1)..];
			}

			stringBuilder.AppendLowerCase(span);
		}

		return WebUtility.UrlEncode(stringBuilder.ToString());
	}

	private static string BuildHeadingText(in ContainerInline containerInline, in StringBuilder stringBuilder)
	{
		stringBuilder.Clear();

		foreach (var inline in containerInline)
		{
			if (inline is not LiteralInline literalInline || literalInline.Content.IsEmpty)
				continue;

			var span = literalInline.Content.AsSpan();
			stringBuilder.Append(span);
		}

		return stringBuilder.ToString();
	}
}
