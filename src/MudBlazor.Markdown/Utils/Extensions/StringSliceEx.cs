namespace MudBlazor;

internal static class StringSliceEx
{
	[Obsolete]
	public static string ToLowerCaseString(this StringSlice @this)
	{
		var text = @this.Text;
		int start = @this.Start, length = @this.End - start + 1;

		if (text is null || length <= 0)
			return string.Empty;

		return string.Create(length, @this, static (span, @this) =>
		{
			for (var i = 0; i < span.Length; i++)
			{
				var @char = @this.Text[@this.Start + i];
				span[i] = char.ToLowerInvariant(@char);
			}
		});
	}
}
