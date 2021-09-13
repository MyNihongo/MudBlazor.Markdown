using System.IO;
using System.Threading.Tasks;

namespace MudBlazor.Markdown.Build.Utils
{
	public static class FileUtils
	{
		public static FileStream Open(string path, FileMode fileMode)
		{
			if (fileMode == FileMode.Create)
			{
				var dir = Path.GetDirectoryName(path);
				if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
					Directory.CreateDirectory(dir);
			}

			return new FileStream(path, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite, 4086, true);
		}

		public static async Task WriteAsync(string path, string content)
		{
			await using var stream = Open(path, FileMode.Create);
			await using var writer = new StreamWriter(stream);

			await writer.WriteAsync(content)
				.ConfigureAwait(false);
		}
	}
}
