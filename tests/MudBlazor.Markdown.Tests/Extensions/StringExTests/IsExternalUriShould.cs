namespace MudBlazor.Markdown.Tests.Extensions.StringExTests;

public sealed class IsExternalUriShould
{
	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void ReturnFalseIfUrlIsNullOrEmpty(string? input)
	{
		var result = input.IsExternalUri("base uri");

		result
			.Should()
			.BeFalse();
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	public void ReturnFalseIfBaseUrlIsNullOrEmpty(string? input)
	{
		var result = "uri".IsExternalUri(input);

		result
			.Should()
			.BeFalse();
	}

	[Fact]
	public void BeFalseIfUriIncorrectlyFormatted()
	{
		const string input = "http://localhost:1$3",
			baseUrl = "http://localhost:1234";

		var result = input.IsExternalUri(baseUrl);

		result
			.Should()
			.BeFalse();
	}

	[Theory]
	[InlineData("")]
	[InlineData("/")]
	public void ReturnFalseIfRelativeUri(string prefix)
	{
		const string baseUrl = "http://localhost:1234";
		var input = $"{prefix}tokyo/edogawa";

		var result = input.IsExternalUri(baseUrl);

		result
			.Should()
			.BeFalse();
	}

	[Fact]
	public void ReturnFalseIfAbsoluteUriFromBase()
	{
		const string baseUrl = "http://localhost:1234",
			input = baseUrl + "/tokyo";

		var result = input.IsExternalUri(baseUrl);

		result
			.Should()
			.BeFalse();
	}

	[Fact]
	public void ReturnTrueIfAbsoluteUriAnotherBase()
	{
		const string baseUrl = "http://localhost:1234",
			input = "https://google.co.jp";

		var result = input.IsExternalUri(baseUrl);

		result
			.Should()
			.BeTrue();
	}
}
