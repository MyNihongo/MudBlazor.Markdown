using Bunit;
using Xunit;

namespace MudBlazor.Markdown.Tests
{
	public sealed class MudMarkdownTestCases : MudMarkdownTestsBase
	{
		[Fact]
		public void RenderTableWithAdjacentText()
		{
			const string value =
@"text before

|col1|col2|
|-|-|
|cell1|cell2|

text after";

			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>text before</p>
	<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1 mud-inherit-text'>col1</p></th>
						<th><p class='mud-typography mud-typography-body1 mud-inherit-text'>col2</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell1</p></td>
						<td><p class='mud-typography mud-typography-body1 mud-inherit-text'>cell2</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>text after</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderTableWithEmptyCells()
		{
			const string value =
@"|col1|col2|
|-|-|
|row1-1|row1-2|
|row2-1||
|row3-1|";

			const string expectedValue =
@"<article class='mud-markdown-body'>
   <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1'>
      <div class='mud-table-container'>
         <table>
            <thead>
               <tr>
                  <th>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>col1</p>
                  </th>
                  <th>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>col2</p>
                  </th>
               </tr>
            </thead>
            <tbody>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>row1-1</p>
                  </td>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>row1-2</p>
                  </td>
               </tr>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>row2-1</p>
                  </td>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'></p>
                  </td>
               </tr>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1 mud-inherit-text'>row3-1</p>
                  </td>
                  <td></td>
               </tr>
            </tbody>
         </table>
      </div>
   </div>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderBoldWithinItalic1()
		{
			const string value = "text *italic **bold within***";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		text 
		<i>
			italic 
			<b>bold within</b>
		</i>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenderBoldWithinItalic2()
		{
			const string value = "text *italic **bold within** more italic*";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		text 
		<i>
			italic
			<b>bold within</b>
			more italic
		</i>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenterItalicWithinBold1()
		{
			const string value = "text **bold *italic within***";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		text 
		<b>
			bold 
			<i>italic within</i>
		</b>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}

		[Fact]
		public void RenterItalicWithinBold2()
		{
			const string value = "text **bold *italic within* more bold**";
			const string expectedValue =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1 mud-inherit-text'>
		text 
		<b>
			bold 
			<i>italic within</i>
			more bold
		</b>
	</p>
</article>";

			using var fixture = CreateFixture(value);
			fixture.MarkupMatches(expectedValue);
		}
	}
}
