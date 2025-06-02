using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

internal sealed class MudTableOfContents : ComponentBase
{
	private bool _isOpen = true;

	[Parameter]
	public string? Header { get; set; }

	[Parameter]
	public string? MarkdownComponentId { get; set; }

	[Parameter]
	public RenderFragment<MudMarkdownHeadingTree>? ChildContent { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		var markdownHeadingTree = new MudMarkdownHeadingTree();

		var elementIndex1 = 0;
		builder1.OpenElement(elementIndex1++, ElementNames.Div);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, "mud-markdown-toc");

		// Drawer
		builder1.OpenElement(elementIndex1++, ElementNames.Div);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, "mud-markdown-toc-drawer");
		builder1.AddContent(elementIndex1++, (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;

			// Toggle button
			builder2.OpenComponent<MudIconButton>(elementIndex2++);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudIconButton.Class), "mud-markdown-toc-btn-toggle");
			builder2.AddComponentParameter(elementIndex2++, nameof(MudIconButton.Icon), Icons.Material.Filled.Menu);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudIconButton.Variant), Variant.Filled);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudIconButton.Color), Color.Primary);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudIconButton.Size), Size.Large);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudIconButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, ToggleDrawer));
			builder2.CloseComponent();

			// Drawer
			var @class = new CssBuilder("mud-markdown-toc-drawer-content").AddClass("open", _isOpen).Build();
			builder2.OpenElement(elementIndex2++, ElementNames.Div);
			builder2.AddAttribute(elementIndex2++, AttributeNames.Class, @class);
			builder2.AddContent(elementIndex2++, "aaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaa");
			builder2.CloseElement();
		}));
		builder1.CloseElement();

		// Content
		var @class = new CssBuilder("mud-markdown-toc-content").AddClass("open", _isOpen).Build();
		builder1.OpenElement(elementIndex1++, ElementNames.Div);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, @class);
		builder1.AddContent(elementIndex1++, ChildContent?.Invoke(markdownHeadingTree));
		builder1.CloseElement();

		builder1.CloseElement();
	}

	private void ToggleDrawer()
	{
		_isOpen = !_isOpen;
	}
}
