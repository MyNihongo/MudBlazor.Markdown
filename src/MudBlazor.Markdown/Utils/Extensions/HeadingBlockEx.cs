using System.Web;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace MudBlazor;

internal static class HeadingBlockEx
{
	private const char JoinChar = '-';

	public static HeadingContent? BuildHeadingContent(this HeadingBlock @this)
	{
		if (@this.Inline == null)
			return null;

		var headingId = BuildHeadingId(@this.Inline);
		var headingText = BuildHeadingText(@this.Inline);
		return new HeadingContent(headingId, headingText);
	}

	private static string BuildHeadingId(in ContainerInline inline)
	{
		var slices = inline
			.Select(static x => x.GetHeadingIdContent())
			.Where(static x => x.Length > 0);

		return string.Join(JoinChar, slices);
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

		return HttpUtility.UrlEncode(str);
	}

	private static string BuildHeadingText(in ContainerInline inline)
	{
		var slices = inline
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
