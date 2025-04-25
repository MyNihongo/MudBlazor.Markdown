namespace MudBlazor.Markdown.Tests.Utils.ServiceCollectionExTests;

public abstract class ServiceCollectionExTestsBase
{
	protected static IServiceCollection CreateFixture()
	{
		return new ServiceCollection();
	}
}
