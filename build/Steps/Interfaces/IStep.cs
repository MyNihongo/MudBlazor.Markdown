using System.Threading.Tasks;
using MudBlazor.Markdown.Build.Models;

namespace MudBlazor.Markdown.Build.Steps.Interfaces
{
	public interface IStep
	{
		Task ProcessFileAsync(string filePath, ProjectDirs dirs);
	}
}
