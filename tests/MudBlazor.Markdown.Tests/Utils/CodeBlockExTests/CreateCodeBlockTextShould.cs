namespace MudBlazor.Markdown.Tests.Utils.CodeBlockExTests;

public sealed class CreateCodeBlockTextShould : CodeBlockExTestsBase
{
	[Fact]
	public void ReturnTextForSingleLine()
	{
		const string input = "singleline";
		
		var result = CreateFixture(input)
			.CreateCodeBlockText();
		
		result
			.Should()
			.Be(input);
	}
	
	[Fact]
	public void ReturnTextForMultiLines()
	{
		var input = $"first line{Environment.NewLine}next line";
		
		var result = CreateFixture(input)
			.CreateCodeBlockText();
		
		result
			.Should()
			.Be(input);
	}
	
	[Fact]
	public void ReturnTextWithEmptyLine()
	{
		var input = $"first line{Environment.NewLine}{Environment.NewLine}next line";
		
		var result = CreateFixture(input)
			.CreateCodeBlockText();
		
		result
			.Should()
			.Be(input);
	}
}
