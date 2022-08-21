using Markdig.Syntax;

namespace MudBlazor;

internal static class HtmlBlockEx
{
	public static bool TryGetDetails(this HtmlBlock @this, out HtmlDetailsData htmlDetailsData)
	{
		htmlDetailsData = new HtmlDetailsData();

		if (!@this.Lines.StartsAndEndsWith("<details>", "</details>", out var range))
			return false;

		var summaryEndIndex = @this.Lines.TryGetContent("<summary>", "</summary>", range.Start, out var summaryContent);
		if (summaryEndIndex.Line == -1)
			return false;

		//for (var i = 0; i < @this.Lines.Lines.Length; i++)
		//{
		//	@this.Lines.Lines[i].Slice
		//}

		//foreach (var stringLine in @this.Lines.Lines)
		//{
		//	Debug.WriteLine(stringLine);
		//}

		htmlDetailsData = new HtmlDetailsData();
		return false;
	}
}
