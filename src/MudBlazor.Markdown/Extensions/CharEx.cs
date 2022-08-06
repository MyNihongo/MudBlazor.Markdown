namespace MudBlazor;

internal static class CharEx
{
	public static bool IsMathOperation(this char @this, out bool hasSpacing)
	{
		hasSpacing = false;

		switch (@this)
		{
			case '=' or '>' or '<' or '-' or '+':
				hasSpacing = true;
				return true;
			case '(' or ')':
				return true;
		}

		return false;
	}
}
