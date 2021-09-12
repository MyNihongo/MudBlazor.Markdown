using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MyNihongo.CodeThemeEnumGenerator
{
	internal class Program
	{
		private static async Task Main()
		{
			var dirs = GetCodeProjectDirs();
			var enumClassDeclaration = CreateEnumClass(dirs.CodeStyleDir);

			await using var stream = new FileStream(dirs.EnumFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite, 4086, true);
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
			string codeStyleDir = Path.Combine(projDir, "wwwroot", "code-styles"),
				enumFilePath = Path.Combine(projDir, "Enums", "CodeBlockTheme.cs");

			return new ProjectDirs(codeStyleDir, enumFilePath);
		}

		private static string CreateEnumClass(string path)
		{
			var sb = new StringBuilder()
				.AppendLine("// ReSharper disable once CheckNamespace")
				.AppendLine("namespace MudBlazor")
				.AppendLine("{")
				.AppendLine("\tpublic enum CodeBlockTheme : ushort")
				.AppendLine("\t{")
				.AppendLine("\t\tDefault = 0,");

			return sb
				.AppendLine("\t}")
				.Append("}")
				.ToString();
		}
	}
}
