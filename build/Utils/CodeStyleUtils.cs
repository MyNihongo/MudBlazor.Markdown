using System.IO;
using MudBlazor.Markdown.Build.Models;

namespace MudBlazor.Markdown.Build.Utils
{
	public static class CodeStyleUtils
	{
		public static string CreateDestinationRelativePath(string filePath, ProjectDirs dirs, string fileExtension, bool includeBaseDir)
		{
			var dstFileRelativePath = filePath[(dirs.CodeStyleDir.Length + 1)..];
			var dstDirectory = Path.GetDirectoryName(dstFileRelativePath) ?? string.Empty;
			var fileName = Path.GetFileNameWithoutExtension(dstFileRelativePath);

			if (!string.IsNullOrEmpty(dstDirectory))
				dstDirectory = AdjustOutputDirectories(dstDirectory);

			fileName += fileExtension;

			return includeBaseDir
				? Path.Combine("code-styles", dstDirectory, fileName)
				: Path.Combine(dstDirectory, fileName);
		}

		private static string AdjustOutputDirectories(string directories)
		{
			var stringBuilder = Program.StringBuilderPool.Get();
			stringBuilder.Append(char.ToLower(directories[0]));

			for (var i = 1; i < directories.Length; i++)
			{
				if (char.IsUpper(directories[i]))
				{
					stringBuilder.Append('-');
					stringBuilder.Append(char.ToLower(directories[i]));
				}
				else
				{
					stringBuilder.Append(directories[i]);
				}
			}

			return stringBuilder.ToString();
		}
	}
}
