namespace MudBlazor;

internal static class CharEx
{
	public static bool IsMathOperation(this char @this) =>
		@this is '=' or '>' or '<' or '-' or '+';
}
