namespace MudBlazor.Markdown.Tests.MarkdownComponentTests;

public sealed class MarkdownComponentCasesShould : MarkdownComponentTestsBase
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

		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>text before</p>
	<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1'>col1</p></th>
						<th><p class='mud-typography mud-typography-body1'>col2</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>cell1</p></td>
						<td><p class='mud-typography mud-typography-body1'>cell2</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
	<p class='mud-typography mud-typography-body1'>text after</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
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

		const string expected =
@"<article class='mud-markdown-body'>
   <div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
      <div class='mud-table-container'>
         <table>
            <thead>
               <tr>
                  <th>
                     <p class='mud-typography mud-typography-body1'>col1</p>
                  </th>
                  <th>
                     <p class='mud-typography mud-typography-body1'>col2</p>
                  </th>
               </tr>
            </thead>
            <tbody>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1'>row1-1</p>
                  </td>
                  <td>
                     <p class='mud-typography mud-typography-body1'>row1-2</p>
                  </td>
               </tr>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1'>row2-1</p>
                  </td>
                  <td>
                     <p class='mud-typography mud-typography-body1'></p>
                  </td>
               </tr>
               <tr>
                  <td>
                     <p class='mud-typography mud-typography-body1'>row3-1</p>
                  </td>
                  <td></td>
               </tr>
            </tbody>
         </table>
      </div>
   </div>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBoldWithinItalic1()
	{
		const string value = "text *italic **bold within***";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		text 
		<i>
			italic 
			<b>bold within</b>
		</i>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBoldWithinItalic2()
	{
		const string value = "text *italic **bold within** more italic*";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		text 
		<i>
			italic
			<b>bold within</b>
			more italic
		</i>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenterItalicWithinBold1()
	{
		const string value = "text **bold *italic within***";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		text 
		<b>
			bold 
			<i>italic within</i>
		</b>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenterItalicWithinBold2()
	{
		const string value = "text **bold *italic within* more bold**";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		text 
		<b>
			bold 
			<i>italic within</i>
			more bold
		</b>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLinkWithMultipleContent()
	{
		const string value = "[Installing Microsoft Visual C++ Redistributable Package](#installing-microsoft-visual-c-redistributable-package)";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		<a href='#installing-microsoft-visual-c-redistributable-package' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
			Installing Microsoft Visual C&#x2B;&#x2B; Redistributable Package
		</a>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderUnderscoreItalics()
	{
		const string value = "Text _italics_";
		const string expected =
@"<article class='mud-markdown-body'>
	<p class='mud-typography mud-typography-body1'>
		Text
		<i>italics</i>
	</p>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#region https://github.com/MyNihongo/MudBlazor.Markdown/issues/64

	[Fact]
	public void RenderHeaderAfterCode()
	{
		const string value =
@"# Heading 1
Some text.

```csharp
public int GetTheAnswer()
{
   return 42;
}
```

## Another headline 1
## Another headline 2";

		const string expected =
@"<article class='mud-markdown-body'>
	<h1 id='heading-1' class='mud-typography mud-typography-h1'>Heading 1</h1>
	<p class='mud-typography mud-typography-body1'>Some text.</p>
	<pre><code blazor:elementReference='8035dc45-0e97-419e-869c-51a5d65602d4' class='language-csharp'>public int GetTheAnswer()&#xD;&#xA;{&#xD;&#xA;   return 42;&#xD;&#xA;}</code></pre>
	<h2 id='another-headline-1' class='mud-typography mud-typography-h2'>Another headline 1</h2>
	<h2 id='another-headline-2' class='mud-typography mud-typography-h2'>Another headline 2</h2>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderListAfterCode()
	{
		const string value =
@"```text
some
code
```

* List item 1
* List item 2
* List item 3

## Another headline";

		const string expected = 
@"<article class='mud-markdown-body'>
	<pre><code blazor:elementReference='9d940986-b033-4d4d-97f0-2c11f46dda30' class='language-text'>some&#xD;&#xA;code</code></pre>
	<ul>
		<li><p class='mud-typography mud-typography-body1'>List item 1</p></li>
		<li><p class='mud-typography mud-typography-body1'>List item 2</p></li>
		<li><p class='mud-typography mud-typography-body1'>List item 3</p></li>
	</ul>
	<h2 id='another-headline' class='mud-typography mud-typography-h2'>Another headline</h2>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion

	#region https://github.com/MyNihongo/MudBlazor.Markdown/issues/102

	[Fact]
	public void RenderCodeBlockWithoutLanguage()
	{
		const string value =
@"```
public bool IsMudBlazorCool()
{
	return true;
}
```";

		const string expected =
@"<article class='mud-markdown-body'>
	<pre><code blazor:elementReference='3acd6993-4f90-4ff3-b73c-670bc06e8008'>public bool IsMudBlazorCool()&#xD;&#xA;{&#xD;&#xA;&#x9;return true;&#xD;&#xA;}</code></pre>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion

	#region https://github.com/MyNihongo/MudBlazor.Markdown/issues/144

	[Fact]
	public void RenderTableInsideList()
	{
		const string value = 
@"## The following requirements must be met outside this Terraform code in advance.

1. Created a new resource group..
2. Created a new AAD group using naming convention..
3. Created a new AAD group using naming convention..
4. Created a separate storage account..
5. Created a new app registration..
     
  |Permission | Type | Granted Through|
  |--|--|--|
  |User.Read | Delegated | Admin consent|
  | User.ReadBasic.All |Delegated| Admin consent|
  |User.Read.All|Delegated| Admin consent|
  |Directory.Read.Alll|Delegated| Admin consent|
  |Directory.AccessAsUser.All|Delegated| Admin consent|
  |User.Invite.All|Application| Admin consent|
  |Directory.Read.All|Application| Admin consent|
  |User.Read.All|Application| Admin consent|
  - Granted permissions User.Invite.All and GroupMember.ReadWrite.All. Admin consent has been granted
 
6. Create a new app registration..
7. Created the key vault..
8. Created secrets..";

		const string expected =
@"<article class='mud-markdown-body'>
	<h2 id='the-following-requirements-must-be-met-outside-this-terraform-code-in-advance.' class='mud-typography mud-typography-h2'>
		The following requirements must be met outside this Terraform code in advance.
	</h2>
	<ol>
		<li><p class='mud-typography mud-typography-body1'>Created a new resource group..</p></li>
		<li><p class='mud-typography mud-typography-body1'>Created a new AAD group using naming convention..</p></li>
		<li><p class='mud-typography mud-typography-body1'>Created a new AAD group using naming convention..</p></li>
		<li><p class='mud-typography mud-typography-body1'>Created a separate storage account..</p></li>
		<li><p class='mud-typography mud-typography-body1'>Created a new app registration..</p></li>
	</ol>
	<div class='mud-table mud-simple-table mud-table-bordered mud-table-striped mud-elevation-1' style='overflow-x: auto;'>
		<div class='mud-table-container'>
			<table>
				<thead>
					<tr>
						<th><p class='mud-typography mud-typography-body1'>Permission</p></th>
						<th><p class='mud-typography mud-typography-body1'>Type</p></th>
						<th><p class='mud-typography mud-typography-body1'>Granted Through</p></th>
					</tr>
				</thead>
				<tbody>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>User.Read</p></td>
						<td><p class='mud-typography mud-typography-body1'>Delegated</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>User.ReadBasic.All</p></td>
						<td><p class='mud-typography mud-typography-body1'>Delegated</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>User.Read.All</p></td>
						<td><p class='mud-typography mud-typography-body1'>Delegated</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>Directory.Read.Alll</p></td>
						<td><p class='mud-typography mud-typography-body1'>Delegated</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>Directory.AccessAsUser.All</p></td>
						<td><p class='mud-typography mud-typography-body1'>Delegated</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>User.Invite.All</p></td>
						<td><p class='mud-typography mud-typography-body1'>Application</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>Directory.Read.All</p></td>
						<td><p class='mud-typography mud-typography-body1'>Application</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
					<tr>
						<td><p class='mud-typography mud-typography-body1'>User.Read.All</p></td>
						<td><p class='mud-typography mud-typography-body1'>Application</p></td>
						<td><p class='mud-typography mud-typography-body1'>Admin consent</p></td>
					</tr>
				</tbody>
			</table>
		</div>
	</div>
	<ul>
		<li><p class='mud-typography mud-typography-body1'>Granted permissions User.Invite.All and GroupMember.ReadWrite.All. Admin consent has been granted</p></li>
	</ul>
	<ol>
		<li><p class='mud-typography mud-typography-body1'>Create a new app registration..</p></li>
		<li><p class='mud-typography mud-typography-body1'>Created the key vault..</p></li>
		<li><p class='mud-typography mud-typography-body1'>Created secrets..</p></li>
	</ol>
</article>";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion
}
