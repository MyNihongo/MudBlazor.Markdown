using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace MudBlazor;

internal sealed class MudMarkdownMemoryCache : IMudMarkdownMemoryCache
{
#if NET9_0_OR_GREATER
	private readonly Lock _lock = new();
#elif NET8_0
	private readonly object _lock = new();
#endif
	private readonly Dictionary<string, Entry> _memoryCache = new();
	private readonly long _ttl;

	public MudMarkdownMemoryCache(IOptions<MudMarkdownMemoryCacheOptions> options)
	{
		_ttl = Convert.ToInt64(options.Value.TimeToLive.TotalSeconds);
	}

	public bool TryGetValue(in string key, out string value)
	{
		Entry? entry;
		lock (_lock)
		{
			if (!_memoryCache.TryGetValue(key, out entry))
				goto False;
		}

		var currentTime = CurrentUnixTimeSeconds();
		if (entry.ExpiresAt >= currentTime)
		{
			value = entry.Value;
			return true;
		}

		lock (_lock)
		{
			_memoryCache.Remove(key);
		}

		False:
		value = string.Empty;
		return false;
	}

	public void Set(in string key, in string value)
	{
		lock (_lock)
		{
			ref var entryRef = ref CollectionsMarshal.GetValueRefOrAddDefault(_memoryCache, key, out _);
			if (entryRef is null)
			{
				entryRef = new Entry(value)
				{
					ExpiresAt = GetExpiresAt(),
				};
			}
			else
			{
				entryRef.ExpiresAt = GetExpiresAt();
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private long GetExpiresAt() =>
		CurrentUnixTimeSeconds() + _ttl;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static long CurrentUnixTimeSeconds() =>
		DateTimeOffset.UtcNow.ToUnixTimeSeconds();

	private sealed class Entry
	{
		public readonly string Value;
		public long ExpiresAt;

		public Entry(in string value)
		{
			Value = value;
		}
	}
}
