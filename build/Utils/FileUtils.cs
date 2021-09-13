using System.IO;

namespace MyNihongo.CodeThemeEnumGenerator.Utils
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
	}
}
