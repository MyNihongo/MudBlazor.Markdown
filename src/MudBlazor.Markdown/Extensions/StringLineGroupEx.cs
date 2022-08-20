namespace MudBlazor;

internal static class StringLineGroupEx
{
	public static bool StartsAndEndsWith(this StringLineGroup @this, in string startsWith, in string endsWith)
	{
		if (@this.Count == 0)
			return false;

		// Starts with
		if (@this.Lines[0].Slice.Length < startsWith.Length)
			return false;

		for (var i = 0; i < startsWith.Length; i++)
			if (@this.Lines[0].Slice[i] != startsWith[i])
				return false;

		// Ends with
		var lastLine = @this.Lines[@this.Count - 1];
		if (lastLine.Slice.Length < endsWith.Length)
			return false;

		for (int i = endsWith.Length - 1, j = lastLine.Slice.Length - 1; i >= 0; i--, j--)
			if (endsWith[i] != lastLine.Slice[lastLine.Position + j])
				return false;

		return true;
	}
}
