namespace MudBlazor.Markdown.Tests.Extensions.CodeBlockExTests;

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
		const string input = "first line\r\nnext line";
		
		var result = CreateFixture(input)
			.CreateCodeBlockText();
		
		result
			.Should()
			.Be(input);
	}
	
	[Fact]
	public void ReturnTextWithEmptyLine()
	{
		const string input = "first line\r\n\r\nnext line";
		
		var result = CreateFixture(input)
			.CreateCodeBlockText();
		
		result
			.Should()
			.Be(input);
	}
}
