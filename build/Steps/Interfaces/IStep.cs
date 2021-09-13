using System.Threading.Tasks;
using MudBlazor.Markdown.Build.Models;

namespace MudBlazor.Markdown.Build.Steps.Interfaces
{
	public interface IStep
	{
		ValueTask ProcessFileAsync(string filePath, ProjectDirs dirs);

		ValueTask CompleteAsync(ProjectDirs dirs);
	}
}
