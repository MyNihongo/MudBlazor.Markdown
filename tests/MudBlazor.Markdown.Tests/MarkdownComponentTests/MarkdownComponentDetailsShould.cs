namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentDetailsShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderDetailsMultiline()
	{
		const string value =
@"<details>
	<summary>Header</summary>
	Some hidden text
	Another text
</details>";

		const string expected =
@"<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true'>
				<path d='M0 0h24v24H0z' fill='none'/><path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div class='mud-collapse-container' style='' blazor:elementReference='608e6c71-e73b-49e8-bbf5-30bbb613ad11'>
			<div class='mud-collapse-wrapper' blazor:elementReference='e1b1f0e7-04f7-4696-8279-3918626ffde7'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden text<br />Another text</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsSummaryMultiLine()
	{
		const string value =
@"<details>
	<summary>
		Header
	</summary>
	Some hidden text
	Another text
</details>";

		const string expected =
@"<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true'>
				<path d='M0 0h24v24H0z' fill='none'/><path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div class='mud-collapse-container' style='' blazor:elementReference='d178b8fb-4478-4ada-9d52-25cccd6e50b8'>
			<div class='mud-collapse-wrapper' blazor:elementReference='99a17eaa-605d-411d-968b-457a412ddd48'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden text<br />Another text</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderDetailsInline()
	{
		const string value =
@"<details><summary>Header</summary>Some hidden text</details>";

		const string expected =
@"<article class='mud-markdown-body'>
	<div class='mud-expand-panel mud-elevation-1 mud-expand-panel-border'>
		<div class='mud-expand-panel-header mud-ripple' blazor:onclick='1'>
			<div class='mud-expand-panel-text'>
				<p class='mud-typography mud-typography-body1'>Header</p>
			</div>
			<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium mud-expand-panel-icon' focusable='false' viewBox='0 0 24 24' aria-hidden='true'>
				<path d='M0 0h24v24H0z' fill='none'/><path d='M16.59 8.59L12 13.17 7.41 8.59 6 10l6 6 6-6z'/>
			</svg>
		</div>
		<div class='mud-collapse-container' style='' blazor:elementReference='8c13559f-c8c5-4ee3-bf41-5693b88ea39a'>
			<div class='mud-collapse-wrapper' blazor:elementReference='5939b12a-7237-44d9-92a5-bbc235724ee5'>
				<div class='mud-collapse-wrapper-inner'>
					<div class='mud-expand-panel-content'>
						<p class='mud-typography mud-typography-body1'>Some hidden tex</p>
					</div>
				</div>
			</div>
		</div>
	</div>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
