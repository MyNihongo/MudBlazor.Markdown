using System.Buffers;
using System.Runtime.CompilerServices;

namespace MudBlazor;

internal static class FrozenSets
{
#if NET9_0_OR_GREATER
	public static readonly FrozenSet<string>.AlternateLookup<ReadOnlySpan<char>> EmptyLookup = FrozenSet<string>.Empty.GetAlternateLookup<ReadOnlySpan<char>>();
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FrozenSet<T> Create<T>(params ReadOnlySpan<T> source)
	{
#if NET9_0_OR_GREATER
		return [..source];
#else
		return source.ToArray().ToFrozenSet();
#endif
	}
}

internal static class SearchValuess
{
	public static SearchValues<char> Create(params ReadOnlySpan<char> values)
	{
		return SearchValues.Create(values);
	}
}
