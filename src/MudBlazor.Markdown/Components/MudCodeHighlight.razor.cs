namespace MudBlazor;

public class MudCodeHighlight : MudComponentBase, IDisposable
{
	private ElementReference _ref;
	private CodeBlockTheme _theme;
	private IMudMarkdownThemeService? _themeService;
	private bool _isFirstThemeSet;

	private string _text = string.Empty;
	private bool _isTextUpdated;

	/// <summary>
	/// Code text to render
	/// </summary>
	[Parameter]
#if NET8_0 || NET9_0 || NET10_0
#pragma warning disable BL0007
#endif
	public string Text
	{
		get => _text;
		set
		{
			if (_text != value)
				_isTextUpdated = true;

			_text = value;
		}
	}
#if NET8_0 || NET9_0 || NET10_0
#pragma warning restore BL0007
#endif

	/// <summary>
	/// Language of the <see cref="Text"/>
	/// </summary>
	[Parameter]
	public string Language { get; set; } = string.Empty;

	/// <summary>
	/// Theme of the code block.<br/>
	/// Browse available themes here: https://highlightjs.org/static/demo/ <br/>
	/// Default is <see cref="CodeBlockTheme.Default"/>
	/// </summary>
#if NET8_0 || NET9_0 || NET10_0
#pragma warning disable BL0007
#endif
	[Parameter]
	public CodeBlockTheme Theme
	{
		get => _theme;
		set
		{
			if (_theme == value)
				return;

			_theme = value;
			Task.Run(SetThemeAsync);
		}
	}
#if NET8_0 || NET9_0 || NET10_0
#pragma warning restore BL0007
#endif

	[Parameter]
	public CodeBlockCopyButton CopyButton { get; set; } = CodeBlockCopyButton.OnHover;

	[Parameter]
	public string? CopyButtonDisplayTextCopied { get; set; }

	[Inject]
	private IJSRuntime Js { get; init; } = null!;

	[Inject]
	private IServiceProvider? ServiceProvider { get; init; }

	private string CodeClasses => new CssBuilder()
		.AddClass("hljs")
		.AddClass(() => $"language-{Language}", () => !string.IsNullOrEmpty(Language))
		.Build();

	public void Dispose()
	{
		if (_themeService != null)
			_themeService.CodeBlockThemeChanged -= OnCodeBlockThemeChanged;

		GC.SuppressFinalize(this);
	}

	protected override bool ShouldRender() =>
		!string.IsNullOrEmpty(Text);

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var containerClass = "hljs mud-markdown-code-highlight";
		if (CopyButton == CodeBlockCopyButton.Sticky)
			containerClass += "-sticky";

		var elementIndex = 0;
		builder.OpenElement(elementIndex++, ElementNames.Div);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, containerClass);

		// Copy button
		if (CopyButton != CodeBlockCopyButton.None)
		{
			var copyButtonClass = "ma-2 mud-markdown-code-highlight-copybtn";

			if (CopyButton == CodeBlockCopyButton.Sticky)
				copyButtonClass += "-sticky";

			builder.OpenComponent<MudCodeHighlightCopyButton>(elementIndex++);
			builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlightCopyButton.Class), copyButtonClass);
			builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlightCopyButton.TextToCopy), Text);
			builder.AddComponentParameter(elementIndex++, nameof(MudCodeHighlightCopyButton.DisplayTextCopied), CopyButtonDisplayTextCopied);
			builder.CloseComponent();
		}

		// Code block
		builder.OpenElement(elementIndex++, "pre");
		builder.OpenElement(elementIndex++, "code");
		builder.AddAttribute(elementIndex++, "class", CodeClasses);
		builder.AddElementReferenceCapture(elementIndex, x => _ref = x);
		builder.CloseElement(); // "pre"
		builder.CloseElement(); // "code"

		builder.CloseElement(); // "div"
	}

	protected override void OnInitialized()
	{
		base.OnInitialized();

		_themeService = ServiceProvider?.GetService<IMudMarkdownThemeService>();

		if (_themeService != null)
			_themeService.CodeBlockThemeChanged += OnCodeBlockThemeChanged;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (_isTextUpdated)
		{
			await Js.InvokeVoidAsync("highlightCodeElement", _ref, Text, Language)
				.ConfigureAwait(false);

			_isTextUpdated = false;
		}

		if (!firstRender)
			return;

		if (!_isFirstThemeSet)
		{
			await SetThemeAsync()
				.ConfigureAwait(false);
		}
	}

	private void OnCodeBlockThemeChanged(object? sender, CodeBlockTheme e) =>
		Theme = e;

	private async Task SetThemeAsync()
	{
		var stylesheetPath = Theme.GetStylesheetPath();

		await Js.InvokeVoidAsync("setHighlightStylesheet", stylesheetPath)
			.ConfigureAwait(false);

		_isFirstThemeSet = true;
	}
}
