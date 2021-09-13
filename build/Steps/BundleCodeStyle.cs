using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.ObjectPool;
using MudBlazor.Markdown.Build.Models;
using MudBlazor.Markdown.Build.Utils;
using NUglify;
using NUglify.Css;

namespace MudBlazor.Markdown.Build.Steps
{
	internal static class BundleCodeStyle
	{
		private static readonly CssSettings CssSettings = new()
		{
			CommentMode = CssComment.None
		};

		private static readonly ObjectPool<StringBuilder> StringBuilderPool = new DefaultObjectPoolProvider()
			.CreateStringBuilderPool();

		/// <remarks>
		///	Due to not being good at webpack I could not bundle these .css files with webpack :(
		/// Any help?
		/// </remarks>
		public static async Task BundleStyleFileAsync(string filePath, ProjectDirs dirs)
		{
			var fileExtension = Path.GetExtension(filePath);
			var task = fileExtension switch
			{
				".css" => BundleCssAsync(filePath, dirs),
				".png" => CopyFile(filePath, dirs, fileExtension),
				_ => Task.CompletedTask
			};

			await task.ConfigureAwait(false);
		}

		private static async Task BundleCssAsync(string filePath, ProjectDirs dirs)
		{
			string styleContent;

			await using (var stream = FileUtils.Open(filePath, FileMode.Open))
			using (var reader = new StreamReader(stream))
			{
				styleContent = await reader.ReadToEndAsync()
					.ConfigureAwait(false);
			}

			styleContent = Uglify.Css(styleContent, CssSettings).Code;
			var dstPath = CreateDestinationPath(filePath, dirs, ".min.css");

			await using (var stream = FileUtils.Open(dstPath, FileMode.Create))
			await using (var writer = new StreamWriter(stream))
			{
				await writer.WriteAsync(styleContent)
					.ConfigureAwait(false);
			}
		}

		private static Task CopyFile(string filePath, ProjectDirs dirs, string fileExtension)
		{
			var dstPath = CreateDestinationPath(filePath, dirs, fileExtension);
			File.Copy(filePath, dstPath, true);

			return Task.CompletedTask;
		}

		private static string CreateDestinationPath(string filePath, ProjectDirs dirs, string fileExtension)
		{
			var dstFileRelativePath = filePath[(dirs.CodeStyleDir.Length + 1)..];
			var dstDirectory = Path.GetDirectoryName(dstFileRelativePath) ?? string.Empty;
			var fileName = Path.GetFileNameWithoutExtension(dstFileRelativePath);

			if (!string.IsNullOrEmpty(dstDirectory))
				dstDirectory = AdjustOutputDirectories(dstDirectory);

			fileName += fileExtension;
			return Path.Combine(dirs.OutputDir, dstDirectory, fileName);
		}

		private static string AdjustOutputDirectories(string directories)
		{
			var stringBuilder = StringBuilderPool.Get();
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
