using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

internal sealed class MudTableOfContentsNavLink : MudComponentBase
{
	[Parameter]
	public string? Id { get; set; }

	[Parameter]
	public string? Title { get; set; }

	[Parameter]
	public Typo Typo { get; set; }

	[Parameter]
	public bool IsActive { get; set; }

	[Parameter]
	public Func<string, Task>? OnClick { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		var elementIndex1 = 0;
		builder1.OpenElement(elementIndex1++, ElementNames.Nav);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, "mud-nav-group");

		var @class = new CssBuilder($"mud-nav-link-{Typo}")
			.AddClass("active", IsActive)
			.Build();

		builder1.OpenComponent<MudNavLink>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudNavLink.Class), @class);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudNavLink.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, OnClickAsync));
		builder1.AddComponentParameter(elementIndex1, nameof(MudNavLink.ChildContent), (RenderFragment)(builder2 => builder2.AddContent(0, Title)));
		builder1.CloseComponent();

		builder1.CloseElement();
	}

	private async Task OnClickAsync()
	{
		if (string.IsNullOrEmpty(Id) || OnClick is null)
			return;

		await OnClick(Id)
			.ConfigureAwait(false);
	}
}
