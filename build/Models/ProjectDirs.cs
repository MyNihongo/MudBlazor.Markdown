namespace MudBlazor.Markdown.Build.Models
{
	public readonly struct ProjectDirs
	{
		public ProjectDirs(string projectDir, string codeStyleDir, string outputDir)
		{
			ProjectDir = projectDir;
			CodeStyleDir = codeStyleDir;
			OutputDir = outputDir;
		}

		public string ProjectDir { get; }

		public string CodeStyleDir { get; }

		public string OutputDir { get; }
	}
}
