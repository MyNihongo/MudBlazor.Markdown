namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentCasesShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderTableWithAdjacentText()
	{
		const string value =
			"""
			text before

			|col1|col2|
			|-|-|
			|cell1|cell2|

			text after
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
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
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderTableWithEmptyCells()
	{
		const string value =
			"""
			|col1|col2|
			|-|-|
			|row1-1|row1-2|
			|row2-1||
			|row3-1|
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
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
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBoldWithinItalic1()
	{
		const string value = "text *italic **bold within***";
		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					text 
					<i>
						italic 
						<b>bold within</b>
					</i>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderBoldWithinItalic2()
	{
		const string value = "text *italic **bold within** more italic*";
		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					text 
					<i>
						italic
						<b>bold within</b>
						more italic
					</i>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenterItalicWithinBold1()
	{
		const string value = "text **bold *italic within***";
		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					text 
					<b>
						bold 
						<i>italic within</i>
					</b>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenterItalicWithinBold2()
	{
		const string value = "text **bold *italic within* more bold**";
		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					text 
					<b>
						bold 
						<i>italic within</i>
						more bold
					</b>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderLinkWithMultipleContent()
	{
		const string value = "[Installing Microsoft Visual C++ Redistributable Package](#installing-microsoft-visual-c-redistributable-package)";
		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					<a href='#installing-microsoft-visual-c-redistributable-package' role='button' blazor:onclick:preventDefault blazor:onclick='1' class='mud-typography mud-link mud-primary-text mud-link-underline-hover mud-typography-body1'>
						Installing Microsoft Visual C++ Redistributable Package
					</a>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderUnderscoreItalics()
	{
		const string value = "Text _italics_";
		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>
					Text
					<i>italics</i>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#region https: //github.com/MyNihongo/MudBlazor.Markdown/issues/64

	[Fact]
	public void RenderHeaderAfterCode()
	{
		const string value =
			"""
			# Heading 1
			Some text.

			```csharp
			public int GetTheAnswer()
			{
			   return 42;
			}
			```

			## Another headline 1
			## Another headline 2
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
				<h1 id='heading-1' class='mud-typography mud-typography-h1'>Heading 1</h1>
				<p class='mud-typography mud-typography-body1'>Some text.</p>
				<div class='snippet-clipboard-content overflow-auto'>
					<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='4debd876-7ce0-4871-af9c-ba021f368d3c'>
						<span class='mud-icon-button-label'>
							<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
								<g><rect fill='none' height='24' width='24'/></g>
								<g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
							</svg>
						</span>
					</button>
					<pre><code class='hljs language-csharp' blazor:elementReference='b2623f71-2ea0-4dd1-94e4-8c03cd62b266'></code></pre>
				</div>
				<h2 id='another-headline-1' class='mud-typography mud-typography-h2'>Another headline 1</h2>
				<h2 id='another-headline-2' class='mud-typography mud-typography-h2'>Another headline 2</h2>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderListAfterCode()
	{
		const string value =
			"""
			```text
			some
			code
			```

			* List item 1
			* List item 2
			* List item 3

			## Another headline
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
				<div class='snippet-clipboard-content overflow-auto'>
					<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='48bbd0ad-a2cf-498a-8fb8-b81d2f4dbeec'>
						<span class='mud-icon-button-label'>
							<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
								<g><rect fill='none' height='24' width='24'/></g><g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
							</svg>
						</span>
					</button>
					<pre><code class='hljs language-text' blazor:elementReference='17023705-9228-4430-906d-725610b1d129'></code></pre>
				</div>
				<ul>
					<li><p class='mud-typography mud-typography-body1'>List item 1</p></li>
					<li><p class='mud-typography mud-typography-body1'>List item 2</p></li>
					<li><p class='mud-typography mud-typography-body1'>List item 3</p></li>
				</ul>
				<h2 id='another-headline' class='mud-typography mud-typography-h2'>Another headline</h2>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion

	#region https: //github.com/MyNihongo/MudBlazor.Markdown/issues/102

	[Fact]
	public void RenderCodeBlockWithoutLanguage()
	{
		const string value =
			"""
			```
			public bool IsMudBlazorCool()
			{
				return true;
			}
			```
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
				<div class='snippet-clipboard-content overflow-auto'>
					<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='84d7bc02-c5ee-472d-a737-b72f42b37b83'>
						<span class='mud-icon-button-label'>
							<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
								<g><rect fill='none' height='24' width='24'/></g>
								<g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
							</svg>
						</span>
					</button>
					<pre><code class='hljs' blazor:elementReference='1010952a-6e9b-4e11-acd5-64bcf75cadd3'></code></pre>
				</div>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion

	#region https: //github.com/MyNihongo/MudBlazor.Markdown/issues/144

	[Fact]
	public void RenderTableInsideList()
	{
		const string value =
			"""
			## The following requirements must be met outside this Terraform code in advance.

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
			8. Created secrets..
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
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
				<ol start='6'>
					<li><p class='mud-typography mud-typography-body1'>Create a new app registration..</p></li>
					<li><p class='mud-typography mud-typography-body1'>Created the key vault..</p></li>
					<li><p class='mud-typography mud-typography-body1'>Created secrets..</p></li>
				</ol>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion

	#region https: //github.com/MyNihongo/MudBlazor.Markdown/issues/233

	[Fact]
	public void RenderListWithWithCompositeListItems()
	{
		const string value =
			"""
			To prevent the warning message regarding the deprecation of the `mysql_native_password` plugin from being logged, you have a couple of options:

			Option 1: Update User Authentication Method:

			1. Connect to your MySQL server using a MySQL client, such as the `mysql` command-line tool:
			   ```bash
			   mysql -u username -p
			   ```

			2. Once connected, run the following command to alter the user's authentication method:
			   ```sql
			   ALTER USER 'username'@'hostname' IDENTIFIED WITH caching_sha2_password;
			   ```
			   Replace `'username'` with the actual username and `'hostname'` with the appropriate hostname or IP address. If you want to update for all users, replace `'username'@'hostname'` with `'*'@'%'`.

			3. Repeat this process for each user on your MySQL server.
			""";

		const string expected =
			"""
			<article class='mud-markdown-body'>
				<p class='mud-typography mud-typography-body1'>To prevent the warning message regarding the deprecation of the <code>mysql_native_password</code> plugin from being logged, you have a couple of options:</p>
				<p class='mud-typography mud-typography-body1'>Option 1: Update User Authentication Method:</p>
				<ol>
					<li>
						<p class='mud-typography mud-typography-body1'>Connect to your MySQL server using a MySQL client, such as the <code>mysql</code> command-line tool:</p>
						<div class='snippet-clipboard-content overflow-auto'>
							<button blazor:onclick='1' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='c5aa6996-aa56-4202-aa6c-9e40b197d739'>
								<span class='mud-icon-button-label'>
									<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
										<g><rect fill='none' height='24' width='24'/></g>
										<g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
									</svg>
								</span>
							</button>
							<pre><code class='hljs language-bash' blazor:elementReference='6353988c-c4bb-496e-a193-44044d50f0eb'></code></pre>
						</div>
					</li>
					<li>
						<p class='mud-typography mud-typography-body1'>Once connected, run the following command to alter the user's authentication method:</p>
						<div class='snippet-clipboard-content overflow-auto'>
							<button blazor:onclick='2' type='button' class='mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple snippet-clipboard-copy-icon ma-2' blazor:onclick:stopPropagation blazor:elementReference='d8bf969c-a5c1-4b9a-bd13-c141af7968c5'>
								<span class='mud-icon-button-label'>
									<svg class='mud-icon-root mud-svg-icon mud-icon-size-medium' focusable='false' viewBox='0 0 24 24' aria-hidden='true' role='img'>
										<g><rect fill='none' height='24' width='24'/></g>
										<g><path d='M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z'/></g>
									</svg>
								</span>
							</button>
							<pre><code class='hljs language-sql' blazor:elementReference='2d167d24-ade4-4ee1-9f4d-28711d608c9b'></code></pre>
						</div>
						<p class='mud-typography mud-typography-body1'>Replace <code>'username'</code> with the actual username and <code>'hostname'</code> with the appropriate hostname or IP address. If you want to update for all users, replace <code>'username'@'hostname'</code> with <code>'*'@'%'</code>.</p>
					</li>
					<li>
						<p class='mud-typography mud-typography-body1'>Repeat this process for each user on your MySQL server.</p>
					</li>
				</ol>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion

	#region https: //github.com/MyNihongo/MudBlazor.Markdown/issues/274

	[Fact]
	public void RenderTableWithWeirdFormat()
	{
		const string value =
			"### My Table: \n\n" +
			"| **Column 1** | **Column 2**   | **Column 3**                |\n" +
			"|--------------|----------------|-----------------------------|\n" +
			"| Row 1, Col 1 | Row 1, Col 2   |                             |\n" +
			"|              | Row 2, Col 2   | Row 2, Col 3                |\n" +
			"| Row 3, Col 1 |                | \u0060\u0060\u0060python    |\n" +
			"|              | Row 4, Col 2   | def greet(name):            |\n" +
			"| Row 5, Col 1 |                |     return name             |\n" +
			"|              |                | \u0060\u0060\u0060          |";

		const string expected =
			"""
			<article class='mud-markdown-body'>
				<h3 id='my-table' class='mud-typography mud-typography-h3'>My Table:</h3>
				<p class='mud-typography mud-typography-body1'>
					<div class='mud-markdown-error'>
						| **Column 1** | **Column 2**   | **Column 3**                |
						|--------------|----------------|-----------------------------|
						| Row 1, Col 1 | Row 1, Col 2   |                             |
						|              | Row 2, Col 2   | Row 2, Col 3                |
						| Row 3, Col 1 |                | ```python    |
						|              | Row 4, Col 2   | def greet(name):            |
						| Row 5, Col 1 |                |     return name             |
						|              |                | ```          |
					</div>
				</p>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	#endregion
}
