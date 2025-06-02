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

		// Toggle button
		builder1.OpenComponent<MudIconButton>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.Class), "mud-markdown-toc-btn-toggle");
		builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.Icon), Icons.Material.Filled.Menu);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.Variant), Variant.Filled);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.Color), Color.Primary);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.Size), Size.Large);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudIconButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, ToggleDrawer));
		builder1.CloseComponent();

		// Drawer
		var aa = new CssBuilder("mud-markdown-toc-drawer").AddClass("open", _isOpen).Build();
		builder1.OpenElement(elementIndex1++, ElementNames.Div);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, aa);
		builder1.AddContent(elementIndex1++, "aaaaaaaaaaaaa aaaaaaaaaaaaaaaaaaaa aaaaaaaaaaaaaaaaa aaaaaaaaaaaa");
		builder1.CloseElement();

		// Content
		aa = new CssBuilder("mud-markdown-toc-content").AddClass("open", _isOpen).Build();
		builder1.OpenElement(elementIndex1++, ElementNames.Div);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, aa);
		builder1.AddContent(elementIndex1++, ChildContent?.Invoke(markdownHeadingTree));
		builder1.CloseElement();

		builder1.CloseElement();
	}

	private void ToggleDrawer()
	{
		_isOpen = !_isOpen;
	}
}
