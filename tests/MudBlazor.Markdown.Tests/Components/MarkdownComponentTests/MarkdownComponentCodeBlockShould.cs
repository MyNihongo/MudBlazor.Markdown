namespace MudBlazor.Markdown.Tests.Components.MarkdownComponentTests;

public sealed class MarkdownComponentCodeBlockShould : MarkdownComponentTestsBase
{
	[Fact]
	public void RenderCodeBlock()
	{
		const string value =
			"""
			```cs
			public bool IsMudBlazorCool()
			{
				return true;
			}
			```
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			  <div class="hljs mud-markdown-code-highlight">
			    <button
			      blazor:onclick="2"
			      type="button"
			      class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			      blazor:onclick:stopPropagation
			      blazor:elementReference=""
			    >
			      <span class="mud-icon-button-label"
			        ><svg
			          class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			          focusable="false"
			          viewBox="0 0 24 24"
			          aria-hidden="true"
			          role="img"
			        >
			          <g><rect fill="none" height="24" width="24" /></g>
			          <g>
			            <path
			              d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			            />
			          </g></svg
			      ></span>
			    </button>
			    <pre><code class="hljs language-cs"><span class="hljs-keyword">public</span> <span class="hljs-type">bool</span> <span class="hljs-title">IsMudBlazorCool</span>()
			{
				<span class="hljs-keyword">return</span> <span class="hljs-literal">true</span>;
			}</code></pre>
			  </div>
			</article>
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCodeBlockIndented()
	{
		const string value =
			"""
			    if (condition)
			    {
			        return;
			    }
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			  <div class="hljs mud-markdown-code-highlight">
			    <button
			      blazor:onclick="2"
			      type="button"
			      class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			      blazor:onclick:stopPropagation
			      blazor:elementReference=""
			    >
			      <span class="mud-icon-button-label"
			        ><svg
			          class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			          focusable="false"
			          viewBox="0 0 24 24"
			          aria-hidden="true"
			          role="img"
			        >
			          <g><rect fill="none" height="24" width="24" /></g>
			          <g>
			            <path
			              d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			            />
			          </g></svg
			      ></span>
			    </button>
			    <pre><code class="hljs">if (condition)
			{
			    return;
			}</code></pre>
			  </div>
			</article>

			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCodeBlockWithoutCopyButton()
	{
		const string value =
			"""
			```cs
			public bool IsMudBlazorCool()
			{
				return true;
			}
			```
			""";

		var styling = new MudMarkdownStyling
		{
			CodeBlock =
			{
				CopyButton = CodeBlockCopyButton.None,
			},
		};

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			  <div class="hljs mud-markdown-code-highlight">
			    <pre><code class="hljs language-cs"><span class="hljs-keyword">public</span> <span class="hljs-type">bool</span> <span class="hljs-title">IsMudBlazorCool</span>()
			{
				<span class="hljs-keyword">return</span> <span class="hljs-literal">true</span>;
			}</code></pre>
			  </div>
			</article>
			""";

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCodeBlockWithStickyButton()
	{
		const string value =
			"""
			```cs
			public bool IsMudBlazorCool()
			{
				return true;
			}
			```
			""";

		var styling = new MudMarkdownStyling
		{
			CodeBlock =
			{
				CopyButton = CodeBlockCopyButton.Sticky,
			},
		};

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			  <div class="hljs mud-markdown-code-highlight-sticky">
			    <button
			      blazor:onclick="2"
			      type="button"
			      class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn-sticky"
			      blazor:onclick:stopPropagation
			      blazor:elementReference=""
			    >
			      <span class="mud-icon-button-label"
			        ><svg
			          class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			          focusable="false"
			          viewBox="0 0 24 24"
			          aria-hidden="true"
			          role="img"
			        >
			          <g><rect fill="none" height="24" width="24" /></g>
			          <g>
			            <path
			              d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			            />
			          </g></svg
			      ></span>
			    </button>
			    <pre><code class="hljs language-cs"><span class="hljs-keyword">public</span> <span class="hljs-type">bool</span> <span class="hljs-title">IsMudBlazorCool</span>()
			{
				<span class="hljs-keyword">return</span> <span class="hljs-literal">true</span>;
			}</code></pre>
			  </div>
			</article>
			""";

		using var fixture = CreateFixture(value, styling: styling);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderCSharp()
	{
		const string value =
			""""
			```cs
			#region Directive Test
			#nullable enable
			using System;
			using System.Collections.Generic;
			using System.Threading.Tasks;

			// Alias and global using
			using StringList = System.Collections.Generic.List<string>;
			global using System.Text;
			#endregion

			namespace SyntaxHighlightingTest.Core;

			/// <summary>
			/// XML Documentation comment testing <see cref="ITestInterface{T}"/>
			/// </summary>

			// Attributes
			[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
			public sealed class TestAttribute(string category, int priority = 1) : Attribute
			{
			    public string Category { get; } = category;
			    public int Priority { get; } = priority;
			}

			// Interface
			public interface ITestInterface<T> where T : class
			{
			    event EventHandler? OnCompleted;
			    T? Execute(in T input, out bool success);
			}

			// Record Struct & Primary Constructor
			public readonly record struct Point(double X, double Y);

			// Enumeration
			public enum Status : byte
			{
			    None = 0,
			    Active = 1,
			    Pending = 2,
			    Error = 255
			}

			// Class with Generics and Inheritance
			[Test("Highlighting", Priority = 10)]
			public class SyntaxTester<T> : ITestInterface<T> where T : class, new()
			{
			    // Fields
			    private static readonly Lazy<SyntaxTester<T>> _instance = new(() => new SyntaxTester<T>());
			    private volatile bool _isRunning = false;
			    private const double MaxThreshold = 3.14159_26535_89793;
			    private const decimal DecimalValue = 123.321m;
			    private const long LongValue = 123321L;

			    // Delegate and Event
			    public delegate void CustomDelegate(ref string message, params object[] args);
			    public event EventHandler? OnCompleted;

			    // Property with Expression-Bodied Member & Modifiers
			    public static SyntaxTester<T> Instance => _instance.Value;
			    public required string Identifier { get; init; }
			    public Status CurrentStatus { get; private set; } = Status.None;

			    // Method with async, pattern matching, tuple, and switch expression
			    public async Task<(bool Success, string Message)> ProcessAsync(object? rawInput, CancellationToken ct = default)
			    {
			        // Null checks and pattern matching
			        if (rawInput is not T validObject)
			        {
			            return (false, $"Input is invalid or not of type {nameof(T)}.");
			        }

			        // Lock & Async/Await
			        lock (this)
			        {
			            _isRunning = true;
			        }

			        try
			        {
			            await Task.Delay(100, ct).ConfigureAwait(false);

			            // Pattern matching switch expression
			            string resultDescription = validObject switch
			            {
			                IComparable c when c.CompareTo(default) > 0 => "Positive comparable",
			                Point(var x, var y) when x > 0 && y > 0 => $"Quadrant 1 Point at ({x}, {y})",
			                string { Length: > 5 } s => $"Long string: {s}",
			                null => throw new ArgumentNullException(nameof(rawInput)),
			                _ => "Default object status"
			            };

			            // LINQ query syntax
			            int[] numbers = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
			            var query = from num in numbers
			                        where num % 2 == 0
			                        orderby num descending
			                        select new { Original = num, Squared = num * num };

			            // Local function with ref/out
			            static bool TryFormat(ref int val, out string formatted)
			            {
			                val *= 2;
			                formatted = $"Formatted_{val}";
			                return true;
			            }

			            int valueToRef = 42;
			            _ = TryFormat(ref valueToRef, out string formattedResult);

			            return (true, $"{resultDescription} | {formattedResult}");
			        }
			        catch (Exception ex) when (ex is not OperationCanceledException)
			        {
			            return (false, $"Error: {ex.GetMessage()}");
			        }
			        finally
			        {
			            _isRunning = false;
			            OnCompleted?.Invoke(this, EventArgs.Empty);
			        }
			    }

			    // Explicit Interface Implementation
			    T? ITestInterface<T>.Execute(in T input, out bool success)
			    {
			        // Unsafe code block & Pointers
			        unsafe
			        {
			            int val = 100;
			            int* ptr = &val;
			            *ptr = 200;
			        }

			        // Literals, Interpolation, Raw Strings, and Escape Characters
			        string verbatimStr = @"C:\Program Files\TestFolder\file.txt";
			        string rawJson = """
			            {
			               "key": "value",
			               "escaped": "Hello \"World\""
			            }
			            """;

			        success = true;
			        return input;
			    }

			    // Operator Overloading
			    public static bool operator ==(SyntaxTester<T>? left, SyntaxTester<T>? right) => Equals(left, right);
			    public static bool operator !=(SyntaxTester<T>? left, SyntaxTester<T>? right) => !Equals(left, right);
			    public override bool Equals(object? obj) => base.Equals(obj);
			    public override int GetHashCode() => base.GetHashCode();
			}
			```
			"""";

		const string expected =
			""""
			<article id:ignore class="mud-markdown-body">
			    <div class="hljs mud-markdown-code-highlight">
			        <button
			            blazor:onclick="2"
			            type="button"
			            class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			            blazor:onclick:stopPropagation
			            blazor:elementReference=""
			        >
			            <span class="mud-icon-button-label"
			                ><svg
			                    class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			                    focusable="false"
			                    viewBox="0 0 24 24"
			                    aria-hidden="true"
			                    role="img"
			                >
			                    <g><rect fill="none" height="24" width="24" /></g>
			                    <g>
			                        <path
			                            d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			                        />
			                    </g></svg
			            ></span>
			        </button>
			        <pre><code class="hljs language-cs"><span class="hljs-meta">#region</span> Directive Test
			<span class="hljs-meta">#nullable</span> enable
			<span class="hljs-keyword">using</span> System;
			<span class="hljs-keyword">using</span> System.Collections.Generic;
			<span class="hljs-keyword">using</span> System.Threading.Tasks;
			
			<span class="hljs-comment">// Alias and global using
			</span>
			<span class="hljs-keyword">using</span> StringList = System.Collections.Generic.<span class="hljs-type">List</span>&lt;<span class="hljs-type">string</span>&gt;;
			<span class="hljs-keyword">global</span> <span class="hljs-keyword">using</span> System.Text;
			<span class="hljs-meta">#endregion</span>
			
			<span class="hljs-keyword">namespace</span> SyntaxHighlightingTest.Core;
			
			<span class="hljs-comment">/// &lt;summary&gt;
			</span>
			<span class="hljs-comment">/// XML Documentation comment testing &lt;see cref="ITestInterface{T}"/&gt;
			</span>
			<span class="hljs-comment">/// &lt;/summary&gt;
			</span>
			
			<span class="hljs-comment">// Attributes
			</span>
			[<span class="hljs-title">AttributeUsage</span>(AttributeTargets.Class | AttributeTargets.Method)]
			<span class="hljs-keyword">public</span> <span class="hljs-keyword">sealed</span> <span class="hljs-keyword">class</span> <span class="hljs-title">TestAttribute</span>(<span class="hljs-type">string</span> category, <span class="hljs-type">int</span> priority = <span class="hljs-number">1</span>) : <span class="hljs-type">Attribute</span>
			{
			    <span class="hljs-keyword">public</span> <span class="hljs-type">string</span> Category { <span class="hljs-keyword">get</span>; } = category;
			    <span class="hljs-keyword">public</span> <span class="hljs-type">int</span> Priority { <span class="hljs-keyword">get</span>; } = priority;
			}
			
			<span class="hljs-comment">// Interface
			</span>
			<span class="hljs-keyword">public</span> <span class="hljs-keyword">interface</span> <span class="hljs-type">ITestInterface</span>&lt;<span class="hljs-type">T</span>&gt; <span class="hljs-keyword">where</span> T : <span class="hljs-keyword">class</span>
			{
			    <span class="hljs-keyword">event</span> <span class="hljs-type">EventHandler</span>? OnCompleted;
			    <span class="hljs-type">T</span>? <span class="hljs-title">Execute</span>(<span class="hljs-keyword">in</span> <span class="hljs-type">T</span> input, <span class="hljs-keyword">out</span> <span class="hljs-type">bool</span> success);
			}
			
			<span class="hljs-comment">// Record Struct &amp; Primary Constructor
			</span>
			<span class="hljs-keyword">public</span> <span class="hljs-keyword">readonly</span> <span class="hljs-keyword">record</span> <span class="hljs-keyword">struct</span> <span class="hljs-title">Point</span>(<span class="hljs-type">double</span> X, <span class="hljs-type">double</span> Y);
			
			<span class="hljs-comment">// Enumeration
			</span>
			<span class="hljs-keyword">public</span> <span class="hljs-keyword">enum</span> Status : <span class="hljs-type">byte</span>
			{
			    None = <span class="hljs-number">0</span>,
			    Active = <span class="hljs-number">1</span>,
			    Pending = <span class="hljs-number">2</span>,
			    Error = <span class="hljs-number">255</span>
			}
			
			<span class="hljs-comment">// Class with Generics and Inheritance
			</span>
			[<span class="hljs-title">Test</span>(<span class="hljs-string">"Highlighting"</span>, Priority = <span class="hljs-number">10</span>)]
			<span class="hljs-keyword">public</span> <span class="hljs-keyword">class</span> <span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt; : <span class="hljs-type">ITestInterface</span>&lt;<span class="hljs-type">T</span>&gt; <span class="hljs-keyword">where</span> T : <span class="hljs-keyword">class</span>, <span class="hljs-keyword">new</span>()
			{
			    <span class="hljs-comment">// Fields
			</span>
			    <span class="hljs-keyword">private</span> <span class="hljs-keyword">static</span> <span class="hljs-keyword">readonly</span> <span class="hljs-type">Lazy</span>&lt;<span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt;&gt; _instance = <span class="hljs-keyword">new</span>(() =&gt; <span class="hljs-keyword">new</span> <span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt;());
			    <span class="hljs-keyword">private</span> <span class="hljs-keyword">volatile</span> <span class="hljs-type">bool</span> _isRunning = <span class="hljs-literal">false</span>;
			    <span class="hljs-keyword">private</span> <span class="hljs-keyword">const</span> <span class="hljs-type">double</span> MaxThreshold = <span class="hljs-number">3.14159_26535_89793</span>;
			    <span class="hljs-keyword">private</span> <span class="hljs-keyword">const</span> <span class="hljs-type">decimal</span> DecimalValue = <span class="hljs-number">123.321m</span>;
			    <span class="hljs-keyword">private</span> <span class="hljs-keyword">const</span> <span class="hljs-type">long</span> LongValue = <span class="hljs-number">123321L</span>;
			
			    <span class="hljs-comment">// Delegate and Event
			</span>
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">delegate</span> <span class="hljs-type">void</span> <span class="hljs-title">CustomDelegate</span>(<span class="hljs-keyword">ref</span> <span class="hljs-type">string</span> message, <span class="hljs-keyword">params</span> <span class="hljs-type">object</span>[] args);
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">event</span> <span class="hljs-type">EventHandler</span>? OnCompleted;
			
			    <span class="hljs-comment">// Property with Expression-Bodied Member &amp; Modifiers
			</span>
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">static</span> <span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt; Instance =&gt; _instance.Value;
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">required</span> <span class="hljs-type">string</span> Identifier { <span class="hljs-keyword">get</span>; <span class="hljs-keyword">init</span>; }
			    <span class="hljs-keyword">public</span> <span class="hljs-type">Status</span> CurrentStatus { <span class="hljs-keyword">get</span>; <span class="hljs-keyword">private</span> <span class="hljs-keyword">set</span>; } = Status.None;
			
			    <span class="hljs-comment">// Method with async, pattern matching, tuple, and switch expression
			</span>
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">async</span> <span class="hljs-type">Task</span>&lt;(<span class="hljs-type">bool</span> <span class="hljs-type">Success</span>, <span class="hljs-type">string</span> <span class="hljs-type">Message</span>)&gt; <span class="hljs-title">ProcessAsync</span>(<span class="hljs-type">object</span>? rawInput, <span class="hljs-type">CancellationToken</span> ct = <span class="hljs-keyword">default</span>)
			    {
			        <span class="hljs-comment">// Null checks and pattern matching
			</span>
			        <span class="hljs-keyword">if</span> (rawInput <span class="hljs-keyword">is</span> <span class="hljs-keyword">not</span> <span class="hljs-type">T</span> validObject)
			        {
			            <span class="hljs-keyword">return</span> (<span class="hljs-literal">false</span>, <span class="hljs-string">$"Input is invalid or not of type <span class="hljs-subst">{<span class="hljs-keyword">nameof</span>(T)}</span>."</span>);
			        }
			
			        <span class="hljs-comment">// Lock &amp; Async/Await
			</span>
			        <span class="hljs-keyword">lock</span> (<span class="hljs-keyword">this</span>)
			        {
			            _isRunning = <span class="hljs-literal">true</span>;
			        }
			
			        <span class="hljs-keyword">try</span>
			        {
			            <span class="hljs-keyword">await</span> Task.<span class="hljs-title">Delay</span>(<span class="hljs-number">100</span>, ct).<span class="hljs-title">ConfigureAwait</span>(<span class="hljs-literal">false</span>);
			
			            <span class="hljs-comment">// Pattern matching switch expression
			</span>
			            <span class="hljs-type">string</span> resultDescription = validObject <span class="hljs-keyword">switch</span>
			            {
			                <span class="hljs-type">IComparable</span> c <span class="hljs-keyword">when</span> c.<span class="hljs-title">CompareTo</span>(<span class="hljs-keyword">default</span>) &gt; <span class="hljs-number">0</span> =&gt; <span class="hljs-string">"Positive comparable"</span>,
			                <span class="hljs-title">Point</span>(<span class="hljs-keyword">var</span> x, <span class="hljs-keyword">var</span> y) <span class="hljs-keyword">when</span> x &gt; <span class="hljs-number">0</span> &amp;&amp; y &gt; <span class="hljs-number">0</span> =&gt; <span class="hljs-string">$"Quadrant 1 Point at (<span class="hljs-subst">{x}</span>, <span class="hljs-subst">{y}</span>)"</span>,
			                <span class="hljs-type">string</span> { Length: &gt; <span class="hljs-number">5</span> } s =&gt; <span class="hljs-string">$"Long string: <span class="hljs-subst">{s}</span>"</span>,
			                <span class="hljs-literal">null</span> =&gt; <span class="hljs-keyword">throw</span> <span class="hljs-keyword">new</span> <span class="hljs-type">ArgumentNullException</span>(<span class="hljs-keyword">nameof</span>(rawInput)),
			                _ =&gt; <span class="hljs-string">"Default object status"</span>
			            };
			
			            <span class="hljs-comment">// LINQ query syntax
			</span>
			            <span class="hljs-type">int</span>[] numbers = [<span class="hljs-number">1</span>, <span class="hljs-number">2</span>, <span class="hljs-number">3</span>, <span class="hljs-number">4</span>, <span class="hljs-number">5</span>, <span class="hljs-number">6</span>, <span class="hljs-number">7</span>, <span class="hljs-number">8</span>, <span class="hljs-number">9</span>, <span class="hljs-number">10</span>];
			            <span class="hljs-keyword">var</span> query = from num <span class="hljs-keyword">in</span> numbers
			                        <span class="hljs-keyword">where</span> num % <span class="hljs-number">2</span> == <span class="hljs-number">0</span>
			                        orderby num descending
			                        select <span class="hljs-keyword">new</span> { Original = num, Squared = num * num };
			
			            <span class="hljs-comment">// Local function with ref/out
			</span>
			            <span class="hljs-keyword">static</span> <span class="hljs-type">bool</span> <span class="hljs-title">TryFormat</span>(<span class="hljs-keyword">ref</span> <span class="hljs-type">int</span> val, <span class="hljs-keyword">out</span> <span class="hljs-type">string</span> formatted)
			            {
			                val *= <span class="hljs-number">2</span>;
			                formatted = <span class="hljs-string">$"Formatted_<span class="hljs-subst">{val}</span>"</span>;
			                <span class="hljs-keyword">return</span> <span class="hljs-literal">true</span>;
			            }
			
			            <span class="hljs-type">int</span> valueToRef = <span class="hljs-number">42</span>;
			            _ = <span class="hljs-title">TryFormat</span>(<span class="hljs-keyword">ref</span> valueToRef, <span class="hljs-keyword">out</span> <span class="hljs-type">string</span> formattedResult);
			
			            <span class="hljs-keyword">return</span> (<span class="hljs-literal">true</span>, <span class="hljs-string">$"<span class="hljs-subst">{resultDescription}</span> | <span class="hljs-subst">{formattedResult}</span>"</span>);
			        }
			        <span class="hljs-keyword">catch</span> (<span class="hljs-type">Exception</span> ex) <span class="hljs-keyword">when</span> (ex <span class="hljs-keyword">is</span> <span class="hljs-keyword">not</span> <span class="hljs-type">OperationCanceledException</span>)
			        {
			            <span class="hljs-keyword">return</span> (<span class="hljs-literal">false</span>, <span class="hljs-string">$"Error: <span class="hljs-subst">{ex.<span class="hljs-title">GetMessage</span>()}</span>"</span>);
			        }
			        <span class="hljs-keyword">finally</span>
			        {
			            _isRunning = <span class="hljs-literal">false</span>;
			            OnCompleted?.<span class="hljs-title">Invoke</span>(<span class="hljs-keyword">this</span>, EventArgs.Empty);
			        }
			    }
			
			    <span class="hljs-comment">// Explicit Interface Implementation
			</span>
			    <span class="hljs-type">T</span>? <span class="hljs-type">ITestInterface</span>&lt;<span class="hljs-type">T</span>&gt;.<span class="hljs-title">Execute</span>(<span class="hljs-keyword">in</span> <span class="hljs-type">T</span> input, <span class="hljs-keyword">out</span> <span class="hljs-type">bool</span> success)
			    {
			        <span class="hljs-comment">// Unsafe code block &amp; Pointers
			</span>
			        <span class="hljs-keyword">unsafe</span>
			        {
			            <span class="hljs-type">int</span> val = <span class="hljs-number">100</span>;
			            <span class="hljs-type">int</span>* ptr = &amp;val;
			            *ptr = <span class="hljs-number">200</span>;
			        }
			
			        <span class="hljs-comment">// Literals, Interpolation, Raw Strings, and Escape Characters
			</span>
			        <span class="hljs-type">string</span> verbatimStr = <span class="hljs-string">@"C:\Program Files\TestFolder\file.txt"</span>;
			        <span class="hljs-type">string</span> rawJson = <span class="hljs-string">"""
			            {
			               "key": "value",
			               "escaped": "Hello \"World\""
			            }
			            """</span>;
			
			        success = <span class="hljs-literal">true</span>;
			        <span class="hljs-keyword">return</span> input;
			    }
			
			    <span class="hljs-comment">// Operator Overloading
			</span>
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">static</span> <span class="hljs-type">bool</span> <span class="hljs-keyword">operator</span> ==(<span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt;? left, <span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt;? right) =&gt; <span class="hljs-title">Equals</span>(left, right);
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">static</span> <span class="hljs-type">bool</span> <span class="hljs-keyword">operator</span> !=(<span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt;? left, <span class="hljs-type">SyntaxTester</span>&lt;<span class="hljs-type">T</span>&gt;? right) =&gt; !<span class="hljs-title">Equals</span>(left, right);
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">override</span> <span class="hljs-type">bool</span> <span class="hljs-title">Equals</span>(<span class="hljs-type">object</span>? obj) =&gt; <span class="hljs-keyword">base</span>.<span class="hljs-title">Equals</span>(obj);
			    <span class="hljs-keyword">public</span> <span class="hljs-keyword">override</span> <span class="hljs-type">int</span> <span class="hljs-title">GetHashCode</span>() =&gt; <span class="hljs-keyword">base</span>.<span class="hljs-title">GetHashCode</span>();
			}</code></pre>
			    </div>
			</article>
			
			"""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderKotlin()
	{
		const string value =
			""""
			```kotlin
			@file:Suppress("UNUSED_VARIABLE", "NOTHING_TO_INLINE")

			package com.syntax.highlighting.test

			import java.io.IOException
			import kotlin.contracts.ExperimentalContracts
			import kotlin.contracts.contract
			import kotlin.properties.Delegates

			// Annotations & Typealiases
			@Target(AnnotationTarget.CLASS, AnnotationTarget.FUNCTION)
			@Retention(AnnotationRetention.RUNTIME)
			annotation class TestAnnotation(val priority: Int = 1, val name: String)

			typealias StringMap<T> = Map<String, T>

			// Interfaces & Sealed Interfaces
			sealed interface Identifiable {
			    val id: Long
			}

			interface Processable<in T : Any, out R> where R : Comparable<R> {
			    suspend fun process(input: T): R
			}

			// Sealed Class Hierarchy
			sealed class State {
			    data object Idle : State()
			    data class Active(val startTime: Long) : State()
			    data class Error(val cause: Throwable) : State()
			}

			// Enum Class
			enum class Priority(val value: Int) {
			    LOW(0),
			    MEDIUM(5),
			    HIGH(10);

			    fun isUrgent(): Boolean = this == HIGH
			}

			// Data Class & Value Class (Value class inline test)
			@JvmInline
			value class UserId(val value: Long)

			data class User(
			    override val id: Long,
			    val username: String,
			    val email: String? = null,
			    val roles: List<String> = emptyList()
			) : Identifiable

			// Class, Generics, Secondary Constructor & Inheritance
			@TestAnnotation(priority = 10, name = "KotlinSyntaxTester")
			open class BaseTester protected constructor(open val name: String) {
			    constructor() : this("DefaultBase")
			}

			class KotlinSyntaxTester<T : Any>(
			    override val name: String,
			    private val delegate: Processable<T, String>
			) : BaseTester(name), Identifiable {

			    // Properties, Modifiers, Lateinit & Delegates
			    override val id: Long = 1001L
			    
			    lateinit var uninitializedString: String
			    
			    var observedProperty: String by Delegates.observable("Initial") { prop, old, new ->
			        println("${prop.name}: $old -> $new")
			    }

			    val lazyValue: String by lazy(LazyThreadSafetyMode.SYNCHRONIZED) {
			        "Computed Lazily"
			    }

			    // Companion Object
			    companion object {
			        const val MAX_RETRIES: Int = 3
			        private const val BASE_URL: String = "https://api.example.com/v1"

			        @JvmStatic
			        fun createDefault(): KotlinSyntaxTester<String> {
			            val dummyProcessor = object : Processable<String, String> {
			                override suspend fun process(input: String): String = input.uppercase()
			            }
			            return KotlinSyntaxTester("Default", dummyProcessor)
			        }
			    }

			    // Functions, Vararg, Infix, Extension Functions, and Nullability
			    infix fun String.concatWith(other: String): String = "$this - $other"

			    fun processItems(vararg items: T?): List<String> {
			        val results = mutableListOf<String>()

			        for (item in items) {
			            // Safe call, Elvis operator, and smart casting
			            val label = item?.toString() ?: "NULL_VALUE"
			            results.add(label)
			        }

			        return results
			    }

			    // Control Flow, Pattern Matching (When), Destructuring, Collections
			    suspend fun execute(state: State, numbers: List<Int>): String {
			        // When expression as a statement/expression
			        val stateName = when (state) {
			            is State.Idle -> "System is idle"
			            is State.Active -> "Active since ${state.startTime}"
			            is State.Error -> throw state.cause
			        }

			        // Loops and Ranges
			        var sum = 0
			        for (i in 0 until 10 step 2) {
			            if (i == 4) continue
			            sum += i
			        }

			        while (sum > 0) {
			            sum--
			            if (sum == 2) break
			        }

			        // Functional Operators & Lambdas
			        val processedNumbers = numbers
			            .filter { it % 2 == 0 }
			            .map { num ->
			                val doubled = num * 2
			                doubled
			            }

			        // Strings: Multi-line / Raw Strings & Interpolation
			        val rawJson = """
			            {
			                "status": "$stateName",
			                "count": ${processedNumbers.size},
			                "escaped": "Hello \"World\""
			            }
			        """.trimIndent()

			        // Try-Catch as Expression
			        val parsedValue: Int? = try {
			            "123".toInt()
			        } catch (e: NumberFormatException) {
			            null
			        } finally {
			            // Cleanup block
			        }

			        return rawJson
			    }

			    // Inline Function & Contracts
			    @OptIn(ExperimentalContracts::class)
			    inline fun performAction(block: () -> Unit) {
			        contract {
			            callsInPlace(block, kotlin.contracts.InvocationKind.EXACTLY_ONCE)
			        }
			        block()
			    }
			}
			```
			"""";

		const string expected =
			""""
			<article id:ignore class="mud-markdown-body">
			    <div class="hljs mud-markdown-code-highlight">
			        <button
			            blazor:onclick="2"
			            type="button"
			            class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			            blazor:onclick:stopPropagation
			            blazor:elementReference=""
			        >
			            <span class="mud-icon-button-label"
			                ><svg
			                    class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			                    focusable="false"
			                    viewBox="0 0 24 24"
			                    aria-hidden="true"
			                    role="img"
			                >
			                    <g><rect fill="none" height="24" width="24" /></g>
			                    <g>
			                        <path
			                            d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			                        />
			                    </g></svg
			            ></span>
			        </button>
			        <pre><code class="hljs language-kotlin"><span class="hljs-meta">@file:Suppress</span>(<span class="hljs-string">"UNUSED_VARIABLE"</span>, <span class="hljs-string">"NOTHING_TO_INLINE"</span>)
			
			<span class="hljs-keyword">package</span> com.syntax.highlighting.test
			
			<span class="hljs-keyword">import</span> java.io.IOException
			<span class="hljs-keyword">import</span> kotlin.contracts.ExperimentalContracts
			<span class="hljs-keyword">import</span> kotlin.contracts.contract
			<span class="hljs-keyword">import</span> kotlin.properties.Delegates
			
			<span class="hljs-comment">// Annotations &amp; Typealiases
			</span>
			<span class="hljs-meta">@Target</span>(<span class="hljs-type">AnnotationTarget</span>.CLASS, <span class="hljs-type">AnnotationTarget</span>.FUNCTION)
			<span class="hljs-meta">@Retention</span>(<span class="hljs-type">AnnotationRetention</span>.RUNTIME)
			<span class="hljs-keyword">annotation</span> <span class="hljs-keyword">class</span> <span class="hljs-type">TestAnnotation</span>(<span class="hljs-keyword">val</span> priority: <span class="hljs-type">Int</span> = <span class="hljs-number">1</span>, <span class="hljs-keyword">val</span> name: <span class="hljs-type">String</span>)
			
			<span class="hljs-keyword">typealias</span> <span class="hljs-type">StringMap</span>&lt;<span class="hljs-type">T</span>&gt; = <span class="hljs-type">Map</span>&lt;<span class="hljs-type">String</span>, <span class="hljs-type">T</span>&gt;
			
			<span class="hljs-comment">// Interfaces &amp; Sealed Interfaces
			</span>
			<span class="hljs-keyword">sealed</span> <span class="hljs-keyword">interface</span> <span class="hljs-type">Identifiable</span> {
			    <span class="hljs-keyword">val</span> id: <span class="hljs-type">Long</span>
			}
			
			<span class="hljs-keyword">interface</span> <span class="hljs-type">Processable</span>&lt;<span class="hljs-keyword">in</span> <span class="hljs-type">T</span> : <span class="hljs-type">Any</span>, <span class="hljs-keyword">out</span> <span class="hljs-type">R</span>&gt; where R : <span class="hljs-type">Comparable</span>&lt;<span class="hljs-type">R</span>&gt; {
			    <span class="hljs-keyword">suspend</span> <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">process</span><span class="hljs-params">(input: <span class="hljs-type">T</span>)</span></span>: <span class="hljs-type">R</span>
			}
			
			<span class="hljs-comment">// Sealed Class Hierarchy
			</span>
			<span class="hljs-keyword">sealed</span> <span class="hljs-keyword">class</span> <span class="hljs-type">State</span> {
			    <span class="hljs-keyword">data</span> <span class="hljs-keyword">object</span> <span class="hljs-type">Idle</span> : <span class="hljs-type">State</span>()
			    <span class="hljs-keyword">data</span> <span class="hljs-keyword">class</span> <span class="hljs-type">Active</span>(<span class="hljs-keyword">val</span> startTime: <span class="hljs-type">Long</span>) : <span class="hljs-type">State</span>()
			    <span class="hljs-keyword">data</span> <span class="hljs-keyword">class</span> <span class="hljs-type">Error</span>(<span class="hljs-keyword">val</span> cause: <span class="hljs-type">Throwable</span>) : <span class="hljs-type">State</span>()
			}
			
			<span class="hljs-comment">// Enum Class
			</span>
			<span class="hljs-keyword">enum</span> <span class="hljs-keyword">class</span> <span class="hljs-type">Priority</span>(<span class="hljs-keyword">val</span> value: <span class="hljs-type">Int</span>) {
			    <span class="hljs-title">LOW</span>(<span class="hljs-number">0</span>),
			    <span class="hljs-title">MEDIUM</span>(<span class="hljs-number">5</span>),
			    <span class="hljs-title">HIGH</span>(<span class="hljs-number">10</span>);
			
			    <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">isUrgent</span><span class="hljs-params">()</span></span>: <span class="hljs-type">Boolean</span> = <span class="hljs-keyword">this</span> == HIGH
			}
			
			<span class="hljs-comment">// Data Class &amp; Value Class (Value class inline test)
			</span>
			<span class="hljs-meta">@JvmInline</span>
			value <span class="hljs-keyword">class</span> <span class="hljs-type">UserId</span>(<span class="hljs-keyword">val</span> value: <span class="hljs-type">Long</span>)
			
			<span class="hljs-keyword">data</span> <span class="hljs-keyword">class</span> <span class="hljs-type">User</span>(
			    <span class="hljs-keyword">override</span> <span class="hljs-keyword">val</span> id: <span class="hljs-type">Long</span>,
			    <span class="hljs-keyword">val</span> username: <span class="hljs-type">String</span>,
			    <span class="hljs-keyword">val</span> email: <span class="hljs-type">String</span>? = <span class="hljs-literal">null</span>,
			    <span class="hljs-keyword">val</span> roles: <span class="hljs-type">List</span>&lt;<span class="hljs-type">String</span>&gt; = <span class="hljs-title">emptyList</span>()
			) : <span class="hljs-type">Identifiable</span>
			
			<span class="hljs-comment">// Class, Generics, Secondary Constructor &amp; Inheritance
			</span>
			<span class="hljs-meta">@TestAnnotation</span>(priority = <span class="hljs-number">10</span>, name = <span class="hljs-string">"KotlinSyntaxTester"</span>)
			<span class="hljs-keyword">open</span> <span class="hljs-keyword">class</span> <span class="hljs-type">BaseTester</span> <span class="hljs-keyword">protected</span> <span class="hljs-keyword">constructor</span>(<span class="hljs-keyword">open</span> <span class="hljs-keyword">val</span> name: <span class="hljs-type">String</span>) {
			    <span class="hljs-keyword">constructor</span>() : <span class="hljs-keyword">this</span>(<span class="hljs-string">"DefaultBase"</span>)
			}
			
			<span class="hljs-keyword">class</span> <span class="hljs-type">KotlinSyntaxTester</span>&lt;<span class="hljs-type">T</span> : <span class="hljs-type">Any</span>&gt;(
			    <span class="hljs-keyword">override</span> <span class="hljs-keyword">val</span> name: <span class="hljs-type">String</span>,
			    <span class="hljs-keyword">private</span> <span class="hljs-keyword">val</span> delegate: <span class="hljs-type">Processable</span>&lt;<span class="hljs-type">T</span>, <span class="hljs-type">String</span>&gt;
			) : <span class="hljs-type">BaseTester</span>(name), <span class="hljs-type">Identifiable</span> {
			
			    <span class="hljs-comment">// Properties, Modifiers, Lateinit &amp; Delegates
			</span>
			    <span class="hljs-keyword">override</span> <span class="hljs-keyword">val</span> id: <span class="hljs-type">Long</span> = <span class="hljs-number">1001L</span>
			    
			    <span class="hljs-keyword">lateinit</span> <span class="hljs-keyword">var</span> uninitializedString: <span class="hljs-type">String</span>
			    
			    <span class="hljs-keyword">var</span> observedProperty: <span class="hljs-type">String</span> <span class="hljs-keyword">by</span> <span class="hljs-type">Delegates</span>.<span class="hljs-title">observable</span>(<span class="hljs-string">"Initial"</span>) { prop, old, new -&gt;
			        <span class="hljs-title">println</span>(<span class="hljs-string">"<span class="hljs-subst">${prop.name}</span>: <span class="hljs-subst">$old</span> -&gt; <span class="hljs-subst">$new</span>"</span>)
			    }
			
			    <span class="hljs-keyword">val</span> lazyValue: <span class="hljs-type">String</span> <span class="hljs-keyword">by</span> <span class="hljs-title">lazy</span>(<span class="hljs-type">LazyThreadSafetyMode</span>.SYNCHRONIZED) {
			        <span class="hljs-string">"Computed Lazily"</span>
			    }
			
			    <span class="hljs-comment">// Companion Object
			</span>
			    <span class="hljs-keyword">companion</span> <span class="hljs-keyword">object</span> {
			        <span class="hljs-keyword">const</span> <span class="hljs-keyword">val</span> MAX_RETRIES: <span class="hljs-type">Int</span> = <span class="hljs-number">3</span>
			        <span class="hljs-keyword">private</span> <span class="hljs-keyword">const</span> <span class="hljs-keyword">val</span> BASE_URL: <span class="hljs-type">String</span> = <span class="hljs-string">"https://api.example.com/v1"</span>
			
			        <span class="hljs-meta">@JvmStatic</span>
			        <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">createDefault</span><span class="hljs-params">()</span></span>: <span class="hljs-type">KotlinSyntaxTester</span>&lt;<span class="hljs-type">String</span>&gt; {
			            <span class="hljs-keyword">val</span> dummyProcessor = <span class="hljs-keyword">object</span> : <span class="hljs-type">Processable</span>&lt;<span class="hljs-type">String</span>, <span class="hljs-type">String</span>&gt; {
			                <span class="hljs-keyword">override</span> <span class="hljs-keyword">suspend</span> <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">process</span><span class="hljs-params">(input: <span class="hljs-type">String</span>)</span></span>: <span class="hljs-type">String</span> = input.<span class="hljs-title">uppercase</span>()
			            }
			            <span class="hljs-keyword">return</span> <span class="hljs-type">KotlinSyntaxTester</span>(<span class="hljs-string">"Default"</span>, dummyProcessor)
			        }
			    }
			
			    <span class="hljs-comment">// Functions, Vararg, Infix, Extension Functions, and Nullability
			</span>
			    <span class="hljs-keyword">infix</span> <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">String</span></span>.<span class="hljs-title">concatWith</span>(other: <span class="hljs-type">String</span>): <span class="hljs-type">String</span> = <span class="hljs-string">"<span class="hljs-subst">$this</span> - <span class="hljs-subst">$other</span>"</span>
			
			    <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">processItems</span><span class="hljs-params">(<span class="hljs-keyword">vararg</span> items: <span class="hljs-type">T</span>?)</span></span>: <span class="hljs-type">List</span>&lt;<span class="hljs-type">String</span>&gt; {
			        <span class="hljs-keyword">val</span> results = <span class="hljs-title">mutableListOf</span>&lt;<span class="hljs-type">String</span>&gt;()
			
			        <span class="hljs-keyword">for</span> (item <span class="hljs-keyword">in</span> items) {
			            <span class="hljs-comment">// Safe call, Elvis operator, and smart casting
			</span>
			            <span class="hljs-keyword">val</span> label = item?.<span class="hljs-title">toString</span>() ?: <span class="hljs-string">"NULL_VALUE"</span>
			            results.<span class="hljs-title">add</span>(label)
			        }
			
			        <span class="hljs-keyword">return</span> results
			    }
			
			    <span class="hljs-comment">// Control Flow, Pattern Matching (When), Destructuring, Collections
			</span>
			    <span class="hljs-keyword">suspend</span> <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">execute</span><span class="hljs-params">(state: <span class="hljs-type">State</span>, numbers: <span class="hljs-type">List</span>&lt;<span class="hljs-type">Int</span>&gt;)</span></span>: <span class="hljs-type">String</span> {
			        <span class="hljs-comment">// When expression as a statement/expression
			</span>
			        <span class="hljs-keyword">val</span> stateName = <span class="hljs-keyword">when</span> (state) {
			            <span class="hljs-keyword">is</span> <span class="hljs-type">State</span>.Idle -&gt; <span class="hljs-string">"System is idle"</span>
			            <span class="hljs-keyword">is</span> <span class="hljs-type">State</span>.Active -&gt; <span class="hljs-string">"Active since <span class="hljs-subst">${state.startTime}</span>"</span>
			            <span class="hljs-keyword">is</span> <span class="hljs-type">State</span>.Error -&gt; <span class="hljs-keyword">throw</span> state.cause
			        }
			
			        <span class="hljs-comment">// Loops and Ranges
			</span>
			        <span class="hljs-keyword">var</span> sum = <span class="hljs-number">0</span>
			        <span class="hljs-keyword">for</span> (i <span class="hljs-keyword">in</span> <span class="hljs-number">0</span> until <span class="hljs-number">10</span> step <span class="hljs-number">2</span>) {
			            <span class="hljs-keyword">if</span> (i == <span class="hljs-number">4</span>) <span class="hljs-keyword">continue</span>
			            sum += i
			        }
			
			        <span class="hljs-keyword">while</span> (sum &gt; <span class="hljs-number">0</span>) {
			            sum--
			            <span class="hljs-keyword">if</span> (sum == <span class="hljs-number">2</span>) <span class="hljs-keyword">break</span>
			        }
			
			        <span class="hljs-comment">// Functional Operators &amp; Lambdas
			</span>
			        <span class="hljs-keyword">val</span> processedNumbers = numbers
			            .<span class="hljs-title">filter</span> { it % <span class="hljs-number">2</span> == <span class="hljs-number">0</span> }
			            .<span class="hljs-title">map</span> { num -&gt;
			                <span class="hljs-keyword">val</span> doubled = num * <span class="hljs-number">2</span>
			                doubled
			            }
			
			        <span class="hljs-comment">// Strings: Multi-line / Raw Strings &amp; Interpolation
			</span>
			        <span class="hljs-keyword">val</span> rawJson = <span class="hljs-string">"""
			            {
			                "status": "<span class="hljs-subst">$stateName</span>",
			                "count": <span class="hljs-subst">${processedNumbers.size}</span>,
			                "escaped": "Hello \"World\""
			            }
			        """</span>.<span class="hljs-title">trimIndent</span>()
			
			        <span class="hljs-comment">// Try-Catch as Expression
			</span>
			        <span class="hljs-keyword">val</span> parsedValue: <span class="hljs-type">Int</span>? = <span class="hljs-keyword">try</span> {
			            <span class="hljs-string">"123"</span>.<span class="hljs-title">toInt</span>()
			        } <span class="hljs-keyword">catch</span> (e: <span class="hljs-type">NumberFormatException</span>) {
			            <span class="hljs-literal">null</span>
			        } <span class="hljs-keyword">finally</span> {
			            <span class="hljs-comment">// Cleanup block
			</span>
			        }
			
			        <span class="hljs-keyword">return</span> rawJson
			    }
			
			    <span class="hljs-comment">// Inline Function &amp; Contracts
			</span>
			    <span class="hljs-meta">@OptIn</span>(<span class="hljs-type">ExperimentalContracts</span>::<span class="hljs-keyword">class</span>)
			    <span class="hljs-keyword">inline</span> <span class="hljs-function"><span class="hljs-keyword">fun</span> <span class="hljs-title">performAction</span><span class="hljs-params">(block: () -&gt; <span class="hljs-type">Unit</span>)</span></span> {
			        <span class="hljs-title">contract</span> {
			            <span class="hljs-title">callsInPlace</span>(block, kotlin.contracts.InvocationKind.EXACTLY_ONCE)
			        }
			        <span class="hljs-title">block</span>()
			    }
			}</code></pre>
			    </div>
			</article>
			
			"""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderGoLang()
	{
		const string value =
			"""
			```golang
			// Package declaration and imports
			package main

			import (
				"context"
				"errors"
				"fmt"
				"math"
				"sync"
				"time"
			)

			// Constants and iota enumeration
			const (
				MaxThreshold float64 = 3.14159_26535_89793
				CategoryName         = "HighlightingTest"
			)

			const (
				StatusNone byte = iota
				StatusActive
				StatusPending
				StatusError
			)

			// Type Aliases and Defined Types
			type Status = byte
			type ID int64

			// Interfaces and Generics (Type Constraints)
			type Stringable interface {
				fmt.Stringer
				~string | ~int
			}

			type Processor[T any] interface {
				Process(ctx context.Context, input T) (T, error)
			}

			// Struct Definition with Tags
			type User struct {
				ID        ID        `json:"id" db:"user_id"`
				Username  string    `json:"username"`
				IsActive  bool      `json:"is_active"`
				CreatedAt time.Time `json:"created_at"`
			}

			// Method on Struct (Value Receiver)
			func (u User) String() string {
				return fmt.Sprintf("User(%d, %s)", u.ID, u.Username)
			}

			// Method on Struct (Pointer Receiver)
			func (u *User) Deactivate() {
				u.IsActive = false
			}

			// Generic Struct
			type Container[T Stringable] struct {
				Value T
			}

			// Main Function
			func main() {
				// Variable Declarations (var, short declaration)
				var explicitInt int = 42
				var uninitializedString string
				shortBool := true
				_ = uninitializedString // Blank identifier

				// Numeric Literals (Hex, Octal, Binary, Floats, Imaginary)
				hexVal := 0xFF
				octalVal := 0o755
				binaryVal := 0b101010
				complexVal := 1.2 + 3.4i

				// Built-in Primitive Types
				var (
					b   uint8  = 255
					r   rune   = '⌘'
					f   float32 = 3.14
					err error   = nil
				)

				// Built-in Data Structures: Slice, Map, Channel
				numbers := []int{10, 20, 30, 40, 50}
				strMap := map[string]int{"alpha": 1, "beta": 2}
				ch := make(chan string, 2)
				defer close(ch) // Defer statement

				// Control Flow: If-Else with Short Initialization
				if length := len(numbers); length > 0 && shortBool {
					fmt.Printf("Slice length: %d\n", length)
				} else if length == 0 {
					fmt.Println("Empty slice")
				} else {
					fmt.Println("Fallback condition")
				}

				// For Loops (Standard, Range, Infinite)
				for i := 0; i < len(numbers); i++ {
					if i == 1 {
						continue
					}
					if i == 3 {
						break
					}
				}

				for key, val := range strMap {
					_ = fmt.Sprintf("Key: %s, Val: %d", key, val)
				}

				// Switch Statement (Expression and Type Switch)
				switch hexVal {
				case 0xFF:
					fallthrough
				case 0xFE:
					break
				default:
					fmt.Println("Unknown byte")
				}

				var genericVar interface{} = "Test String"
				switch v := genericVar.(type) {
				case int:
					fmt.Printf("Integer: %d\n", v)
				case string:
					fmt.Printf("String: %s\n", v)
				default:
					fmt.Println("Unknown type")
				}

				// Concurrency: Goroutine, Channels, Select, Sync
				var wg sync.WaitGroup
				wg.Add(1)

				go func(msg string) {
					defer wg.Done()
					ch <- msg
				}(categoryFormat("Concurrency Test"))

				select {
				case res := <-ch:
					fmt.Println("Received:", res)
				case <-time.After(100 * time.Millisecond):
					fmt.Println("Timeout")
				}

				wg.Wait()

				// Anonymous Function / Closure & Error Handling
				res, err := safeDivide(10.0, 0.0)
				if err != nil {
					_ = fmt.Errorf("operation failed: %w", err)
				}

				// Raw Strings, Verbatim, and Escapes
				rawString := `Line 1
			Line 2 with "quotes" and \no escapes\`
				interpretedString := "Line 1\nLine 2 with \"escapes\""

				_, _ = res, rawString
				_, _ = b, r
			}

			// Function with Multiple Return Values & Named Parameters
			func safeDivide(a, b float64) (result float64, err error) {
				// Panic & Recover
				defer func() {
					if r := recover(); r != nil {
						err = fmt.Errorf("recovered from panic: %v", r)
					}
				}()

				if b == 0 {
					panic("division by zero")
				}
				return a / b, nil
			}

			// Variadic Function
			func categoryFormat(format string, args ...any) string {
				return fmt.Sprintf(format, args...)
			}
			```
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			    <div class="hljs mud-markdown-code-highlight">
			        <button
			            blazor:onclick="2"
			            type="button"
			            class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			            blazor:onclick:stopPropagation
			            blazor:elementReference=""
			        >
			            <span class="mud-icon-button-label"
			                ><svg
			                    class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			                    focusable="false"
			                    viewBox="0 0 24 24"
			                    aria-hidden="true"
			                    role="img"
			                >
			                    <g><rect fill="none" height="24" width="24" /></g>
			                    <g>
			                        <path
			                            d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			                        />
			                    </g></svg
			            ></span>
			        </button>
			        <pre><code class="hljs language-golang"><span class="hljs-comment">// Package declaration and imports
			</span>
			<span class="hljs-keyword">package</span> main
			
			<span class="hljs-keyword">import</span> (
				<span class="hljs-string">"context"</span>
				<span class="hljs-string">"errors"</span>
				<span class="hljs-string">"fmt"</span>
				<span class="hljs-string">"math"</span>
				<span class="hljs-string">"sync"</span>
				<span class="hljs-string">"time"</span>
			)
			
			<span class="hljs-comment">// Constants and iota enumeration
			</span>
			<span class="hljs-keyword">const</span> (
				MaxThreshold <span class="hljs-type">float64</span> = <span class="hljs-number">3.14159_26535_89793</span>
				CategoryName         = <span class="hljs-string">"HighlightingTest"</span>
			)
			
			<span class="hljs-keyword">const</span> (
				StatusNone <span class="hljs-type">byte</span> = <span class="hljs-literal">iota</span>
				StatusActive
				StatusPending
				StatusError
			)
			
			<span class="hljs-comment">// Type Aliases and Defined Types
			</span>
			<span class="hljs-keyword">type</span> <span class="hljs-type">Status</span> = <span class="hljs-type">byte</span>
			<span class="hljs-keyword">type</span> <span class="hljs-type">ID</span> <span class="hljs-type">int64</span>
			
			<span class="hljs-comment">// Interfaces and Generics (Type Constraints)
			</span>
			<span class="hljs-keyword">type</span> <span class="hljs-type">Stringable</span> <span class="hljs-keyword">interface</span> {
				fmt.Stringer
				~<span class="hljs-type">string</span> | ~<span class="hljs-type">int</span>
			}
			
			<span class="hljs-keyword">type</span> <span class="hljs-type">Processor</span>[<span class="hljs-type">T</span> <span class="hljs-type">any</span>] <span class="hljs-keyword">interface</span> {
				<span class="hljs-title">Process</span>(ctx context.Context, input <span class="hljs-type">T</span>) (<span class="hljs-type">T</span>, <span class="hljs-type">error</span>)
			}
			
			<span class="hljs-comment">// Struct Definition with Tags
			</span>
			<span class="hljs-keyword">type</span> <span class="hljs-type">User</span> <span class="hljs-keyword">struct</span> {
				ID        <span class="hljs-type">ID</span>        <span class="hljs-string">`json:"id" db:"user_id"`</span>
				Username  <span class="hljs-type">string</span>    <span class="hljs-string">`json:"username"`</span>
				IsActive  <span class="hljs-type">bool</span>      <span class="hljs-string">`json:"is_active"`</span>
				CreatedAt time.Time <span class="hljs-string">`json:"created_at"`</span>
			}
			
			<span class="hljs-comment">// Method on Struct (Value Receiver)
			</span>
			<span class="hljs-function"><span class="hljs-keyword">func</span> <span class="hljs-params">(u <span class="hljs-type">User</span>)</span></span> <span class="hljs-title">String</span>() <span class="hljs-type">string</span> {
				<span class="hljs-keyword">return</span> fmt.<span class="hljs-title">Sprintf</span>(<span class="hljs-string">"User(%d, %s)"</span>, u.ID, u.Username)
			}
			
			<span class="hljs-comment">// Method on Struct (Pointer Receiver)
			</span>
			<span class="hljs-function"><span class="hljs-keyword">func</span> <span class="hljs-params">(u *<span class="hljs-type">User</span>)</span></span> <span class="hljs-title">Deactivate</span>() {
				u.IsActive = <span class="hljs-literal">false</span>
			}
			
			<span class="hljs-comment">// Generic Struct
			</span>
			<span class="hljs-keyword">type</span> <span class="hljs-type">Container</span>[<span class="hljs-type">T</span> <span class="hljs-type">Stringable</span>] <span class="hljs-keyword">struct</span> {
				Value <span class="hljs-type">T</span>
			}
			
			<span class="hljs-comment">// Main Function
			</span>
			<span class="hljs-function"><span class="hljs-keyword">func</span> <span class="hljs-title">main</span><span class="hljs-params">()</span></span> {
				<span class="hljs-comment">// Variable Declarations (var, short declaration)
			</span>
				<span class="hljs-keyword">var</span> explicitInt <span class="hljs-type">int</span> = <span class="hljs-number">42</span>
				<span class="hljs-keyword">var</span> uninitializedString <span class="hljs-type">string</span>
				shortBool := <span class="hljs-literal">true</span>
				_ = uninitializedString <span class="hljs-comment">// Blank identifier
			</span>
			
				<span class="hljs-comment">// Numeric Literals (Hex, Octal, Binary, Floats, Imaginary)
			</span>
				hexVal := <span class="hljs-number">0xFF</span>
				octalVal := <span class="hljs-number">0o</span><span class="hljs-number">755</span>
				binaryVal := <span class="hljs-number">0b</span><span class="hljs-number">101010</span>
				complexVal := <span class="hljs-number">1.2</span> + <span class="hljs-number">3.4i</span>
			
				<span class="hljs-comment">// Built-in Primitive Types
			</span>
				<span class="hljs-keyword">var</span> (
					b   <span class="hljs-type">uint8</span>  = <span class="hljs-number">255</span>
					r   <span class="hljs-type">rune</span>   = <span class="hljs-string">'⌘'</span>
					f   <span class="hljs-type">float32</span> = <span class="hljs-number">3.14</span>
					err <span class="hljs-type">error</span>   = <span class="hljs-literal">nil</span>
				)
			
				<span class="hljs-comment">// Built-in Data Structures: Slice, Map, Channel
			</span>
				numbers := []<span class="hljs-type">int</span>{<span class="hljs-number">10</span>, <span class="hljs-number">20</span>, <span class="hljs-number">30</span>, <span class="hljs-number">40</span>, <span class="hljs-number">50</span>}
				strMap := <span class="hljs-keyword">map</span>[<span class="hljs-type">string</span>]<span class="hljs-type">int</span>{<span class="hljs-string">"alpha"</span>: <span class="hljs-number">1</span>, <span class="hljs-string">"beta"</span>: <span class="hljs-number">2</span>}
				ch := <span class="hljs-title">make</span>(<span class="hljs-keyword">chan</span> <span class="hljs-type">string</span>, <span class="hljs-number">2</span>)
				<span class="hljs-keyword">defer</span> <span class="hljs-title">close</span>(ch) <span class="hljs-comment">// Defer statement
			</span>
			
				<span class="hljs-comment">// Control Flow: If-Else with Short Initialization
			</span>
				<span class="hljs-keyword">if</span> length := <span class="hljs-title">len</span>(numbers); length &gt; <span class="hljs-number">0</span> &amp;&amp; shortBool {
					fmt.<span class="hljs-title">Printf</span>(<span class="hljs-string">"Slice length: %d\n"</span>, length)
				} <span class="hljs-keyword">else</span> <span class="hljs-keyword">if</span> length == <span class="hljs-number">0</span> {
					fmt.<span class="hljs-title">Println</span>(<span class="hljs-string">"Empty slice"</span>)
				} <span class="hljs-keyword">else</span> {
					fmt.<span class="hljs-title">Println</span>(<span class="hljs-string">"Fallback condition"</span>)
				}
			
				<span class="hljs-comment">// For Loops (Standard, Range, Infinite)
			</span>
				<span class="hljs-keyword">for</span> i := <span class="hljs-number">0</span>; i &lt; <span class="hljs-title">len</span>(numbers); i++ {
					<span class="hljs-keyword">if</span> i == <span class="hljs-number">1</span> {
						<span class="hljs-keyword">continue</span>
					}
					<span class="hljs-keyword">if</span> i == <span class="hljs-number">3</span> {
						<span class="hljs-keyword">break</span>
					}
				}
			
				<span class="hljs-keyword">for</span> key, val := <span class="hljs-keyword">range</span> strMap {
					_ = fmt.<span class="hljs-title">Sprintf</span>(<span class="hljs-string">"Key: %s, Val: %d"</span>, key, val)
				}
			
				<span class="hljs-comment">// Switch Statement (Expression and Type Switch)
			</span>
				<span class="hljs-keyword">switch</span> hexVal {
				<span class="hljs-keyword">case</span> <span class="hljs-number">0xFF</span>:
					<span class="hljs-keyword">fallthrough</span>
				<span class="hljs-keyword">case</span> <span class="hljs-number">0xFE</span>:
					<span class="hljs-keyword">break</span>
				<span class="hljs-keyword">default</span>:
					fmt.<span class="hljs-title">Println</span>(<span class="hljs-string">"Unknown byte"</span>)
				}
			
				<span class="hljs-keyword">var</span> genericVar <span class="hljs-keyword">interface</span>{} = <span class="hljs-string">"Test String"</span>
				<span class="hljs-keyword">switch</span> v := genericVar.(<span class="hljs-keyword">type</span>) {
				<span class="hljs-keyword">case</span> <span class="hljs-type">int</span>:
					fmt.<span class="hljs-title">Printf</span>(<span class="hljs-string">"Integer: %d\n"</span>, v)
				<span class="hljs-keyword">case</span> <span class="hljs-type">string</span>:
					fmt.<span class="hljs-title">Printf</span>(<span class="hljs-string">"String: %s\n"</span>, v)
				<span class="hljs-keyword">default</span>:
					fmt.<span class="hljs-title">Println</span>(<span class="hljs-string">"Unknown type"</span>)
				}
			
				<span class="hljs-comment">// Concurrency: Goroutine, Channels, Select, Sync
			</span>
				<span class="hljs-keyword">var</span> wg sync.WaitGroup
				wg.<span class="hljs-title">Add</span>(<span class="hljs-number">1</span>)
			
				<span class="hljs-keyword">go</span> <span class="hljs-function"><span class="hljs-keyword">func</span><span class="hljs-params">(msg <span class="hljs-type">string</span>)</span></span> {
					<span class="hljs-keyword">defer</span> wg.<span class="hljs-title">Done</span>()
					ch &lt;- msg
				}(<span class="hljs-title">categoryFormat</span>(<span class="hljs-string">"Concurrency Test"</span>))
			
				<span class="hljs-keyword">select</span> {
				<span class="hljs-keyword">case</span> res := &lt;-ch:
					fmt.<span class="hljs-title">Println</span>(<span class="hljs-string">"Received:"</span>, res)
				<span class="hljs-keyword">case</span> &lt;-time.<span class="hljs-title">After</span>(<span class="hljs-number">100</span> * time.Millisecond):
					fmt.<span class="hljs-title">Println</span>(<span class="hljs-string">"Timeout"</span>)
				}
			
				wg.<span class="hljs-title">Wait</span>()
			
				<span class="hljs-comment">// Anonymous Function / Closure &amp; Error Handling
			</span>
				res, err := <span class="hljs-title">safeDivide</span>(<span class="hljs-number">10.0</span>, <span class="hljs-number">0.0</span>)
				<span class="hljs-keyword">if</span> err != <span class="hljs-literal">nil</span> {
					_ = fmt.<span class="hljs-title">Errorf</span>(<span class="hljs-string">"operation failed: %w"</span>, err)
				}
			
				<span class="hljs-comment">// Raw Strings, Verbatim, and Escapes
			</span>
				rawString := <span class="hljs-string">`Line 1
			Line 2 with "quotes" and \no escapes\`</span>
				interpretedString := <span class="hljs-string">"Line 1\nLine 2 with \"escapes\""</span>
			
				_, _ = res, rawString
				_, _ = b, r
			}
			
			<span class="hljs-comment">// Function with Multiple Return Values &amp; Named Parameters
			</span>
			<span class="hljs-function"><span class="hljs-keyword">func</span> <span class="hljs-title">safeDivide</span><span class="hljs-params">(a, b <span class="hljs-type">float64</span>)</span></span> (result <span class="hljs-type">float64</span>, err <span class="hljs-type">error</span>) {
				<span class="hljs-comment">// Panic &amp; Recover
			</span>
				<span class="hljs-keyword">defer</span> <span class="hljs-function"><span class="hljs-keyword">func</span><span class="hljs-params">()</span></span> {
					<span class="hljs-keyword">if</span> r := <span class="hljs-title">recover</span>(); r != <span class="hljs-literal">nil</span> {
						err = fmt.<span class="hljs-title">Errorf</span>(<span class="hljs-string">"recovered from panic: %v"</span>, r)
					}
				}()
			
				<span class="hljs-keyword">if</span> b == <span class="hljs-number">0</span> {
					<span class="hljs-title">panic</span>(<span class="hljs-string">"division by zero"</span>)
				}
				<span class="hljs-keyword">return</span> a / b, <span class="hljs-literal">nil</span>
			}
			
			<span class="hljs-comment">// Variadic Function
			</span>
			<span class="hljs-function"><span class="hljs-keyword">func</span> <span class="hljs-title">categoryFormat</span><span class="hljs-params">(format <span class="hljs-type">string</span>, args ...<span class="hljs-type">any</span>)</span></span> <span class="hljs-type">string</span> {
				<span class="hljs-keyword">return</span> fmt.<span class="hljs-title">Sprintf</span>(format, args...)
			}</code></pre>
			    </div>
			</article>
			
			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderRust()
	{
		const string value =
			"""
			```rust
			#![allow(dead_code, unused_variables)]

			//! Module-level documentation comment testing markdown rendering.

			use std::collections::HashMap;
			use std::fmt::{Display, Formatter, Result as FmtResult};
			use std::marker::PhantomData;
			use std::ops::Add;
			use std::sync::{Arc, Mutex};

			// Outer Attributes and Derive Macros
			#[derive(Debug, Clone, Copy, PartialEq, Eq, Hash)]
			#[repr(u8)]
			pub enum Status {
			    Idle = 0,
			    Active = 1,
			    Failed = 255,
			}

			// Traits with Associated Types and Const Generics
			pub trait Processor<T, const N: usize> {
			    type Output;
			    type Error: std::error::Error + 'static;

			    fn process(&self, input: [T; N]) -> Result<Self::Output, Self::Error>;
			}

			// Structs: Named, Tuple, and Unit
			#[derive(Debug)]
			pub struct Point<T> {
			    pub x: T,
			    pub y: T,
			}

			pub struct Wrapper<T>(pub T);
			pub struct Marker;

			// Lifetime Annotations and Structs
			pub struct Container<'a, T: 'a> 
			where 
			    T: Display + ?Sized, 
			{
			    name: &'a str,
			    item: &'a T,
			    _phantom: PhantomData<&'a ()>,
			}

			// Impl Block with Lifetimes and Trait Implementation
			impl<'a, T> Display for Container<'a, T>
			where
			    T: Display + ?Sized,
			{
			    fn fmt(&self, f: &mut Formatter<'_>) -> FmtResult {
			        write!(f, "Container[{}]: {}", self.name, self.item)
			    }
			}

			// Macro Definition
			#[macro_export]
			macro_rules! create_map {
			    ( $( $key:expr => $val:expr ),* $(,)? ) => {{
			        let mut map = HashMap::new();
			        $( map.insert($key, $val); )*
			        map
			    }};
			}

			// Async Function, Generics, Pattern Matching, Control Flow
			pub async fn execute_pipeline<'a, T>(
			    status: Status,
			    raw_data: Option<&'a str>,
			) -> Result<String, Box<dyn std::error::Error + Send + Sync>>
			where
			    T: Add<Output = T> + Default + Copy,
			{
			    // Local Variables, Type Inferences, and Mutability
			    let mut counter: u64 = 100_000;
			    let ref_counter = &mut counter;
			    *ref_counter += 1;

			    // Numbers: Hex, Binary, Octal, Floats, Suffixes
			    let hex_val = 0xFF_u8;
			    let bin_val = 0b1010_1010;
			    let oct_val = 0o77;
			    let float_val = 3.14159_f64;

			    // Pattern Matching & If-Let / While-Let
			    if let Some(text) = raw_data {
			        println!("Received input: {text}");
			    }

			    let description = match status {
			        Status::Idle => "System is standing by",
			        Status::Active => "Processing data",
			        Status::Failed => return Err("Critical system failure".into()),
			    };

			    // Loops, Labels, and Expressions
			    'outer: loop {
			        while counter > 0 {
			            counter -= 1;
			            if counter == 50 {
			                break 'outer;
			            }
			        }
			    }

			    // Macro invocation and Closure
			    let map = create_map! {
			        "key1" => 10,
			        "key2" => 20,
			    };

			    let closure = |x: i32| -> i32 { x * 2 };
			    let _closure_res = closure(5);

			    // Unsafe Block and Raw Pointers
			    let mut x: i32 = 42;
			    let raw_ptr: *mut i32 = &mut x;
			    unsafe {
			        *raw_ptr = 100;
			    }

			    // Strings: Raw Strings, Verbatim, Byte Strings
			    let raw_str = r#"Raw string with "quotes" and \no escape sequences\"#;
			    let byte_str: &[u8; 4] = b"RUST";

			    Ok(format!("{description} | Raw: {raw_str}"))
			}
			```
			""";

		const string expected =
			"""
			<article id:ignore class="mud-markdown-body">
			    <div class="hljs mud-markdown-code-highlight">
			        <button
			            blazor:onclick="2"
			            type="button"
			            class="mud-button-root mud-icon-button mud-button mud-button-filled mud-button-filled-primary mud-button-filled-size-medium mud-ripple ma-2 mud-markdown-code-highlight-copybtn"
			            blazor:onclick:stopPropagation
			            blazor:elementReference=""
			        >
			            <span class="mud-icon-button-label"
			                ><svg
			                    class="mud-icon-root mud-svg-icon mud-icon-size-medium"
			                    focusable="false"
			                    viewBox="0 0 24 24"
			                    aria-hidden="true"
			                    role="img"
			                >
			                    <g><rect fill="none" height="24" width="24" /></g>
			                    <g>
			                        <path
			                            d="M15,20H5V7c0-0.55-0.45-1-1-1h0C3.45,6,3,6.45,3,7v13c0,1.1,0.9,2,2,2h10c0.55,0,1-0.45,1-1v0C16,20.45,15.55,20,15,20z M20,16V4c0-1.1-0.9-2-2-2H9C7.9,2,7,2.9,7,4v12c0,1.1,0.9,2,2,2h9C19.1,18,20,17.1,20,16z M18,16H9V4h9V16z"
			                        />
			                    </g></svg
			            ></span>
			        </button>
			        <pre><code class="hljs language-rust"><span class="hljs-meta">#![allow(dead_code, unused_variables)]</span>

			<span class="hljs-comment">//! Module-level documentation comment testing markdown rendering.</span>

			<span class="hljs-keyword">use</span> std::collections::HashMap;
			<span class="hljs-keyword">use</span> std::fmt::{Display, Formatter, <span class="hljs-type">Result</span> <span class="hljs-keyword">as</span> <span class="hljs-type">FmtResult</span>};
			<span class="hljs-keyword">use</span> std::marker::PhantomData;
			<span class="hljs-keyword">use</span> std::ops::Add;
			<span class="hljs-keyword">use</span> std::sync::{Arc, Mutex};

			<span class="hljs-comment">// Outer Attributes and Derive Macros</span>
			<span class="hljs-meta">#[derive(<span class="hljs-type">Debug</span>, <span class="hljs-type">Clone</span>, <span class="hljs-type">Copy</span>, <span class="hljs-type">PartialEq</span>, <span class="hljs-type">Eq</span>, <span class="hljs-type">Hash</span>)]</span>
			<span class="hljs-meta">#[repr(<span class="hljs-type">u8</span>)]</span>
			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">enum</span> <span class="hljs-type">Status</span> {
			    Idle = <span class="hljs-number">0</span>,
			    Active = <span class="hljs-number">1</span>,
			    Failed = <span class="hljs-number">255</span>,
			}

			<span class="hljs-comment">// Traits with Associated Types and Const Generics</span>
			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">trait</span> <span class="hljs-type">Processor</span>&lt;<span class="hljs-type">T</span>, <span class="hljs-keyword">const</span> <span class="hljs-type">N</span>: <span class="hljs-type">usize</span>&gt; {
			    <span class="hljs-keyword">type</span> Output;
			    <span class="hljs-keyword">type</span> Error: std::error::Error + <span class="hljs-symbol">'static</span>;

			    <span class="hljs-function"><span class="hljs-keyword">fn</span> <span class="hljs-title">process</span><span class="hljs-params">(<span class="hljs-operator">&amp;</span><span class="hljs-keyword">self</span>, input: [T; N])</span></span> -&gt; <span class="hljs-type">Result</span>&lt;<span class="hljs-type">Self</span>::<span class="hljs-type">Output</span>, <span class="hljs-type">Self</span>::<span class="hljs-type">Error</span>&gt;;
			}

			<span class="hljs-comment">// Structs: Named, Tuple, and Unit</span>
			<span class="hljs-meta">#[derive(<span class="hljs-type">Debug</span>)]</span>
			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">struct</span> <span class="hljs-type">Point</span>&lt;<span class="hljs-type">T</span>&gt; {
			    <span class="hljs-keyword">pub</span> x: <span class="hljs-type">T</span>,
			    <span class="hljs-keyword">pub</span> y: <span class="hljs-type">T</span>,
			}

			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">struct</span> <span class="hljs-type">Wrapper</span>&lt;<span class="hljs-type">T</span>&gt;(<span class="hljs-keyword">pub</span> T);
			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">struct</span> <span class="hljs-type">Marker</span>;

			<span class="hljs-comment">// Lifetime Annotations and Structs</span>
			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">struct</span> <span class="hljs-type">Container</span>&lt;<span class="hljs-symbol">'a</span>, <span class="hljs-type">T</span>: <span class="hljs-symbol">'a</span>&gt; 
			<span class="hljs-keyword">where</span> 
			    T: <span class="hljs-type">Display</span> + ?Sized, 
			{
			    name: <span class="hljs-operator">&amp;</span><span class="hljs-symbol">'a</span> <span class="hljs-type">str</span>,
			    item: <span class="hljs-operator">&amp;</span><span class="hljs-symbol">'a</span> T,
			    _phantom: <span class="hljs-type">PhantomData</span>&lt;<span class="hljs-operator">&amp;</span><span class="hljs-symbol">'a</span> ()&gt;,
			}

			<span class="hljs-comment">// Impl Block with Lifetimes and Trait Implementation</span>
			<span class="hljs-keyword">impl</span>&lt;<span class="hljs-symbol">'a</span>, T&gt; <span class="hljs-type">Display</span> <span class="hljs-keyword">for</span> <span class="hljs-type">Container</span>&lt;<span class="hljs-symbol">'a</span>, <span class="hljs-type">T</span>&gt;
			<span class="hljs-keyword">where</span>
			    T: <span class="hljs-type">Display</span> + ?Sized,
			{
			    <span class="hljs-function"><span class="hljs-keyword">fn</span> <span class="hljs-title">fmt</span><span class="hljs-params">(<span class="hljs-operator">&amp;</span><span class="hljs-keyword">self</span>, f: <span class="hljs-operator">&amp;</span><span class="hljs-keyword">mut</span> <span class="hljs-type">Formatter</span>&lt;<span class="hljs-symbol">'_</span>&gt;)</span></span> -&gt; FmtResult {
			        <span class="hljs-title">write!</span>(f, <span class="hljs-string">"Container[{}]: {}"</span>, <span class="hljs-keyword">self</span>.name, <span class="hljs-keyword">self</span>.item)
			    }
			}

			<span class="hljs-comment">// Macro Definition</span>
			<span class="hljs-meta">#[macro_export]</span>
			<span class="hljs-title">macro_rules!</span> create_map {
			    ( $( $key:expr =&gt; $val:expr ),* $(,)? ) =&gt; {{
			        <span class="hljs-keyword">let</span> <span class="hljs-keyword">mut</span> map = <span class="hljs-type">HashMap</span>::<span class="hljs-title">new</span>();
			        $( map.<span class="hljs-title">insert</span>($key, $val); )*
			        map
			    }};
			}

			<span class="hljs-comment">// Async Function, Generics, Pattern Matching, Control Flow</span>
			<span class="hljs-keyword">pub</span> <span class="hljs-keyword">async</span> <span class="hljs-function"><span class="hljs-keyword">fn</span> <span class="hljs-title">execute_pipeline</span></span>&lt;<span class="hljs-symbol">'a</span>, T&gt;(
			    status: <span class="hljs-type">Status</span>,
			    raw_data: <span class="hljs-type">Option</span>&lt;<span class="hljs-operator">&amp;</span><span class="hljs-symbol">'a</span> <span class="hljs-type">str</span>&gt;,
			) -&gt; <span class="hljs-type">Result</span>&lt;<span class="hljs-type">String</span>, <span class="hljs-type">Box</span>&lt;<span class="hljs-keyword">dyn</span> std::error::<span class="hljs-type">Error</span> + <span class="hljs-type">Send</span> + <span class="hljs-type">Sync</span>&gt;&gt;
			<span class="hljs-keyword">where</span>
			    T: <span class="hljs-type">Add</span>&lt;<span class="hljs-type">Output</span> = <span class="hljs-type">T</span>&gt; + Default + Copy,
			{
			    <span class="hljs-comment">// Local Variables, Type Inferences, and Mutability</span>
			    <span class="hljs-keyword">let</span> <span class="hljs-keyword">mut</span> counter: <span class="hljs-type">u64</span> = <span class="hljs-number">100_000</span>;
			    <span class="hljs-keyword">let</span> ref_counter = <span class="hljs-operator">&amp;</span><span class="hljs-keyword">mut</span> counter;
			    *ref_counter += <span class="hljs-number">1</span>;

			    <span class="hljs-comment">// Numbers: Hex, Binary, Octal, Floats, Suffixes</span>
			    <span class="hljs-keyword">let</span> hex_val = <span class="hljs-number">0xFF_u</span><span class="hljs-number">8</span>;
			    <span class="hljs-keyword">let</span> bin_val = <span class="hljs-number">0b</span><span class="hljs-number">1010_1010</span>;
			    <span class="hljs-keyword">let</span> oct_val = <span class="hljs-number">0o</span><span class="hljs-number">77</span>;
			    <span class="hljs-keyword">let</span> float_val = <span class="hljs-number">3.14159_f</span><span class="hljs-number">64</span>;

			    <span class="hljs-comment">// Pattern Matching &amp; If-Let / While-Let</span>
			    <span class="hljs-keyword">if</span> <span class="hljs-keyword">let</span> <span class="hljs-literal">Some</span>(text) = raw_data {
			        <span class="hljs-title">println!</span>(<span class="hljs-string">"Received input: {text}"</span>);
			    }

			    <span class="hljs-keyword">let</span> description = <span class="hljs-keyword">match</span> status {
			        <span class="hljs-type">Status</span>::Idle =&gt; <span class="hljs-string">"System is standing by"</span>,
			        <span class="hljs-type">Status</span>::Active =&gt; <span class="hljs-string">"Processing data"</span>,
			        <span class="hljs-type">Status</span>::Failed =&gt; <span class="hljs-keyword">return</span> <span class="hljs-literal">Err</span>(<span class="hljs-string">"Critical system failure"</span>.<span class="hljs-title">into</span>()),
			    };

			    <span class="hljs-comment">// Loops, Labels, and Expressions</span>
			    <span class="hljs-symbol">'outer</span>: <span class="hljs-keyword">loop</span> {
			        <span class="hljs-keyword">while</span> counter &gt; <span class="hljs-number">0</span> {
			            counter -= <span class="hljs-number">1</span>;
			            <span class="hljs-keyword">if</span> counter == <span class="hljs-number">50</span> {
			                <span class="hljs-keyword">break</span> <span class="hljs-symbol">'outer</span>;
			            }
			        }
			    }

			    <span class="hljs-comment">// Macro invocation and Closure</span>
			    <span class="hljs-keyword">let</span> map = <span class="hljs-title">create_map!</span> {
			        <span class="hljs-string">"key1"</span> =&gt; <span class="hljs-number">10</span>,
			        <span class="hljs-string">"key2"</span> =&gt; <span class="hljs-number">20</span>,
			    };

			    <span class="hljs-keyword">let</span> closure = |x: <span class="hljs-type">i32</span>| -&gt; <span class="hljs-type">i32</span> { x * <span class="hljs-number">2</span> };
			    <span class="hljs-keyword">let</span> _closure_res = <span class="hljs-title">closure</span>(<span class="hljs-number">5</span>);

			    <span class="hljs-comment">// Unsafe Block and Raw Pointers</span>
			    <span class="hljs-keyword">let</span> <span class="hljs-keyword">mut</span> x: <span class="hljs-type">i32</span> = <span class="hljs-number">42</span>;
			    <span class="hljs-keyword">let</span> raw_ptr: *<span class="hljs-keyword">mut</span> <span class="hljs-type">i32</span> = <span class="hljs-operator">&amp;</span><span class="hljs-keyword">mut</span> x;
			    <span class="hljs-keyword">unsafe</span> {
			        *raw_ptr = <span class="hljs-number">100</span>;
			    }

			    <span class="hljs-comment">// Strings: Raw Strings, Verbatim, Byte Strings</span>
			    <span class="hljs-keyword">let</span> raw_str = <span class="hljs-string">r#"Raw string with "quotes" and \no escape sequences\"#</span>;
			    <span class="hljs-keyword">let</span> byte_str: <span class="hljs-operator">&amp;</span>[<span class="hljs-type">u8</span>; <span class="hljs-number">4</span>] = <span class="hljs-string">b"RUST"</span>;

			    <span class="hljs-literal">Ok</span>(<span class="hljs-title">format!</span>(<span class="hljs-string">"{description} | Raw: {raw_str}"</span>))
			}</code></pre>
			    </div>
			</article>

			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderJson()
	{
		const string value =
			"""
			```json
			{
			  "name": "mud-blazor",
			  "version": 2,
			  "enabled": true,
			  "tags": ["md", "blazor"],
			  "nested": { "count": 10, "ratio": 3.14, "empty": null }
			}
			```
			""";

		const string expected =
			"""

			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderXml()
	{
		const string value =
			"""
			```xml
			<?xml version="1.0" encoding="UTF-8"?>
			<!-- Application configuration -->
			<config env="prod">
			  <name>MudBlazor</name>
			  <value count="3">text &amp; more</value>
			  <self-closing attr="x" />
			</config>
			```
			""";

		const string expected =
			"""

			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}

	[Fact]
	public void RenderYaml()
	{
		const string value =
			"""
			```yaml
			# Application configuration
			name: mud-blazor
			version: 2
			enabled: true
			ratio: 3.14
			description: "quoted value"
			tags:
			  - md
			  - blazor
			nested:
			  count: 10
			  empty: null
			anchor: &a value
			ref: *a
			```
			""";

		const string expected =
			"""

			""";

		using var fixture = CreateFixture(value);
		fixture.MarkupMatches(expected);
	}
}
