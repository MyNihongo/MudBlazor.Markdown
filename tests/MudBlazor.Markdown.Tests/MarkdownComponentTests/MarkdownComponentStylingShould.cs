namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentStylingShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderTableNotStriped()
	{
		const string value =
@"|Column1|Column2|Column3|
|-|-|-|
|cell1-1|cell1-2|cell1-3|
|cell2-1|cell2-2|cell2-3|";

		const string expected =
@"<article class='mud-markdown-body'>
	<div class='mud-table mud-simple-table mud-table-bordered mud-elevation-1' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1'>Column1</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column2</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column3</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell1-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-3</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell2-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-3</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
</article>";

		var styling = new MudMarkdownStyling
		{
			Table =
			{
				IsStriped = false,
			},
		};

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}
	
	[Fact]
	public void RenderTableNotBordered()
	{
		const string value =
@"|Column1|Column2|Column3|
|-|-|-|
|cell1-1|cell1-2|cell1-3|
|cell2-1|cell2-2|cell2-3|";

		const string expected =
@"<article class='mud-markdown-body'>
	<div class='mud-table mud-simple-table mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1'>Column1</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column2</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column3</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell1-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-3</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell2-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-3</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
</article>";

		var styling = new MudMarkdownStyling
		{
			Table =
			{
				IsBordered = false,
			},
		};

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderTableNoElevation()
	{
		const string value =
@"|Column1|Column2|Column3|
|-|-|-|
|cell1-1|cell1-2|cell1-3|
|cell2-1|cell2-2|cell2-3|";

		const string expected =
@"<article class='mud-markdown-body'>
	<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-0' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1'>Column1</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column2</p></th>
						<th><p class='mud-typography mud-typography-body1'>Column3</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell1-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell1-3</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell2-1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-2</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2-3</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
</article>";

		var styling = new MudMarkdownStyling
		{
			Table =
			{
				Elevation = 0,
			},
		};

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}
	
	[Fact]
	public void RenderLinkUnderlineDefault()
	{
		const string value = "[my link](https://www.mynihongo.org/)";
		
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a rel='noopener noreferrer' href='https://www.mynihongo.org/' target='_blank' blazor:onclick='1' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>my link</a>
	</p>
</article>";

		var styling = new MudMarkdownStyling();
		
		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLinkUnderlineAlways()
	{
		const string value = "[my link](https://www.mynihongo.org/)";
		
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a rel='noopener noreferrer' href='https://www.mynihongo.org/' target='_blank' blazor:onclick='1' class='mud-typography mud-link mud-primary-text mud-link-underline-always mud-typography-body1'>my link</a>
	</p>
</article>";

		var styling = new MudMarkdownStyling
		{
			Link =
			{
				Underline = Underline.Always,
			},
		};
		
		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}
	
	[Fact]
	public void RenderLinkUnderlineNone()
	{
		const string value = "[my link](https://www.mynihongo.org/)";
		
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a rel='noopener noreferrer' href='https://www.mynihongo.org/' target='_blank' blazor:onclick='1' class='mud-typography mud-link mud-primary-text mud-link-underline-none mud-typography-body1'>my link</a>
	</p>
</article>";

		var styling = new MudMarkdownStyling
		{
			Link =
			{
				Underline = Underline.None,
			},
		};
		
		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}
}
