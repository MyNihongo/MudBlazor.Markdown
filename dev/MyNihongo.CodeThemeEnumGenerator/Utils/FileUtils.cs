using System.IO;

namespace MyNihongo.CodeThemeEnumGenerator.Utils
{
	public static class FileUtils
	{
		public static FileStream Open(string path, FileMode fileMode) =>
			new(path, fileMode, FileAccess.ReadWrite, FileShare.ReadWrite, 4086, true);
	}
}
