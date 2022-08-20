using System.Text;
using Markdig.Syntax;

namespace MudBlazor;

internal static class HtmlBlockEx
{
	public static bool TryGetDetails(this HtmlBlock @this, out HtmlDetailsData htmlDetailsData)
	{
		htmlDetailsData = new HtmlDetailsData();

		if (!@this.Lines.StartsAndEndsWith("<details>", "</details>"))
			return false;

		StringBuilder summaryBuilder = new(), detailsBuilder = new();
		bool isSummary = false, isDetail = false;

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
