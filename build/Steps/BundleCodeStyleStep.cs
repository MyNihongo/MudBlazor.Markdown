using System.IO;
using System.Threading.Tasks;
using MudBlazor.Markdown.Build.Models;
using MudBlazor.Markdown.Build.Steps.Interfaces;
using MudBlazor.Markdown.Build.Utils;
using NUglify;
using NUglify.Css;

namespace MudBlazor.Markdown.Build.Steps
{
	public sealed class BundleCodeStyleStep : IStep
	{
		private static readonly CssSettings CssSettings = new()
		{
			CommentMode = CssComment.None
		};

		/// <remarks>
		///	Due to not being good at webpack I could not bundle these .css files with webpack :(
		/// Any help?
		/// </remarks>
		public ValueTask ProcessFileAsync(string filePath, ProjectDirs dirs)
		{
			var fileExtension = Path.GetExtension(filePath);

			return fileExtension switch
			{
				".css" => BundleCssAsync(filePath, dirs),
				".png" => CopyFile(filePath, dirs, fileExtension),
				_ => ValueTask.CompletedTask
			};
		}

		public ValueTask CompleteAsync(ProjectDirs dirs) =>
			ValueTask.CompletedTask;

		private static async ValueTask BundleCssAsync(string filePath, ProjectDirs dirs)
		{
			string styleContent;

			await using (var stream = FileUtils.Open(filePath, FileMode.Open))
			using (var reader = new StreamReader(stream))
			{
				styleContent = await reader.ReadToEndAsync()
					.ConfigureAwait(false);
			}

			styleContent = Uglify.Css(styleContent, CssSettings).Code;
			var dstPath = CreateDestinationPath(filePath, dirs, Program.CssOutputExtension);

			await using (var stream = FileUtils.Open(dstPath, FileMode.Create))
			await using (var writer = new StreamWriter(stream))
			{
				await writer.WriteAsync(styleContent)
					.ConfigureAwait(false);
			}
		}

		private static ValueTask CopyFile(string filePath, ProjectDirs dirs, string fileExtension)
		{
			var dstPath = CreateDestinationPath(filePath, dirs, fileExtension);
			File.Copy(filePath, dstPath, true);

			return ValueTask.CompletedTask;
		}

		private static string CreateDestinationPath(string filePath, ProjectDirs dirs, string fileExtension)
		{
			var relativePath = CodeStyleUtils.CreateDestinationRelativePath(filePath, dirs, fileExtension, true);
			return Path.Combine(dirs.OutputDir, relativePath);
		}
	}
}
