using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.ObjectPool;
using MudBlazor.Markdown.Build.Models;
using MudBlazor.Markdown.Build.Steps;
using MudBlazor.Markdown.Build.Steps.Interfaces;
using MudBlazor.Markdown.Build.Utils;

namespace MudBlazor.Markdown.Build
{
	internal class Program
	{
		public const string CodeStylesDir = "CodeStyles", CssOutputExtension = ".min.css";
		public const char HtmlPathSeparatorChar = '/';

		public static readonly ObjectPool<StringBuilder> StringBuilderPool = new DefaultObjectPoolProvider()
			.CreateStringBuilderPool();

		private static async Task Main()
		{
			var steps = new IStep[]
			{
				new GenerateCodeStyleEnumStep(),
				new BundleCodeStyleStep()
			};

			var dirs = GetCodeProjectDirs();

			await steps.ProcessAsync(dirs)
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

			string codeStyleDir = Path.Combine(projDir, "Resources", CodeStylesDir),
				outputDir = Path.Combine(projDir, "wwwroot");

			return new ProjectDirs(projDir, codeStyleDir, outputDir);
		}
	}
}
