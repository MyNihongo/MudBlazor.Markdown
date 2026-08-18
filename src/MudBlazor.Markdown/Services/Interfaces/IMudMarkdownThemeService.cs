namespace MudBlazor;

[Obsolete("TODO add message")]
public interface IMudMarkdownThemeService
{
	event EventHandler<CodeBlockTheme> CodeBlockThemeChanged;

	void SetCodeBlockTheme(CodeBlockTheme theme);
}
