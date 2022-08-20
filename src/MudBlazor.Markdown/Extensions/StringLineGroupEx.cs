namespace MudBlazor;

internal static class StringLineGroupEx
{
	public static bool StartsAndEndsWith(this StringLineGroup @this, in string startsWith, in string endsWith, out StringLineGroupRange range)
	{
		range = new StringLineGroupRange();

		if (@this.Count == 0)
			return false;

		const int firstLineIndex = 0;
		var lastLineIndex = @this.Count - 1;

		// Starts with
		if (@this.Lines[firstLineIndex].Slice.Length < startsWith.Length)
			return false;

		var startIndex = 0;
		for (;startIndex < startsWith.Length; startIndex++)
			if (@this.Lines[firstLineIndex].Slice[startIndex] != startsWith[startIndex])
				return false;

		// Ends with
		var lastLine = @this.Lines[lastLineIndex];
		if (lastLine.Slice.Length < endsWith.Length)
			return false;

		var endIndex = lastLine.Slice.Length - 1;
		for (var i = endsWith.Length - 1; i >= 0; i--, endIndex--)
			if (endsWith[i] != lastLine.Slice[lastLine.Position + endIndex])
				return false;

		range = new StringLineGroupRange(
			new StringLineGroupIndex(firstLineIndex, startIndex),
			new StringLineGroupIndex(lastLineIndex, endIndex));

		return true;
	}
}
