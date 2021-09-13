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

		public override string ToString() =>
			Program.StringBuilderPool.Get()
				.AppendFormat("{0}: {1}", nameof(ProjectDir), ProjectDir).AppendLine()
				.AppendFormat("{0}: {1}", nameof(CodeStyleDir), CodeStyleDir).AppendLine()
				.AppendFormat("{0}: {1}", nameof(OutputDir), OutputDir)
				.ToString();
	}
}
