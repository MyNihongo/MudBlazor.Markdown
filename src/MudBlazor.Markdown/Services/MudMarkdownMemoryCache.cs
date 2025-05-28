using Microsoft.Extensions.Logging;

namespace MudBlazor;

internal sealed class MudMarkdownMemoryCache : MemoryCache, IMudMarkdownMemoryCache
{
	public MudMarkdownMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor)
		: base(optionsAccessor)
	{
	}

	public MudMarkdownMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor, ILoggerFactory loggerFactory)
		: base(optionsAccessor, loggerFactory)
	{
	}
}
