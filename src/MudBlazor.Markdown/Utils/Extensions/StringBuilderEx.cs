using System.Text;

namespace MudBlazor;

internal static class StringBuilderEx
{
	public static StringBuilder AppendLowerCase(this StringBuilder @this, in ReadOnlySpan<char> span, in int endIndex)
	{
		for (var i = 0; i < endIndex; i++)
			@this.Append(char.ToLowerInvariant(span[i]));

		return @this;
	}

	public static void AppendLowerCase(this StringBuilder @this, in ReadOnlySpan<char> span)
	{
		foreach (var @char in span)
			@this.Append(char.ToLowerInvariant(@char));
	}
}
