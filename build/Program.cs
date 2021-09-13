using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Markdown.Build.Models;
using MudBlazor.Markdown.Build.Steps;
using MudBlazor.Markdown.Build.Utils;

namespace MudBlazor.Markdown.Build
{
	internal class Program
	{
		private static async Task Main()
		{
			var dirs = GetCodeProjectDirs();

			var enumClassDeclaration = await ProcessCodeStyles.CreateEnumDeclaration(dirs)
				.ConfigureAwait(false);

			await using var stream = FileUtils.Open(dirs.EnumFilePath, FileMode.Create);
			await using var writer = new StreamWriter(stream);

			await writer.WriteAsync(enumClassDeclaration)
				.ConfigureAwait(false);
		}

		private static ProjectDirs GetCodeProjectDirs()
		{
			var pathSplit = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
			var sb = new StringBuilder();

			for (var i = pathSplit.Length - 1; i >= 0; i--)
			{
				if (pathSplit[i] != "build")
					continue;

				for (var j = 0; j < i; j++)
				{
					if (j != 0)
						sb.Append(Path.DirectorySeparatorChar);

					sb.Append(pathSplit[j]);
				}

				break;
			}

			var projDir = Path.Combine(sb.ToString(), "src", "MudBlazor.Markdown");

			string codeStyleDir = Path.Combine(projDir, "Resources", "CodeStyles"),
				enumFilePath = Path.Combine(projDir, "Enums", "CodeBlockTheme.cs"),
				outputDir = Path.Combine(projDir, "wwwroot");

			return new ProjectDirs(codeStyleDir, enumFilePath, outputDir);
		}
	}
}
