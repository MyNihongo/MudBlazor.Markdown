using MudBlazor.Markdown.Tests.Services;

namespace MudBlazor.Markdown.Tests;

public abstract class ComponentTestsBase : IDisposable
{
	protected readonly TestContext Ctx = new();

	protected ComponentTestsBase()
	{
		Ctx.Services
			.AddMudMarkdownServices()
			.AddSingleton(MockJsRuntime.Object)
			.AddSingleton<NavigationManager>(MockNavigationManager);
	}

	protected Mock<IJSRuntime> MockJsRuntime { get; } = new();

	protected TestNavigationManager MockNavigationManager { get; } = new();

	public void Dispose()
	{
		Ctx.Dispose();
		GC.SuppressFinalize(this);
	}
}
