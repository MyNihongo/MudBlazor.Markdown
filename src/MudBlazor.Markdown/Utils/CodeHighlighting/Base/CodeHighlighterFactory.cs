using System.Collections.Concurrent;

namespace MudBlazor;

/// <summary>
/// Resolves the <see cref="ICodeHighlighter"/> for a Markdown code-block language.<br/>
/// Returns <see langword="null"/> for languages that are not supported natively (the caller then
/// falls back to the client-side highlighter).
/// </summary>
internal static class CodeHighlighterFactory
{
	private const string CSharpLanguage = "csharp";
	private const string GoLanguage = "go";
	private const string KotlinLanguage = "kotlin";
	private const string RazorLanguage = "razor";
	private const string RustLanguage = "rust";
	private const string JsonLanguage = "json";
	private const string XmlLanguage = "xml";
	private const string YamlLanguage = "yaml";

	private static readonly ConcurrentDictionary<string, ICodeHighlighter?> CodeHighlighters = new();

	/// <summary>
	/// Returns the highlighter for <paramref name="language"/>, or <see langword="null"/> if unsupported.
	/// </summary>
	public static ICodeHighlighter? Create(string? language)
	{
		var key = Normalize(language);

		return CodeHighlighters.GetOrAdd(key, static key =>
		{
			return key switch
			{
				CSharpLanguage => new CSharpCodeHighlighter(),
				GoLanguage => new GoCodeHighlighter(),
				KotlinLanguage => new KotlinCodeHighlighter(),
				RazorLanguage => new RazorCodeHighlighter(),
				RustLanguage => new RustCodeHighlighter(),
				JsonLanguage => new JsonCodeHighlighter(),
				XmlLanguage => new XmlCodeHighlighter(),
				YamlLanguage => new YamlCodeHighlighter(),
				_ => null,
			};
		});
	}

	private static string Normalize(string? language)
	{
		return language?.Trim().ToLowerInvariant() switch
		{
			"c#" or "cs" or CSharpLanguage => CSharpLanguage,
			GoLanguage or "golang" => GoLanguage,
			KotlinLanguage or "kt" or "kts" => KotlinLanguage,
			RazorLanguage or "cshtml" or "razor-cshtml" => RazorLanguage,
			RustLanguage or "rs" => RustLanguage,
			JsonLanguage => JsonLanguage,
			XmlLanguage or "html" or "xhtml" or "svg" or "xsd" or "xsl" or "xslt" => XmlLanguage,
			YamlLanguage or "yml" => YamlLanguage,
			_ => string.Empty,
		};
	}
}
