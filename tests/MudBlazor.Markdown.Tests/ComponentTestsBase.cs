using MudBlazor.Markdown.Tests.Services;

namespace MudBlazor.Markdown.Tests;

public abstract class ComponentTestsBase : IDisposable
{
	private readonly Mock<IPopoverService> _popoverServiceMock = new();
	protected readonly Mock<IJSRuntime> MockJsRuntime = new();
	protected readonly TestNavigationManager MockNavigationManager = new();
	protected readonly BunitContext Ctx = new();

	protected ComponentTestsBase()
	{
		_popoverServiceMock
			.SetupGet(x => x.PopoverOptions)
			.Returns(new PopoverOptions());

		Ctx.Services
			.AddMudMarkdownServices()
			.AddSingleton(MockJsRuntime.Object)
			.AddSingleton(_popoverServiceMock.Object)
			.AddSingleton<NavigationManager>(MockNavigationManager);
	}

	public void Dispose()
	{
		Ctx.Dispose();
		GC.SuppressFinalize(this);
	}
}
