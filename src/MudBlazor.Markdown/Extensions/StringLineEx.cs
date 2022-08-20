namespace MudBlazor;

internal static class StringLineEx
{
	public static int IndexOf(this StringLine @this, string value)
	{
		const int notFoundIndex = -1;

		if (@this.Slice.Length < value.Length)
			return notFoundIndex;

		for (var i = 0; i < @this.Slice.Length - value.Length; i++)
		{
			var j = 0;
			for (; j < value.Length; j++)
			{
				if (@this.Slice[i + j] != value[j])
					break;
			}

			if (j == value.Length)
				return i;
		}

		return notFoundIndex;
	}
}
