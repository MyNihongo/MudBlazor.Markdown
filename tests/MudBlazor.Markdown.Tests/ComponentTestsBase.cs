using MudBlazor.Markdown.Tests.Services;

namespace MudBlazor.Markdown.Tests;

public abstract class ComponentTestsBase : IDisposable
{
	protected readonly TestContext Ctx = new();

	protected ComponentTestsBase()
	{
		Ctx.Services
			.AddSingleton(MockJsRuntime.Object)
			.AddSingleton<NavigationManager>(MockNavigationManager)
			.AddSingleton<IMudMarkdownValueProvider, MudMarkdownValueProvider>();
	}

	protected Mock<IJSRuntime> MockJsRuntime { get; } = new();

	protected TestNavigationManager MockNavigationManager { get; } = new();

	public void Dispose()
	{
		Ctx.Dispose();
		GC.SuppressFinalize(this);
	}
}
