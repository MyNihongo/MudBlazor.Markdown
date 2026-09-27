namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentTableShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderTable()
	{
		const string value =
			"""
			|Column1|Column2|Column3|
			|-|-|-|
			|cell1-1|cell1-2|cell1-3|
			|cell2-1|cell2-2|cell2-3|
			""";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
				<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
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
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Theory]
	[InlineData("<br>")]
	[InlineData("<br/>")]
	[InlineData("<br />")]
	[InlineData("<BR>")]
	[InlineData("<BR/>")]
	[InlineData("<BR />")]
	public void RenderTableWithNewLines(string newLineChar)
	{
		var value =
			$"""
			 |1|2|
			 |-|-|
			 |a{newLineChar}b|c
			 """;

		var expected =
			$"""
			 <article id:ignore class='mud-markdown-body'>
			    <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
			       <div class='mud-table-container'>
			          <table>
			             <thead>
			                <tr>
			                   <th>
			                      <p class='mud-typography mud-typography-body1'>1</p>
			                   </th>
			                   <th>
			                      <p class='mud-typography mud-typography-body1'>2</p>
			                   </th>
			                </tr>
			             </thead>
			             <tbody>
			                <tr>
			                   <td>
			                      <p class='mud-typography mud-typography-body1'>a{newLineChar}b</p>
			                   </td>
			                   <td>
			                      <p class='mud-typography mud-typography-body1'>c</p>
			                   </td>
			                </tr>
			             </tbody>
			          </table>
			       </div>
			    </div>
			 </article>
			 """;

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderTableMinWidth()
	{
		const string value =
			"""
			|col1|col2|
			|-|-|
			|cell1|cell2|
			""";

		const string expected =
			"""
			<article id:ignore class='mud-markdown-body'>
			   <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
			      <div class='mud-table-container'>
			         <table>
			            <thead>
			               <tr>
			                  <th style='min-width:200px'>
			                     <p class='mud-typography mud-typography-body1'>col1</p>
			                  </th>
			                  <th style='min-width:200px'>
			                     <p class='mud-typography mud-typography-body1'>col2</p>
			                  </th>
			               </tr>
			            </thead>
			            <tbody>
			               <tr>
			                  <td>
			                     <p class='mud-typography mud-typography-body1'>cell1</p>
			                  </td>
			                  <td>
			                     <p class='mud-typography mud-typography-body1'>cell2</p>
			                  </td>
			               </tr>
			            </tbody>
			         </table>
			      </div>
			   </div>
			</article>
			""";

		var styling = new MudMarkdownStyling
		{
			Table =
			{
				CellMinWidth = 200,
			},
		};

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void OverrideTypo()
	{
		const string value =
			"""
			|Column1|Column2|
			|-|-|
			|cell1|cell2|
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
				<div class="mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1" style="overflow-x: auto;">
					<div class="mud-table-container">
						<table>
							<thead>
								<tr>
									<th><h3 class="mud-typography mud-typography-h3">Column1</h3></th>
									<th><h3 class="mud-typography mud-typography-h3">Column2</h3></th>
								</tr>
							</thead>
							<tbody>
								<tr>
									<td><h3 class="mud-typography mud-typography-h3">cell1</h3></td>
									<td><h3 class="mud-typography mud-typography-h3">cell2</h3></td>
								</tr>
							</tbody>
						</table>
					</div>
				</div>
			</article>
			""";

		var props = new MudMarkdownProps
		{
			Body =
			{
				OverrideTypo = Typo.h3,
			},
		};

		using var fixture = CreateFixture(value, props: props);
		fixture.MarkupMatches(expected);
	}
}
