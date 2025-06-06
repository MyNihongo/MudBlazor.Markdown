using System.Runtime.InteropServices;

namespace MudBlazor;

internal sealed class MudTableOfContentsNavMenu : ComponentBase, IAsyncDisposable
{
	private bool _hasScrollSpyStarted;
	private string? _activeElementId;

	private List<MudMarkdownHeadingTree.Item>? _headingItems;
	private DotNetObjectReference<MudTableOfContentsNavMenu>? _dotNetObjectReference;

	[Parameter]
	public MudMarkdownHeadingTree? MarkdownHeadingTree { get; set; }

	[Parameter]
	public string? MarkdownComponentId { get; set; }

	[Inject]
	private IJSRuntime JsRuntime { get; set; } = null!;

	public DotNetObjectReference<MudTableOfContentsNavMenu>? ObjectReference => _dotNetObjectReference;

	[JSInvokable]
	public async Task OnActiveElementChangedAsync(string? newElementId)
	{
		_activeElementId = newElementId;
		await InvokeAsync(StateHasChanged);
	}

	public void InvokeRenderNavMenu(in List<MudMarkdownHeadingTree.Item> headingItems)
	{
		_headingItems = headingItems;
		StateHasChanged();
	}

	public async ValueTask DisposeAsync()
	{
		_dotNetObjectReference?.Dispose();
		_dotNetObjectReference = null;

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

	protected override void OnParametersSet()
	{
		MarkdownHeadingTree?.SetNavMenuReference(this);
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender || string.IsNullOrEmpty(MarkdownComponentId))
			return;

		_hasScrollSpyStarted = true;
		await JsRuntime.StartScrollSpyAsync(MarkdownComponentId, _dotNetObjectReference)
			.ConfigureAwait(false);
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder1)
	{
		if (_headingItems is null || _headingItems.Count == 0)
			return;

		var elementIndex1 = 0;
		builder1.OpenComponent<MudNavMenu>(elementIndex1++);
		builder1.AddComponentParameter(elementIndex1++, nameof(MudNavMenu.Class), "mud-markdown-toc-nav-menu");
		builder1.AddComponentParameter(elementIndex1, nameof(MudNavMenu.ChildContent), (RenderFragment)(builder2 =>
		{
			var spanOfItems = CollectionsMarshal.AsSpan(_headingItems);

			var elementIndex2 = 0;
			foreach (var item in spanOfItems)
			{
				builder2.OpenComponent<MudTableOfContentsNavLink>(elementIndex2++);
				builder2.AddComponentParameter(elementIndex2++, nameof(MudTableOfContentsNavLink.Id), item.Id);
				builder2.AddComponentParameter(elementIndex2++, nameof(MudTableOfContentsNavLink.Title), item.Text);
				builder2.AddComponentParameter(elementIndex2++, nameof(MudTableOfContentsNavLink.Typo), item.Typo);
				builder2.AddComponentParameter(elementIndex2++, nameof(MudTableOfContentsNavLink.IsActive), item.Id == _activeElementId);
				builder2.AddComponentParameter(elementIndex2++, nameof(MudTableOfContentsNavLink.OnClick), (Func<string, Task>)OnNavLinkClickedAsync);
				builder2.CloseComponent();
			}
		}));
		builder1.CloseComponent();
	}

	private async Task OnNavLinkClickedAsync(string elementId)
	{
		await JsRuntime.ScrollToAsync(elementId, _dotNetObjectReference)
			.ConfigureAwait(false);
	}
}
