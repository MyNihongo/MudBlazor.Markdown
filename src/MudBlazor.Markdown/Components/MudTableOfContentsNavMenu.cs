using System.Runtime.InteropServices;

namespace MudBlazor;

internal sealed class MudTableOfContentsNavMenu : ComponentBase
{
	private List<MudMarkdownHeadingTree.Item>? _headingItems;

	[Parameter]
	public MudMarkdownHeadingTree? MarkdownHeadingTree { get; set; }

	public void InvokeRenderNavMenu(in List<MudMarkdownHeadingTree.Item> headingItems)
	{
		_headingItems = headingItems;
		StateHasChanged();
	}

	protected override void OnParametersSet()
	{
		MarkdownHeadingTree?.SetNavMenuReference(this);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		if (_headingItems is null || _headingItems.Count == 0)
			return;

		var elementIndex1 = 0;
		builder1.OpenComponent<MudNavMenu>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1, nameof(MudNavMenu.ChildContent), (RenderFragment)(builder2 =>
		{
			var span = CollectionsMarshal.AsSpan(_headingItems);

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
