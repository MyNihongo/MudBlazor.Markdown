using System.Threading.Tasks;

namespace MudBlazor.Markdown.Core
{
	public interface IMarkdownService
	{
		Task<string> GetSampleAsync();
	}
}
