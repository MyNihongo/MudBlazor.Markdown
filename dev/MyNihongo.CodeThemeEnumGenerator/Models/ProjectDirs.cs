namespace MyNihongo.CodeThemeEnumGenerator.Models
{
	internal readonly struct ProjectDirs
	{
		public ProjectDirs(string codeStyleDir, string enumFilePath, string outputDir)
		{
			CodeStyleDir = codeStyleDir;
			EnumFilePath = enumFilePath;
			OutputDir = outputDir;
		}

		public string CodeStyleDir { get; }

		public string EnumFilePath { get; }

		public string OutputDir { get; }
	}
}
