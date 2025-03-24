﻿using Microsoft.AspNetCore.Components.Web;

namespace MudBlazor;

public class MudCodeHighlight : MudComponentBase, IDisposable
{
	private ElementReference _ref;
	private IMudMarkdownThemeService? _themeService;
	private bool _isFirstThemeSet, _isTextUpdated;

	/// <summary>
	/// Code text to render
	/// </summary>
	[Parameter]
	public string Text { get; set; } = string.Empty;

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
	[Parameter]
	public CodeBlockTheme Theme { get; set; }

	[Inject]
	private IJSRuntime Js { get; init; } = default!;

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

	public override async Task SetParametersAsync(ParameterView parameters)
	{
		if (parameters.TryGetValue<string>(nameof(Text), out var text) && !ReferenceEquals(text, Text))
			_isTextUpdated = true;

		if (parameters.TryGetValue<CodeBlockTheme>(nameof(Theme), out var theme) && theme != Theme)
		{
			await SetThemeAsync()
				.ConfigureAwait(false);
		}

		await base.SetParametersAsync(parameters)
			.ConfigureAwait(false);
	}

	protected override bool ShouldRender() =>
		!string.IsNullOrEmpty(Text);

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		var i = 0;

		builder.OpenElement(i++, "div");
		builder.AddAttribute(i++, "class", "snippet-clipboard-content overflow-auto");

		// Copy button
		builder.OpenComponent<MudIconButton>(i++);
		builder.AddAttribute(i++, nameof(MudIconButton.Icon), Icons.Material.Rounded.ContentCopy);
		builder.AddAttribute(i++, nameof(MudIconButton.Variant), Variant.Filled);
		builder.AddAttribute(i++, nameof(MudIconButton.Color), Color.Primary);
		builder.AddAttribute(i++, nameof(MudIconButton.Size), Size.Medium);
		builder.AddAttribute(i++, nameof(MudIconButton.Class), "snippet-clipboard-copy-icon ma-2");
		builder.AddAttribute(i++, nameof(MudIconButton.OnClick), EventCallback.Factory.Create<MouseEventArgs>(this, CopyTextToClipboardAsync));
		builder.CloseComponent();

		// Code block
		builder.OpenElement(i++, "pre");
		builder.OpenElement(i++, "code");
		builder.AddAttribute(i++, "class", CodeClasses);
		builder.AddElementReferenceCapture(i++, x => _ref = x);

		builder.CloseElement();
		builder.CloseElement();
		builder.CloseElement();
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

	private async Task CopyTextToClipboardAsync(MouseEventArgs args)
	{
		var ok = await Js.InvokeAsync<bool>("copyTextToClipboard", Text)
			.ConfigureAwait(false);

		if (ok)
			return;

		var clipboardService = ServiceProvider?.GetService<IMudMarkdownClipboardService>();

		if (clipboardService != null)
		{
			await clipboardService.CopyToClipboardAsync(Text)
				.ConfigureAwait(false);
		}
	}
}
