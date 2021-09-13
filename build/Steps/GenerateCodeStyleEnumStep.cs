using System.IO;
using System.Text;
using System.Threading.Tasks;
using MudBlazor.Markdown.Build.Models;
using MudBlazor.Markdown.Build.Steps.Interfaces;
using MudBlazor.Markdown.Build.Utils;

namespace MudBlazor.Markdown.Build.Steps
{
	public sealed class GenerateCodeStyleEnumStep : IStep
	{
		private readonly StringBuilder _declarationBuilder = new(),
			_extensionBuilder = new();

		private const string EnumName = "CodeBlockTheme",
			EnumExName = EnumName + "Ex";

		public GenerateCodeStyleEnumStep()
		{
			_declarationBuilder
				.AppendLine("// ReSharper disable once CheckNamespace")
				.AppendLine("namespace MudBlazor")
				.AppendLine("{")
				.AppendFormat("\tpublic enum {0} : ushort", EnumName).AppendLine()
				.AppendLine("\t{")
				.Append("\t\tDefault = 0");

			_extensionBuilder
				.AppendLine("// ReSharper disable once CheckNamespace")
				.AppendLine("namespace MudBlazor")
				.AppendLine("{")
				.AppendFormat("\tinternal static class {0}", EnumExName).AppendLine()
				.AppendLine("\t{")
				.AppendFormat("\t\tpublic static string GetStylesheetPath(this {0} @this) =>", EnumName).AppendLine()
				.AppendLine("\t\t\t@this switch")
				.AppendLine("\t\t\t{");
		}

		public ValueTask ProcessFileAsync(string filePath, ProjectDirs dirs)
		{
			if (Path.GetExtension(filePath) != ".css")
				return ValueTask.CompletedTask;

			var fileName = Path.GetFileNameWithoutExtension(filePath);
			var parentDirName = Path.GetFileName(Path.GetDirectoryName(filePath));
			var enumName = CreateEnumName(fileName, parentDirName);

			_extensionBuilder.Append("\t\t\t\t");
			_extensionBuilder.AppendFormat(EnumName + ".{0} => \"{1}\"", enumName, CreateDestinationPath(filePath, dirs));
			_extensionBuilder.AppendLine(",");

			if (fileName == "default")
				return ValueTask.CompletedTask;

			_declarationBuilder.AppendLine(",");
			_declarationBuilder.Append("\t\t");
			_declarationBuilder.Append(enumName);

			return ValueTask.CompletedTask;
		}

		public async ValueTask CompleteAsync(ProjectDirs dirs)
		{
			var enumString = _declarationBuilder
				.AppendLine()
				.AppendLine("\t}")
				.Append('}')
				.ToString();

			var enumExString = _extensionBuilder
				.AppendLine("\t\t\t\t_ => string.Empty")
				.AppendLine("\t\t\t};")
				.AppendLine("\t}")
				.Append('}')
				.ToString();

			string enumDstPath = Path.Combine(dirs.ProjectDir, "Enums", $"{EnumName}.cs"),
				enumExDstPath = Path.Combine(dirs.ProjectDir, "Extensions", $"{EnumExName}.cs");

			var tasks = new[]
			{
				FileUtils.WriteAsync(enumDstPath, enumString),
				FileUtils.WriteAsync(enumExDstPath, enumExString)
			};

			await Task.WhenAll(tasks)
				.ConfigureAwait(false);
		}

		private static string CreateEnumName(string fileName, string parentDir)
		{
			var sb = Program.StringBuilderPool.Get();
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

			if (parentDir != Program.CodeStylesDir)
				sb.Append(parentDir);

			return sb.ToString();
		}

		private static string CreateDestinationPath(string filePath, ProjectDirs dirs)
		{
			var relativePath = CodeStyleUtils.CreateDestinationRelativePath(filePath, dirs, Program.CssOutputExtension, false);

			if (Program.HtmlPathSeparatorChar != Path.DirectorySeparatorChar)
				relativePath = relativePath.Replace(Path.DirectorySeparatorChar, Program.HtmlPathSeparatorChar);

			return relativePath;
		}
	}
}
