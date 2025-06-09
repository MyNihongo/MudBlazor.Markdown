using System.Buffers;
using System.Text;
using System.Web;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MudBlazor;

internal static class HeadingBlockEx
{
	private const char JoinChar = '-';
	private static readonly string[] EscapeChars = ["+", ":", "&"];

	private static readonly SearchValues<char> SplitChars = SearchValues.Create(" ");

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

				stringBuilder
					.AppendLowerCase(span, endIndex)
					.Append(JoinChar);

				span = span[(endIndex + 1)..];
			}

			stringBuilder.AppendLowerCase(span);
		}

		return stringBuilder.ToString();
	}

	private static string GetHeadingIdContent(this Inline @this)
	{
		var sliceString = @this.GetInlineContent(toLowerCase: true);
		return PrepareHeadingIdContent(sliceString);
	}

	private static string PrepareHeadingIdContent(this string @this)
	{
		var words = @this.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		var str = string.Join(JoinChar, words);

		for (var i = 0; i < EscapeChars.Length; i++)
			str = str.Replace(EscapeChars[i], string.Empty);

		return HttpUtility.UrlEncode(str);
	}

	private static string BuildHeadingText(in ContainerInline containerInline, in StringBuilder stringBuilder)
	{
		stringBuilder.Clear();

		var slices = containerInline
			.Select(static x => x.GetInlineContent())
			.Where(static x => x.Length > 0);

		return string.Join(' ', slices);
	}

	private static string GetInlineContent(this Inline @this, bool toLowerCase = false)
	{
		var slice = @this switch
		{
			LiteralInline x => x.Content,
			_ => StringSlice.Empty,
		};

		return toLowerCase
			? slice.ToLowerCaseString()
			: slice.ToString();
	}
}
