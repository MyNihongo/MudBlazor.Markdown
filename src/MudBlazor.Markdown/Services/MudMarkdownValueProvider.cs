using System.Text;

namespace MudBlazor;

internal sealed class MudMarkdownValueProvider : IMudMarkdownValueProvider
{
	private readonly IMemoryCache _memoryCache;
	private readonly MemoryCacheEntryOptions _memoryCacheEntryOptions;
	private static readonly HttpClient HttpClient = new();

	public MudMarkdownValueProvider(IMemoryCache memoryCache, IOptions<MudMarkdownMemoryCacheEntryOptions> options)
	{
		_memoryCache = memoryCache;
		_memoryCacheEntryOptions = options.Value;
	}

	public async ValueTask<string> GetValueAsync(string value, MarkdownSourceType sourceType, CancellationToken ct = default)
	{
		return sourceType switch
		{
			MarkdownSourceType.RawValue => value,
			MarkdownSourceType.File => await ReadFromFileAsync(value, ct).ConfigureAwait(false),
			MarkdownSourceType.Url => await ReadFromUrlAsync(value, ct).ConfigureAwait(false),
			_ => throw new ArgumentOutOfRangeException(nameof(sourceType), sourceType, $"Unknown {nameof(MarkdownSourceType)}, value=`{sourceType}`"),
		};
	}

	private async ValueTask<string> ReadFromFileAsync(string path, CancellationToken ct = default)
	{
		if (_memoryCache.TryGetValue<string>(path, out var value) && value is not null)
			return value;

		try
		{
			await using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);
			using var reader = new StreamReader(fileStream);

			value = await reader.ReadToEndAsync(ct)
				.ConfigureAwait(false);

			_memoryCache.Set(path, value, _memoryCacheEntryOptions);
			return value;
		}
		catch (Exception e)
		{
			return new StringBuilder()
				.Append($"Error while reading from file, path=`{path}`")
				.BuildErrorMessage(e);
		}
	}

	private async ValueTask<string> ReadFromUrlAsync(string url, CancellationToken ct = default)
	{
		if (_memoryCache.TryGetValue<string>(url, out var value) && value is not null)
			return url;

		try
		{
			value = await HttpClient.GetStringAsync(url, ct)
				.ConfigureAwait(false);

			_memoryCache.Set(url, value, _memoryCacheEntryOptions);
			return value;
		}
		catch (Exception e)
		{
			return new StringBuilder()
				.Append($"Error while reading from URL, URL=`{url}`")
				.BuildErrorMessage(e);
		}
	}
}

file static class StringBuilderEx
{
	public static string BuildErrorMessage(this StringBuilder @this, in Exception e)
	{
		return @this
			.AppendLine($", error=`{e.Message}`")
			.AppendLine("```txt")
			.AppendLine(e.ToString())
			.Append("```")
			.ToString();
	}
}
