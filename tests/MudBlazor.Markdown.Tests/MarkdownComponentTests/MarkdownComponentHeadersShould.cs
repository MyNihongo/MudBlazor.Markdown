using MyNihongo.Option;

namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentHeadersShould : MarkdownComponentTestsBase
{
	[Theory]
	[InlineData("#", "h1")]
	[InlineData("##", "h2")]
	[InlineData("###", "h3")]
	[InlineData("####", "h4")]
	[InlineData("#####", "h5")]
	[InlineData("######", "h6")]
	public void RenderHeaders(string valueInput, string expectedTag)
	{
		var value = valueInput + " some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH1(Typo newTypo, string expectedTag)
	{
		const string value = "# some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		var @override = Optional<Func<Typo, Typo>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
		fixture.MarkupMatches(expected);

		Typo Override(Typo x) =>
			x == Typo.h1 ? newTypo : x;
	}

	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH2(Typo newTypo, string expectedTag)
	{
		const string value = "## some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		var @override = Optional<Func<Typo, Typo>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
		fixture.MarkupMatches(expected);

		Typo Override(Typo x) =>
			x == Typo.h2 ? newTypo : x;
	}

	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH3(Typo newTypo, string expectedTag)
	{
		const string value = "### some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		var @override = Optional<Func<Typo, Typo>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
		fixture.MarkupMatches(expected);

		Typo Override(Typo x) =>
			x == Typo.h3 ? newTypo : x;
	}

	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH4(Typo newTypo, string expectedTag)
	{
		const string value = "#### some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		var @override = Optional<Func<Typo, Typo>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
		fixture.MarkupMatches(expected);

		Typo Override(Typo x) =>
			x == Typo.h4 ? newTypo : x;
	}

	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH5(Typo newTypo, string expectedTag)
	{
		const string value = "##### some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		var @override = Optional<Func<Typo, Typo>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
		fixture.MarkupMatches(expected);

		Typo Override(Typo x) =>
			x == Typo.h5 ? newTypo : x;
	}

	[Theory]
	[InlineData(Typo.h1, "h1")]
	[InlineData(Typo.h2, "h2")]
	[InlineData(Typo.h3, "h3")]
	[InlineData(Typo.h4, "h4")]
	[InlineData(Typo.h5, "h5")]
	[InlineData(Typo.h6, "h6")]
	public void RenderHeadersWithDifferentTypoH6(Typo newTypo, string expectedTag)
	{
		const string value = "###### some text";

		var expected =
$@"<article class='mud-markdown-body'>
	<{expectedTag} id='some-text' class='mud-typography mud-typography-{expectedTag}'>
		some text
	</{expectedTag}>
</article>";

		var @override = Optional<Func<Typo, Typo>?>.Of(Override);

		using var fixture = CreateFixture(value, overrideHeaderTypo: @override);
		fixture.MarkupMatches(expected);

		Typo Override(Typo x) =>
			x == Typo.h6 ? newTypo : x;
	}
}