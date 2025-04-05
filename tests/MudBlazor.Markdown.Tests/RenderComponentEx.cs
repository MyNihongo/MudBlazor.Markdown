namespace MudBlazor.Markdown.Tests;

public static class RenderComponentEx
{
	public static IRenderedComponent<T> AwaitElement<T>(this IRenderedComponent<T> @this, in string cssSelector)
		where T : ComponentBase
	{
		@this.WaitForElement(cssSelector, TimeSpan.FromSeconds(0.5d));
		return @this;
	}
}
