using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyNihongo.CodeThemeEnumGenerator.Models;
using MyNihongo.CodeThemeEnumGenerator.Utils;
using NUglify;
using NUglify.Css;

namespace MyNihongo.CodeThemeEnumGenerator
{
	internal class Program
	{
		private static CssSettings CssSettings = new()
		{
			CommentMode = CssComment.None
		};

		private static async Task Main()
		{
			var dirs = GetCodeProjectDirs();

			var enumClassDeclaration = await ProcessCssFileAsync(dirs)
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
				if (pathSplit[i] != "dev")
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

		private static async Task<string> ProcessCssFileAsync(ProjectDirs dirs)
		{
			var sb = new StringBuilder()
				.AppendLine("// ReSharper disable once CheckNamespace")
				.AppendLine("namespace MudBlazor")
				.AppendLine("{")
				.AppendLine("\tpublic enum CodeBlockTheme : ushort")
				.AppendLine("\t{")
				.Append("\t\tDefault = 0");

			await ProcessStylesAsync(sb, dirs.CodeStyleDir, string.Empty, dirs)
				.ConfigureAwait(false);

			foreach (var nestedPath in Directory.EnumerateDirectories(dirs.CodeStyleDir))
			{
				var suffix = Path.GetFileName(nestedPath);
				suffix = char.ToUpper(suffix[0]) + suffix[1..];

				await ProcessStylesAsync(sb, nestedPath, suffix, dirs)
					.ConfigureAwait(false);
			}

			return sb
				.AppendLine()
				.AppendLine("\t}")
				.Append('}')
				.ToString();
		}

		private static async Task ProcessStylesAsync(StringBuilder sb, string path, string suffix, ProjectDirs dirs)
		{
			foreach (var file in Directory.EnumerateFiles(path))
			{
				if (Path.GetExtension(file) != ".css")
					continue;

				var fileName = Path.GetFileNameWithoutExtension(file);

				if (fileName == "default")
					continue;

				await BundleStyleFileAsync(file, dirs)
					.ConfigureAwait(false);

				sb.AppendLine(",");
				sb.Append("\t\t");
				sb.Append(char.ToUpper(fileName[0]));

				var isUpperCase = false;
				for (var i = 1; i < fileName.Length; i++)
				{
					if (fileName[i] == '-')
					{
						isUpperCase = true;
						continue;
					}

					if (isUpperCase)
					{
						isUpperCase = false;
						sb.Append(char.ToUpper(fileName[i]));
					}
					else
					{
						sb.Append(fileName[i]);
					}
				}

				sb.Append(suffix);
			}
		}

		/// <remarks>
		///	Due to not being good at webpack I could not bundle these .css files with webpack :(
		/// Any help?
		/// </remarks>
		private static async Task BundleStyleFileAsync(string filePath, ProjectDirs dirs)
		{
			string styleContent;

			await using (var stream = FileUtils.Open(filePath, FileMode.Open))
			using (var reader = new StreamReader(stream))
			{
				styleContent = await reader.ReadToEndAsync()
					.ConfigureAwait(false);
			}

			styleContent = Uglify.Css(styleContent, CssSettings).Code;
			var a = "a";
		}
	}
}
