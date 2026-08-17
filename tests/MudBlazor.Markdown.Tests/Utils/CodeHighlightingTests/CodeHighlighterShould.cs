using System.Text;
using MudBlazor;

namespace MudBlazor.Markdown.Tests.Utils.CodeHighlightingTests;

public sealed class CodeHighlighterShould
{
	[Fact]
	public void HighlightGo()
	{
		var input = Join(
			"// some func",
			"func main() {",
			"    fmt.Println(\"Hello, 世界\")",
			"}");

		var expected = Join(
			"<span class=\"hljs-comment\">// some func</span>",
			"<span class=\"hljs-function\"><span class=\"hljs-keyword\">func</span> <span class=\"hljs-title\">main</span><span class=\"hljs-params\">()</span></span> {",
			"    fmt.Println(<span class=\"hljs-string\">\"Hello, 世界\"</span>)",
			"}");

		Highlight("go", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HighlightKotlin()
	{
		var input = Join(
			"// some func",
			"fun main(args : Array<String>) {",
			"    val num: Int32 = 123",
			"    println(\"Hello, 世界!\")",
			"}");

		var expected = Join(
			"<span class=\"hljs-comment\">// some func</span>",
			"<span class=\"hljs-function\"><span class=\"hljs-keyword\">fun</span> <span class=\"hljs-title\">main</span><span class=\"hljs-params\">(args : <span class=\"hljs-type\">Array</span>&lt;<span class=\"hljs-type\">String</span>&gt;)</span></span> {",
			"    <span class=\"hljs-keyword\">val</span> num: <span class=\"hljs-type\">Int32</span> = <span class=\"hljs-number\">123</span>",
			"    println(<span class=\"hljs-string\">\"Hello, 世界!\"</span>)",
			"}");

		Highlight("kotlin", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HighlightCSharpMethodNames()
	{
		var input = Join(
			"public static void Main()",
			"{",
			"    Console.WriteLine(\"hi\");",
			"}");

		var expected = Join(
			"<span class=\"hljs-keyword\">public</span> <span class=\"hljs-keyword\">static</span> <span class=\"hljs-type\">void</span> <span class=\"hljs-title\">Main</span>()",
			"{",
			"    Console.<span class=\"hljs-title\">WriteLine</span>(<span class=\"hljs-string\">\"hi\"</span>);",
			"}");

		Highlight("csharp", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HighlightRazorHtmlElement()
	{
		const string input = "<div class=\"card\">Hello</div>";

		const string expected =
			"<span class=\"hljs-tag\">&lt;<span class=\"hljs-name\">div</span> <span class=\"hljs-attr\">class</span>=<span class=\"hljs-string\">\"card\"</span>&gt;</span>" +
			"Hello" +
			"<span class=\"hljs-tag\">&lt;/<span class=\"hljs-name\">div</span>&gt;</span>";

		Highlight("razor", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void ReturnNullForUnsupportedLanguage() =>
		CodeHighlighterFactory.Create("python")
			.Should()
			.BeNull();

	[Fact]
	public void HighlightCSharpTypes()
	{
		const string input = "public Guid Id { get; set; }";

		const string expected =
			"<span class=\"hljs-keyword\">public</span> " +
			"<span class=\"hljs-type\">Guid</span> " +
			"Id { " +
			"<span class=\"hljs-keyword\">get</span>; " +
			"<span class=\"hljs-keyword\">set</span>; }";

		Highlight("csharp", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HighlightCSharpGenericType()
	{
		const string input = "Array<String> items";

		const string expected =
			"<span class=\"hljs-type\">Array</span>&lt;<span class=\"hljs-type\">String</span>&gt; items";

		Highlight("csharp", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void HighlightRazorCSharpTypes()
	{
		var input = Join(
			"private List<TaskItem> tasks = new();",
			"public Guid Id { get; set; }");

		var expected = Join(
			"<span class=\"hljs-keyword\">private</span> <span class=\"hljs-type\">List</span>&lt;<span class=\"hljs-type\">TaskItem</span>&gt; tasks = <span class=\"hljs-keyword\">new</span>();",
			"<span class=\"hljs-keyword\">public</span> <span class=\"hljs-type\">Guid</span> Id { <span class=\"hljs-keyword\">get</span>; <span class=\"hljs-keyword\">set</span>; }");

		Highlight("razor", input)
			.Should()
			.Be(expected);
	}

	[Fact]
	public void DoesNotHighlightMemberNamesAsTypes()
	{
		var input = Join(
			"public string Title { get; set; } = string.Empty;",
			"public bool IsCompleted { get; set; }");

		var expected = Join(
			"<span class=\"hljs-keyword\">public</span> <span class=\"hljs-type\">string</span> Title { <span class=\"hljs-keyword\">get</span>; <span class=\"hljs-keyword\">set</span>; } = <span class=\"hljs-type\">string</span>.Empty;",
			"<span class=\"hljs-keyword\">public</span> <span class=\"hljs-type\">bool</span> IsCompleted { <span class=\"hljs-keyword\">get</span>; <span class=\"hljs-keyword\">set</span>; }");

		Highlight("csharp", input)
			.Should()
			.Be(expected);
	}

	private static string Highlight(string language, string code)
	{
		var highlighter = CodeHighlighterFactory.Create(language);
		highlighter.Should().NotBeNull();

		var sb = new StringBuilder();
		foreach (var node in highlighter!.Highlight(code))
			Append(sb, node);

		return sb.ToString();
	}

	private static void Append(StringBuilder sb, CodeNode node)
	{
		switch (node)
		{
			case CodeText text:
				sb.Append(Escape(text.Value));
				break;
			case CodeSpan span:
				sb.Append("<span class=\"").Append(span.ClassName).Append("\">");
				foreach (var child in span.Children)
					Append(sb, child);
				sb.Append("</span>");
				break;
		}
	}

	// Matches how Blazor's RenderTreeBuilder.AddContent escapes text.
	private static string Escape(string value) =>
		value
			.Replace("&", "&amp;")
			.Replace("<", "&lt;")
			.Replace(">", "&gt;");

	private static string Join(params string[] lines) =>
		string.Join('\n', lines);
}
