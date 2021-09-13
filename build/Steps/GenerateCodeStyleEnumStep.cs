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
		private readonly StringBuilder _stringBuilder = new();

		public GenerateCodeStyleEnumStep()
		{
			_stringBuilder.AppendLine("// ReSharper disable once CheckNamespace")
				.AppendLine("namespace MudBlazor")
				.AppendLine("{")
				.AppendLine("\tpublic enum CodeBlockTheme : ushort")
				.AppendLine("\t{")
				.Append("\t\tDefault = 0");
		}

		public ValueTask ProcessFileAsync(string filePath, ProjectDirs dirs)
		{
			if (Path.GetExtension(filePath) != ".css")
				return ValueTask.CompletedTask;

			var fileName = Path.GetFileNameWithoutExtension(filePath);

			_stringBuilder.AppendLine(",");
			_stringBuilder.Append("\t\t");
			_stringBuilder.Append(char.ToUpper(fileName[0]));

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
					_stringBuilder.Append(char.ToUpper(fileName[i]));
				}
				else
				{
					_stringBuilder.Append(fileName[i]);
				}
			}

			var parentDirName = Path.GetFileName(Path.GetDirectoryName(filePath));
			if (parentDirName != Program.CodeStylesDir)
				_stringBuilder.Append(parentDirName);

			return ValueTask.CompletedTask;
		}

		public async ValueTask CompleteAsync(ProjectDirs dirs)
		{
			var enumString = _stringBuilder
				.AppendLine()
				.AppendLine("\t}")
				.Append('}')
				.ToString();

			var enumDstPath = Path.Combine(dirs.ProjectDir, "Enums", "CodeBlockTheme.cs");

			await FileUtils.WriteAsync(enumDstPath, enumString)
				.ConfigureAwait(false);
		}
	}
}
