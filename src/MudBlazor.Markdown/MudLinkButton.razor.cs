using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using MudBlazor.Extensions;
using MudBlazor.Utilities;

// ReSharper disable once CheckNamespace
namespace MudBlazor
{
	internal sealed class MudLinkButton : MudComponentBase
	{
		private string Classname =>
			new CssBuilder("mud-typography mud-link")
				.AddClass($"mud-{Color.ToDescriptionString()}-text")
				.AddClass($"mud-link-underline-{Underline.ToDescriptionString()}")
				.AddClass($"mud-typography-{Typo.ToDescriptionString()}")
				.AddClass("mud-link-disabled", Disabled)
				.AddClass(Class)
				.Build();

		/// <summary>
		/// The color of the component. It supports the theme colors.
		/// </summary>
		[Parameter]
		public Color Color { get; set; } = Color.Primary;

		/// <summary>
		/// Typography variant to use.
		/// </summary>
		[Parameter]
		public Typo Typo { get; set; } = Typo.body1;

		/// <summary>
		/// Controls when the link should have an underline.
		/// </summary>
		[Parameter]
		public Underline Underline { get; set; } = Underline.Hover;

		/// <summary>
		/// If true, the navlink will be disabled.
		/// </summary>
		[Parameter]
		public bool Disabled { get; set; }

		/// <summary>
		/// Child content of component.
		/// </summary>
		[Parameter]
		public RenderFragment? ChildContent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			var i = 0;

			builder.OpenElement(i++, "span");
			builder.AddAttribute(i++, "class", Classname);
			builder.AddAttribute(i++, "style", Style);
			builder.AddContent(i++, ChildContent);
			builder.CloseElement();
		}
	}
}
