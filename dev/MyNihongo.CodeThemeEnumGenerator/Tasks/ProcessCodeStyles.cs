using System.IO;
using System.Text;
using System.Threading.Tasks;
using MyNihongo.CodeThemeEnumGenerator.Models;

namespace MyNihongo.CodeThemeEnumGenerator.Tasks
{
	internal static class ProcessCodeStyles
	{
		public static async Task<string> CreateEnumDeclaration(ProjectDirs dirs)
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

				await BundleCodeStyle.BundleStyleFileAsync(file, dirs)
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
	}
}
