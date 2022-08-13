namespace MudBlazor;

internal static class CharEx
{
	public static bool IsMathOperation(this char @this, out bool hasSpacing, out char? customChar)
	{
		hasSpacing = false;
		customChar = null;

		switch (@this)
		{
			case '-':
				customChar = '−';
				hasSpacing = true;
				return true;
			case '=' or '>' or '<' or '+' or '−':
				hasSpacing = true;
				return true;
			case '(' or ')':
				return true;
		}

		return false;
	}
}
