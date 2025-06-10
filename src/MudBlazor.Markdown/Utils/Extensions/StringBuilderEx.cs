using System.Runtime.CompilerServices;
using System.Text;

namespace MudBlazor;

internal static class StringBuilderEx
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static StringBuilder AppendLowerCase(this StringBuilder @this, in ReadOnlySpan<char> span, in int endIndex)
	{
		for (var i = 0; i < endIndex; i++)
			@this.Append(char.ToLowerInvariant(span[i]));

		return @this;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void AppendLowerCase(this StringBuilder @this, in ReadOnlySpan<char> span)
	{
		foreach (var @char in span)
			@this.Append(char.ToLowerInvariant(@char));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static StringBuilder Append(this StringBuilder @this, in ReadOnlySpan<char> span, in int endIndex)
	{
		for (var i = 0; i < endIndex; i++)
			@this.Append(span[i]);

		return @this;
	}
}
