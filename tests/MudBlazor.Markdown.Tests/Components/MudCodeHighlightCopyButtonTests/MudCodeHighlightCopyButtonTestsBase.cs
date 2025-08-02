namespace MudBlazor.Markdown.Tests.Components.MudCodeHighlightCopyButtonTests;

public abstract class MudCodeHighlightCopyButtonTestsBase : ComponentTestsBase
{
	internal IRenderedComponent<MudCodeHighlightCopyButton> CreateFixture()
	{
		return Ctx.RenderComponent<MudCodeHighlightCopyButton>();
	}
}
