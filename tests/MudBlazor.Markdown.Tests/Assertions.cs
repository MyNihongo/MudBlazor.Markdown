using System.Collections;
using System.Reflection;
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

internal sealed class MudMarkdownMemoryCacheAssertions(IMudMarkdownMemoryCache? value) : ObjectAssertions<IMudMarkdownMemoryCache?, MudMarkdownMemoryCacheAssertions>(value)
{
	private FieldInfo? _cacheField;

	public void HaveEmptyCache()
	{
		var memoryCache = GetMemoryCache();

		memoryCache.Count
			.Should()
			.Be(0);
	}

	public void HaveSingleCacheEntry(string key)
	{
		var memoryCache = GetMemoryCache();

		memoryCache.Keys.Count
			.Should()
			.Be(1);

		memoryCache.Keys
			.Cast<string>()
			.Single()
			.Should()
			.Be(key);
	}

	private IDictionary GetMemoryCache()
	{
		var field = GetMemoryCacheField();
		return (IDictionary?)field.GetValue(Subject) ?? throw new InvalidOperationException("Memory cache is null.");
	}

	private FieldInfo GetMemoryCacheField()
	{
		if (_cacheField is not null)
			return _cacheField;

		var cacheField = Subject?.GetType()
			.GetField("_memoryCache", BindingFlags.Instance | BindingFlags.NonPublic);

		return _cacheField = cacheField ?? throw new InvalidOperationException("Cache field not found.");
	}
}
