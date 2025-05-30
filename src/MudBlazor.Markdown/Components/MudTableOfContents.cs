using System.Diagnostics;

namespace MudBlazor;

internal sealed class MudTableOfContents : ComponentBase, IAsyncDisposable
{
	private bool _isOpen = true;
	private bool _hasScrollSpyStarted;
	private DotNetObjectReference<MudTableOfContents>? _dotNetObjectReference;

	[Parameter]
	public string? Header { get; set; }

	[Parameter]
	public string? MarkdownComponentId { get; set; }

	[Parameter]
	public RenderFragment<MudMarkdownHeadingTree>? ChildContent { get; set; }

	[Inject]
	private IJSRuntime JsRuntime { get; set; } = null!;

	[JSInvokable]
	public async Task OnActiveElementChangedAsync(string? newElementId)
	{
		Debug.WriteLine($"new: `{newElementId}`");
	}

	public async ValueTask DisposeAsync()
	{
		_dotNetObjectReference?.Dispose();

		if (!_hasScrollSpyStarted || string.IsNullOrEmpty(MarkdownComponentId))
			return;

		await JsRuntime.StopScrollSpyAsync(MarkdownComponentId)
			.ConfigureAwait(false);

		_hasScrollSpyStarted = false;
	}

	protected override void OnInitialized()
	{
		_dotNetObjectReference = DotNetObjectReference.Create(this);
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender || string.IsNullOrEmpty(MarkdownComponentId))
			return;

		_hasScrollSpyStarted = true;
		await JsRuntime.StartScrollSpyAsync(_dotNetObjectReference, MarkdownComponentId)
			.ConfigureAwait(false);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		var elementIndex1 = 0;
		builder1.OpenElement(elementIndex1++, ElementNames.Div);
		builder1.AddAttribute(elementIndex1++, AttributeNames.Class, "mud-markdown-toc");
		builder1.OpenComponent<MudDrawerContainer>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudDrawerContainer.Class), "mud-height-full");
		builder1.AddAttribute(elementIndex1, nameof(MudDrawerContainer.ChildContent), (RenderFragment)(builder2 =>
		{
			var markdownHeadingTree = new MudMarkdownHeadingTree();

			var elementIndex2 = 0;
			builder2.OpenComponent<MudDrawer>(elementIndex2++);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudDrawer.Fixed), false);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudDrawer.Anchor), Anchor.Right);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudDrawer.Elevation), 0);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudDrawer.Variant), DrawerVariant.Persistent);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudDrawer.Open), _isOpen);
			builder2.AddComponentParameter(elementIndex2++, nameof(MudDrawer.OpenChanged), EventCallback.Factory.Create(this, RuntimeHelpers.CreateInferredEventCallback(this, isOpen => { _isOpen = isOpen; }, _isOpen)));
			builder2.AddAttribute(elementIndex2++, nameof(MudDrawer.ChildContent), (RenderFragment)(builder3 =>
			{
				var elementIndex3 = 0;

				if (!string.IsNullOrEmpty(Header))
				{
					builder3.OpenComponent<MudDrawerHeader>(elementIndex3++);
					builder3.AddAttribute(elementIndex3++, nameof(MudDrawerHeader.ChildContent), (RenderFragment)(builder4 =>
					{
						var elementIndex4 = 0;
						builder4.OpenComponent<MudText>(elementIndex4++);
						builder4.AddComponentParameter(elementIndex4++, nameof(MudText.Typo), Typo.h6);
						builder4.AddAttribute(elementIndex4, nameof(MudText.ChildContent), (RenderFragment)(builder5 => builder5.AddContent(0, Header)));
						builder4.CloseComponent();
					}));
					builder3.CloseComponent();
				}

				builder3.OpenComponent<MudTableOfContentsNavMenu>(elementIndex3++);
				builder3.AddComponentParameter(elementIndex3, nameof(MudTableOfContentsNavMenu.MarkdownHeadingTree), markdownHeadingTree);
				builder3.CloseComponent();
			}));
			builder2.CloseComponent();

			builder2.OpenElement(elementIndex2++, ElementNames.Div);
			builder2.AddAttribute(elementIndex2++, AttributeNames.Class, "d-flex mud-height-full");
			builder2.AddContent(elementIndex2, ChildContent?.Invoke(markdownHeadingTree));
			builder2.CloseElement();
		}));
		builder1.CloseComponent();
		builder1.CloseElement();
	}
}
