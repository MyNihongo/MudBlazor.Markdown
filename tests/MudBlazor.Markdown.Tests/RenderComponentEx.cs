namespace MudBlazor.Markdown.Tests;

public static class RenderComponentEx
{
	public static IRenderedComponent<T> AwaitElement<T>(this IRenderedComponent<T> @this, in string cssSelector, in TimeSpan? timeout = null)
		where T : ComponentBase
	{
		@this.WaitForElement(cssSelector, timeout ?? TimeSpan.FromSeconds(0.5d));
		return @this;
	}
}
