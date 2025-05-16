using System.Diagnostics;
using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

internal sealed class MudTableOfContentsNavLink : MudComponentBase
{
	[Parameter]
	public string? Title { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		var elementIndex1 = 0;
		builder1.OpenElement(elementIndex1++, ElementNames.Nav);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, "mud-nav-group");

		builder1.OpenComponent<MudNavLink>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudNavLink.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, args => Debug.WriteLine("click")));
		builder1.AddComponentParameter(elementIndex1, nameof(MudNavLink.ChildContent), (RenderFragment)(builder2 => builder2.AddContent(0, Title)));
		builder1.CloseComponent();

		builder1.CloseElement();
	}
}
