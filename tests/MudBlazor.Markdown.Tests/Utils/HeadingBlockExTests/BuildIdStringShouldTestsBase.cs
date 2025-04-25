namespace MudBlazor.Markdown.Tests.Utils.HeadingBlockExTests;

public abstract class BuildIdStringShouldTestsBase
{
	protected static HeadingBlock CreateFixture(string value)
	{
		var block = Markdig.Markdown.Parse(value)
			.Single(x => x is HeadingBlock);

		return (HeadingBlock)block;
	}
}