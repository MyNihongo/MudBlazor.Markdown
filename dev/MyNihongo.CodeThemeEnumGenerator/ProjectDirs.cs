namespace MyNihongo.CodeThemeEnumGenerator
{
	internal readonly struct ProjectDirs
	{
		public ProjectDirs(string codeStyleDir, string enumFilePath)
		{
			CodeStyleDir = codeStyleDir;
			EnumFilePath = enumFilePath;
		}

		public string CodeStyleDir { get; }

		public string EnumFilePath { get; }
	}
}
