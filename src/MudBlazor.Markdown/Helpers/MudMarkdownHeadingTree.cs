namespace MudBlazor;

internal sealed class MudMarkdownHeadingTree
{
	public void Append(in Typo typo, in HeadingContent? content)
	{
		if (typo < Typo.h1 || typo > Typo.h3 || content is null)
			return;
	}
}
