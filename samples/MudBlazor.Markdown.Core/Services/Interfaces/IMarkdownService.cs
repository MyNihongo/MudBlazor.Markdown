namespace MudBlazor.Markdown.Core.Services;

public interface IMarkdownService
{
	Task<string> GetSampleAsync(MarkdownResourceType resourceType);
}
