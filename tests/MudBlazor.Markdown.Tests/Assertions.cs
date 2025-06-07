using System.Runtime.CompilerServices;
using FluentAssertions.Primitives;

namespace MudBlazor.Markdown.Tests;

internal static class MudMarkdownMemoryCacheEx
{
#if NET9_0_OR_GREATER
	[OverloadResolutionPriority(1)]
#endif
	public static MudMarkdownMemoryCacheAssertions Should(this IMudMarkdownMemoryCache? @this)
	{
		return new MudMarkdownMemoryCacheAssertions(@this);
	}
}

internal sealed class MudMarkdownMemoryCacheAssertions : ObjectAssertions<IMudMarkdownMemoryCache?, MudMarkdownMemoryCacheAssertions>
{
	public MudMarkdownMemoryCacheAssertions(IMudMarkdownMemoryCache? value)
		: base(value)
	{
	}

	public void HaveEmptyCache()
	{
		throw new Exception();
	}

	public void HaveSingleCacheEntry(string key)
	{
		throw new Exception();
	}
}
