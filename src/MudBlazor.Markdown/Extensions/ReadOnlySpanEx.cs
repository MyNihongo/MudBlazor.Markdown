namespace MudBlazor;

internal static class ReadOnlySpanEx
{
	public static int IndexOf(this ReadOnlySpan<char> @this, in char @char, in int index)
	{
		for (var i = index; i < @this.Length; i++)
			if (@this[i] == @char)
				return i;

		return -1;
	}
}
