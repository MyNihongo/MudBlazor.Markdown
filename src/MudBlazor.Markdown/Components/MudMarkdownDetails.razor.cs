namespace MudBlazor;

/// <summary>
/// For some reason MudExpansionPanels eternally tried to dispose all panels, therefore, RenderFragment was called infinitely<br/>
/// Created this component in order to bypass that weird behaviour
/// </summary>
internal sealed class MudMarkdownDetails : ComponentBase
{
	[Parameter]
	public RenderFragment? TitleContent { get; set; }

	[Parameter]
	public RenderFragment? ChildContent { get; set; }
}
