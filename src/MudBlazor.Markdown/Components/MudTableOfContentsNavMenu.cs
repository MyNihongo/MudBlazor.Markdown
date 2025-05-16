namespace MudBlazor;

internal sealed class MudTableOfContentsNavMenu : ComponentBase
{
	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		var elementIndex1 = 0;
		builder1.OpenComponent<MudNavMenu>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1, nameof(MudNavMenu.ChildContent), (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;
			builder2.OpenComponent<MudTableOfContentsNavLink>(elementIndex2++);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudTableOfContentsNavLink.Title), "Dashboard");
			builder2.CloseComponent();
			
			builder2.OpenComponent<MudTableOfContentsNavLink>(elementIndex2++);
			builder2.AddComponentParameter(elementIndex2, nameof(MudTableOfContentsNavLink.Title), "Shop");
			builder2.CloseComponent();
		}));
		builder1.CloseComponent();
	}
}
