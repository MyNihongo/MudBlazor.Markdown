using System.Threading.Tasks;

namespace MudBlazor.Markdown.Core.Services.Interfaces
{
	public interface IMarkdownService
	{
		Task<string> GetSampleAsync();
	}
}
