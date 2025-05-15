namespace MudBlazor;

/// <summary>
/// For some reason MudExpansionPanels eternally tried to dispose all panels, therefore, RenderFragment was called infinitely.<br/>
/// Created this component in order to bypass that weird behaviour.
/// </summary>
internal sealed class MudMarkdownDetails : ComponentBase
{
	[Parameter]
	public RenderFragment? TitleContent { get; set; }

	[Parameter]
	public RenderFragment? ChildContent { get; set; }

	private bool IsExpanded { get; set; }

	private string IconClasses => new CssBuilder("mud-expand-panel-icon")
		.AddClass("mud-transform", IsExpanded)
		.Build();

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var elementIndex = 0;
		builder.OpenElement(elementIndex++, "div");
		builder.AddAttribute(elementIndex++, "class", "mud-expand-panel mud-elevation-1 mud-expand-panel-border");

		BuildTitle(builder, ref elementIndex);
		BuildContent(builder, ref elementIndex);

		builder.CloseElement();
	}

	private void BuildTitle(in RenderTreeBuilder builder, ref int elementIndex)
	{
		builder.OpenElement(elementIndex++, ElementNames.Div);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, "mud-expand-panel-header mud-ripple");
		builder.AddAttribute(elementIndex++, AttributeNames.OnClick, EventCallback.Factory.Create(this, OnHeaderClick));

		// Text
		builder.OpenElement(elementIndex++, ElementNames.Div);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, "mud-expand-panel-text");
		builder.AddContent(elementIndex++, TitleContent);
		builder.CloseElement();

		// Collapse icon
		builder.OpenComponent<MudIcon>(elementIndex++);
		builder.AddComponentParameter(elementIndex++, nameof(MudIcon.Icon), Icons.Material.Filled.ExpandMore);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, IconClasses);
		builder.CloseComponent();

		builder.CloseElement(); // "div"
	}

	private void BuildContent(RenderTreeBuilder builder1, ref int elementIndex1)
	{
		builder1.OpenComponent<MudCollapse>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudCollapse.Expanded), IsExpanded);

		builder1.AddAttribute(elementIndex1++, nameof(MudCollapse.ChildContent), (RenderFragment)(builder2 =>
		{
			var elementIndex2 = 0;
			builder2.OpenElement(elementIndex2++, ElementNames.Div);
			builder2.AddAttribute(elementIndex2++, AttributeNames.Class, "mud-expand-panel-content");
			builder2.AddContent(elementIndex2, ChildContent);
			builder2.CloseElement();
		}));

		builder1.CloseComponent();
	}

	private void OnHeaderClick()
	{
		IsExpanded = !IsExpanded;
	}
}
