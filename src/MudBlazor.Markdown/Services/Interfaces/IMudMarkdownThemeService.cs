namespace MudBlazor;

[Obsolete("`CodeBlockTheme` is obsolete and has no effect. Use `MudMarkdownThemeProvider` instead. For more details see https://github.com/MyNihongo/MudBlazor.Markdown/wiki/MudMarkdownThemeProvider")]
public interface IMudMarkdownThemeService
{
	event EventHandler<CodeBlockTheme> CodeBlockThemeChanged;

	void SetCodeBlockTheme(CodeBlockTheme theme);
}

[Obsolete("`CodeBlockTheme` is obsolete and has no effect. Use `MudMarkdownThemeProvider` instead. For more details see https://github.com/MyNihongo/MudBlazor.Markdown/wiki/MudMarkdownThemeProvider")]
internal sealed class MudMarkdownThemeService : IMudMarkdownThemeService
{
	public event EventHandler<CodeBlockTheme>? CodeBlockThemeChanged;

	public void SetCodeBlockTheme(CodeBlockTheme theme)
	{
	}
}
