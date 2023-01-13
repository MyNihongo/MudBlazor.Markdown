using System.Reflection;

namespace MudBlazor.Markdown.Core.Services;

internal sealed class MarkdownService : IMarkdownService
{
	public Task<string> GetSampleAsync(MarkdownResourceType resourceType)
	{
		var resourceName = resourceType switch
		{
			MarkdownResourceType.Main => "sample",
			MarkdownResourceType.Enderal => "sample-enderal",
			MarkdownResourceType.Math => "sample-math",
			MarkdownResourceType.Issues => "sample-issues",
			_ => throw new ArgumentOutOfRangeException(nameof(resourceType), resourceType, $"Unknown {nameof(MarkdownResourceType)}: {resourceType}")
		};

		return GetMarkdownAsync(resourceName);
	}

	private static async Task<string> GetMarkdownAsync(string name)
	{
		var assembly = Assembly.GetExecutingAssembly();
		var resourceName = $"{assembly.GetName().Name}.{name}.md";

		await using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("Markdown resource not found");
		using var reader = new StreamReader(stream);

		return await reader.ReadToEndAsync()
			.ConfigureAwait(false);
	}
}
