namespace MudBlazor;

/// <summary>
/// Renders TeX math as HTML elements which are laid out with CSS (<c>.mud-math</c> in MudBlazor.Markdown.css)
/// </summary>
internal sealed class MudMathJax : ComponentBase
{
	private enum CommandType
	{
		Identifier,
		UprightIdentifier,
		Operator,
		LargeOperator,
		NamedOperator,
		Integral,
		Function,
		Space,
		Accent,
		Overline,
		Underline,
		Overbrace,
		Underbrace,
		Font,
		Fraction,
		Binomial,
		Root,
		Left,
		Text,
	}

	private readonly record struct Command(CommandType Type, string Value = "");

	private const string Right = "\\right";

	/// <summary>
	/// Binary operators and relations which have spaces around them
	/// </summary>
	private const string SpacedOperators = "=<>+−±∓×÷⋅∗⋆∘∙∩∪∖∧∨⊕⊗≤≥≠≪≫≈∼≃≅≡∝∈∉∋⊂⊃⊆⊇⊥∥∣→←↔⇒⇐⇔⟹⟺↦:";

	private static readonly FrozenDictionary<string, Command> Commands = CreateCommands();

#if NET9_0_OR_GREATER
	private static readonly FrozenDictionary<string, Command>.AlternateLookup<ReadOnlySpan<char>> CommandsLookup = Commands.GetAlternateLookup<ReadOnlySpan<char>>();
#endif

	private string _text = string.Empty;
	private int _i, _end;
	private bool _isDisplay;

	[Parameter]
	public string Delimiter { get; set; } = string.Empty;

	[Parameter]
	public StringSlice Value { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		_isDisplay = Delimiter.Length > 1;
		_text = Value.Text ?? string.Empty;
		_i = Value.Start;
		_end = Value.Text == null ? 0 : Value.End + 1;

		var elementIndex = 0;
		OpenSpan(builder, ref elementIndex, _isDisplay ? "mud-math mud-math-block" : "mud-math");
		builder.AddAttribute(elementIndex++, "role", "math");
		RenderRow(builder, ref elementIndex, stop: null);
		builder.CloseElement();
	}

	#region Rendering

	/// <summary>
	/// Renders elements until the end, the <paramref name="stop"/> character or <c>\right</c>
	/// </summary>
	private void RenderRow(RenderTreeBuilder builder, ref int elementIndex, char? stop)
	{
		while (true)
		{
			SkipSpaces();
			if (_i >= _end)
				return;

			if (_text[_i] == stop)
			{
				_i++;
				return;
			}

			if (IsAt(Right))
				return;

			RenderScripts(builder, ref elementIndex);
		}
	}

	private void RenderScripts(RenderTreeBuilder builder, ref int elementIndex)
	{
		// the scripts wrap the base, so their positions are found before rendering
		var baseStart = _i;
		var hasLimits = SkipAtom() && _isDisplay;
		int subStart = -1, supStart = -1, primes = 0;
		while (true)
		{
			SkipSpaces();
			if (_i >= _end)
				break;

			var c = _text[_i];
			if (c == '_' && subStart < 0)
			{
				subStart = ++_i;
				SkipArgument();
			}
			else if (c == '^' && supStart < 0 && primes == 0)
			{
				supStart = ++_i;
				SkipArgument();
			}
			else if (c == '\'' && supStart < 0 && primes == 0)
			{
				for (; _i < _end && _text[_i] == '\''; _i++)
					primes++;
			}
			else
			{
				break;
			}
		}

		var end = _i;
		_i = baseStart;

		if (subStart < 0 && supStart < 0 && primes == 0)
		{
			RenderAtom(builder, ref elementIndex);
			return;
		}

		if (hasLimits)
		{
			// limits above and below the operator in the display mode
			OpenSpan(builder, ref elementIndex, "mud-math-limits");
			RenderScript(builder, ref elementIndex, supStart, primes, "mud-math-over");
			_i = baseStart;
			RenderAtom(builder, ref elementIndex);
			RenderScript(builder, ref elementIndex, subStart, 0, "mud-math-under");
			builder.CloseElement();
		}
		else
		{
			OpenSpan(builder, ref elementIndex, "mud-math-scripts");
			RenderAtom(builder, ref elementIndex);

			var hasSup = supStart >= 0 || primes != 0;
			if (hasSup && subStart >= 0)
			{
				OpenSpan(builder, ref elementIndex, "mud-math-subsup");
				RenderScript(builder, ref elementIndex, supStart, primes, "mud-math-sup");
				RenderScript(builder, ref elementIndex, subStart, 0, "mud-math-sub");
				builder.CloseElement();
			}
			else if (hasSup)
			{
				RenderScript(builder, ref elementIndex, supStart, primes, "mud-math-sup");
			}
			else
			{
				RenderScript(builder, ref elementIndex, subStart, 0, "mud-math-sub");
			}

			builder.CloseElement();
		}

		_i = end;
	}

	private void RenderScript(RenderTreeBuilder builder, ref int elementIndex, int start, int primes, string cssClass)
	{
		if (start >= 0)
		{
			_i = start;
			RenderArgument(builder, ref elementIndex, cssClass);
		}
		else if (primes != 0)
		{
			RenderText(builder, ref elementIndex, cssClass, new string('′', primes));
		}
	}

	/// <returns><see langword="true"/> if the atom is an operator with limits</returns>
	private bool RenderAtom(RenderTreeBuilder builder, ref int elementIndex)
	{
		var c = _text[_i];
		switch (c)
		{
			case '{':
				_i++;
				RenderRow(builder, ref elementIndex, '}');
				return false;
			case '\\':
				return RenderCommand(builder, ref elementIndex);
			case '^' or '_':
				// a script without the base
				return false;
		}

		if (IsNumberStart(c))
		{
			var start = _i;
			SkipNumber();
			RenderText(builder, ref elementIndex, "mud-math-mn", _text[start.._i]);
			return false;
		}

		_i++;
		if (char.IsLetter(c))
		{
			RenderText(builder, ref elementIndex, "mud-math-mi", c.ToString());
			return false;
		}

		switch (c)
		{
			case '-':
				RenderOperator(builder, ref elementIndex, "−");
				break;
			case '*':
				RenderOperator(builder, ref elementIndex, "∗");
				break;
			case '~':
				RenderSpace(builder, ref elementIndex, "margin-left:0.25em");
				break;
			case ',' or ';':
				RenderText(builder, ref elementIndex, "mud-math-punct", c.ToString());
				break;
			default:
				RenderOperator(builder, ref elementIndex, c.ToString());
				break;
		}

		return false;
	}

	private bool RenderCommand(RenderTreeBuilder builder, ref int elementIndex)
	{
		var name = ReadCommandName();
		if (!TryGetCommand(name, out var command))
		{
			// unsupported commands are displayed as text like MathJax does
			RenderText(builder, ref elementIndex, "mud-math-error", "\\" + name.ToString());
			return false;
		}

		switch (command.Type)
		{
			case CommandType.Identifier:
				RenderText(builder, ref elementIndex, "mud-math-mi", command.Value);
				break;
			case CommandType.UprightIdentifier:
				RenderText(builder, ref elementIndex, "mud-math-mn", command.Value);
				break;
			case CommandType.Operator:
				RenderOperator(builder, ref elementIndex, command.Value);
				break;
			case CommandType.LargeOperator:
				RenderText(builder, ref elementIndex, "mud-math-largeop", command.Value);
				return true;
			case CommandType.NamedOperator:
				RenderText(builder, ref elementIndex, "mud-math-fn", command.Value);
				return true;
			case CommandType.Integral:
				RenderText(builder, ref elementIndex, "mud-math-largeop mud-math-integral", command.Value);
				break;
			case CommandType.Function:
				RenderText(builder, ref elementIndex, "mud-math-fn", command.Value);
				break;
			case CommandType.Space:
				RenderSpace(builder, ref elementIndex, command.Value);
				break;
			case CommandType.Accent:
				OpenSpan(builder, ref elementIndex, "mud-math-accent");
				RenderText(builder, ref elementIndex, "mud-math-accent-mark", command.Value);
				RenderArgument(builder, ref elementIndex, "mud-math-accent-base");
				builder.CloseElement();
				break;
			case CommandType.Overline:
				RenderArgument(builder, ref elementIndex, "mud-math-overline");
				break;
			case CommandType.Underline:
				RenderArgument(builder, ref elementIndex, "mud-math-underline");
				break;
			case CommandType.Overbrace:
				OpenSpan(builder, ref elementIndex, "mud-math-limits");
				RenderText(builder, ref elementIndex, "mud-math-brace", "⏞");
				RenderArgument(builder, ref elementIndex, "mud-math-row");
				builder.CloseElement();
				return true;
			case CommandType.Underbrace:
				OpenSpan(builder, ref elementIndex, "mud-math-limits");
				RenderArgument(builder, ref elementIndex, "mud-math-row");
				RenderText(builder, ref elementIndex, "mud-math-brace", "⏟");
				builder.CloseElement();
				return true;
			case CommandType.Font:
				RenderFont(builder, ref elementIndex, command.Value, ReadGroupText());
				break;
			case CommandType.Fraction:
			case CommandType.Binomial:
			{
				var isBinomial = command.Type == CommandType.Binomial;
				if (isBinomial)
				{
					OpenSpan(builder, ref elementIndex, "mud-math-fenced");
					RenderText(builder, ref elementIndex, "mud-math-fence", "(");
				}

				OpenSpan(builder, ref elementIndex, isBinomial ? "mud-math-frac mud-math-binom" : "mud-math-frac");
				RenderArgument(builder, ref elementIndex, "mud-math-num");
				RenderArgument(builder, ref elementIndex, "mud-math-den");
				builder.CloseElement();

				if (isBinomial)
				{
					RenderText(builder, ref elementIndex, "mud-math-fence", ")");
					builder.CloseElement();
				}

				break;
			}
			case CommandType.Root:
			{
				OpenSpan(builder, ref elementIndex, "mud-math-sqrt");
				SkipSpaces();
				if (_i < _end && _text[_i] == '[')
				{
					_i++;
					OpenSpan(builder, ref elementIndex, "mud-math-index");
					RenderRow(builder, ref elementIndex, ']');
					builder.CloseElement();
				}

				RenderText(builder, ref elementIndex, "mud-math-surd", "√");
				RenderArgument(builder, ref elementIndex, "mud-math-radicand");
				builder.CloseElement();
				break;
			}
			case CommandType.Left:
			{
				OpenSpan(builder, ref elementIndex, "mud-math-fenced");
				RenderFence(builder, ref elementIndex);
				OpenSpan(builder, ref elementIndex, "mud-math-row");
				RenderRow(builder, ref elementIndex, stop: null);
				builder.CloseElement();
				if (IsAt(Right))
				{
					_i += Right.Length;
					RenderFence(builder, ref elementIndex);
				}

				builder.CloseElement();
				break;
			}
			case CommandType.Text:
				RenderText(builder, ref elementIndex, "mud-math-text", ReadGroupText().ToString());
				break;
		}

		return false;
	}

	/// <summary>
	/// Renders a braced group or a single token
	/// </summary>
	private void RenderArgument(RenderTreeBuilder builder, ref int elementIndex, string cssClass)
	{
		OpenSpan(builder, ref elementIndex, cssClass);
		SkipSpaces();
		if (_i < _end)
		{
			if (_text[_i] == '{')
			{
				_i++;
				RenderRow(builder, ref elementIndex, '}');
			}
			else if (_text[_i] == '\\')
			{
				RenderAtom(builder, ref elementIndex);
			}
			else
			{
				// a single character, e.g. x^2 or \frac12
				var c = _text[_i++];
				RenderText(builder, ref elementIndex, char.IsLetter(c) ? "mud-math-mi" : "mud-math-mn", c.ToString());
			}
		}

		builder.CloseElement();
	}

	private void RenderFence(RenderTreeBuilder builder, ref int elementIndex)
	{
		var delimiter = ReadDelimiter();
		if (delimiter != null)
			RenderText(builder, ref elementIndex, "mud-math-fence", delimiter);
	}

	private static void RenderFont(RenderTreeBuilder builder, ref int elementIndex, string variant, ReadOnlySpan<char> text)
	{
		var offsets = variant switch
		{
			"double-struck" => (Upper: 0x1D538, Lower: 0x1D552),
			"script" => (Upper: 0x1D49C, Lower: 0x1D4B6),
			"fraktur" => (Upper: 0x1D504, Lower: 0x1D51E),
			_ => (Upper: 0, Lower: 0),
		};

		if (offsets.Upper == 0)
		{
			RenderText(builder, ref elementIndex, "mud-math-" + variant, text.ToString());
			return;
		}

		// these alphabets exist only as Unicode characters
		var value = new System.Text.StringBuilder(text.Length * 2);
		foreach (var c in text)
		{
			var letter = GetLetterlikeSymbol(variant, c);
			if (letter != null)
				value.Append(letter);
			else if (char.IsAsciiLetterUpper(c))
				value.Append(char.ConvertFromUtf32(offsets.Upper + c - 'A'));
			else if (char.IsAsciiLetterLower(c))
				value.Append(char.ConvertFromUtf32(offsets.Lower + c - 'a'));
			else
				value.Append(c);
		}

		RenderText(builder, ref elementIndex, "mud-math-mn", value.ToString());
	}

	private static string? GetLetterlikeSymbol(string variant, char c) =>
		(variant, c) switch
		{
			("double-struck", 'C') => "ℂ", ("double-struck", 'H') => "ℍ", ("double-struck", 'N') => "ℕ", ("double-struck", 'P') => "ℙ",
			("double-struck", 'Q') => "ℚ", ("double-struck", 'R') => "ℝ", ("double-struck", 'Z') => "ℤ",
			("script", 'B') => "ℬ", ("script", 'E') => "ℰ", ("script", 'F') => "ℱ", ("script", 'H') => "ℋ", ("script", 'I') => "ℐ",
			("script", 'L') => "ℒ", ("script", 'M') => "ℳ", ("script", 'R') => "ℛ", ("script", 'e') => "ℯ", ("script", 'g') => "ℊ",
			("script", 'o') => "ℴ",
			("fraktur", 'C') => "ℭ", ("fraktur", 'H') => "ℌ", ("fraktur", 'I') => "ℑ", ("fraktur", 'R') => "ℜ", ("fraktur", 'Z') => "ℨ",
			_ => null,
		};

	private static void RenderOperator(RenderTreeBuilder builder, ref int elementIndex, string value)
	{
		var cssClass = value.Length == 1 && SpacedOperators.Contains(value[0]) ? "mud-math-op" : "mud-math-mo";
		RenderText(builder, ref elementIndex, cssClass, value);
	}

	private static void RenderSpace(RenderTreeBuilder builder, ref int elementIndex, string style)
	{
		OpenSpan(builder, ref elementIndex, "mud-math-space");
		builder.AddAttribute(elementIndex++, AttributeNames.Style, style);
		builder.CloseElement();
	}

	private static void RenderText(RenderTreeBuilder builder, ref int elementIndex, string cssClass, string value)
	{
		OpenSpan(builder, ref elementIndex, cssClass);
		builder.AddContent(elementIndex++, value);
		builder.CloseElement();
	}

	private static void OpenSpan(RenderTreeBuilder builder, ref int elementIndex, string cssClass)
	{
		builder.OpenElement(elementIndex++, ElementNames.Span);
		builder.AddAttribute(elementIndex++, AttributeNames.Class, cssClass);
	}

	#endregion

	#region Skipping

	private void SkipRow(char? stop)
	{
		while (true)
		{
			SkipSpaces();
			if (_i >= _end)
				return;

			if (_text[_i] == stop)
			{
				_i++;
				return;
			}

			if (IsAt(Right))
				return;

			SkipAtom();
			while (true)
			{
				SkipSpaces();
				if (_i >= _end || _text[_i] is not ('_' or '^' or '\''))
					break;

				if (_text[_i++] != '\'')
					SkipArgument();
			}
		}
	}

	/// <returns><see langword="true"/> if the atom is an operator with limits</returns>
	private bool SkipAtom()
	{
		var c = _text[_i];
		switch (c)
		{
			case '{':
				_i++;
				SkipRow('}');
				return false;
			case '\\':
				return SkipCommand();
			case '^' or '_':
				return false;
		}

		if (IsNumberStart(c))
			SkipNumber();
		else
			_i++;

		return false;
	}

	private bool SkipCommand()
	{
		if (!TryGetCommand(ReadCommandName(), out var command))
			return false;

		switch (command.Type)
		{
			case CommandType.LargeOperator or CommandType.NamedOperator:
				return true;
			case CommandType.Overbrace or CommandType.Underbrace:
				SkipArgument();
				return true;
			case CommandType.Accent or CommandType.Overline or CommandType.Underline:
				SkipArgument();
				break;
			case CommandType.Font or CommandType.Text:
				ReadGroupText();
				break;
			case CommandType.Fraction or CommandType.Binomial:
				SkipArgument();
				SkipArgument();
				break;
			case CommandType.Root:
			{
				SkipSpaces();
				if (_i < _end && _text[_i] == '[')
				{
					_i++;
					SkipRow(']');
				}

				SkipArgument();
				break;
			}
			case CommandType.Left:
			{
				ReadDelimiter();
				SkipRow(stop: null);
				if (IsAt(Right))
				{
					_i += Right.Length;
					ReadDelimiter();
				}

				break;
			}
		}

		return false;
	}

	private void SkipArgument()
	{
		SkipSpaces();
		if (_i >= _end)
			return;

		if (_text[_i] == '{')
		{
			_i++;
			SkipRow('}');
		}
		else if (_text[_i] == '\\')
		{
			SkipAtom();
		}
		else
		{
			_i++;
		}
	}

	private void SkipNumber()
	{
		_i++;
		while (_i < _end && (char.IsAsciiDigit(_text[_i]) || (_text[_i] == '.' && _i + 1 < _end && char.IsAsciiDigit(_text[_i + 1]))))
			_i++;
	}

	private void SkipSpaces()
	{
		while (_i < _end && char.IsWhiteSpace(_text[_i]))
			_i++;
	}

	#endregion

	#region Reading

	private ReadOnlySpan<char> ReadCommandName()
	{
		_i++;
		if (_i >= _end)
			return [];

		var start = _i;
		if (!char.IsAsciiLetter(_text[_i]))
			return _text.AsSpan(_i++, 1);

		while (_i < _end && char.IsAsciiLetter(_text[_i]))
			_i++;

		return _text.AsSpan(start, _i - start);
	}

	/// <summary>
	/// Reads the raw content of <c>{...}</c> for <c>\text</c> and the font commands
	/// </summary>
	private ReadOnlySpan<char> ReadGroupText()
	{
		SkipSpaces();
		if (_i >= _end)
			return [];

		if (_text[_i] != '{')
			return _text.AsSpan(_i++, 1);

		var start = ++_i;
		for (var depth = 1; _i < _end; _i++)
		{
			if (_text[_i] == '{')
				depth++;
			else if (_text[_i] == '}' && --depth == 0)
				break;
		}

		var text = _text.AsSpan(start, _i - start);
		if (_i < _end)
			_i++;

		return text;
	}

	/// <returns>Delimiter after <c>\left</c> or <c>\right</c>, <see langword="null"/> for <c>.</c></returns>
	private string? ReadDelimiter()
	{
		SkipSpaces();
		if (_i >= _end)
			return null;

		if (_text[_i] == '\\')
			return TryGetCommand(ReadCommandName(), out var command) && command.Type == CommandType.Operator ? command.Value : null;

		var c = _text[_i++];
		return c == '.' ? null : c.ToString();
	}

	private bool IsAt(string value) =>
		_end - _i >= value.Length && _text.AsSpan(_i, value.Length).SequenceEqual(value);

	private bool IsNumberStart(char c) =>
		char.IsAsciiDigit(c) || (c == '.' && _i + 1 < _end && char.IsAsciiDigit(_text[_i + 1]));

	private static bool TryGetCommand(ReadOnlySpan<char> name, out Command command)
	{
#if NET9_0_OR_GREATER
		return CommandsLookup.TryGetValue(name, out command);
#else
		return Commands.TryGetValue(name.ToString(), out command);
#endif
	}

	#endregion

	private static FrozenDictionary<string, Command> CreateCommands()
	{
		var commands = new Dictionary<string, Command>(StringComparer.Ordinal);

		void Add(CommandType type, params (string Name, string Value)[] values)
		{
			foreach (var (name, value) in values)
				commands[name] = new Command(type, value);
		}

		Add(CommandType.Identifier,
			("alpha", "α"), ("beta", "β"), ("gamma", "γ"), ("delta", "δ"), ("epsilon", "ϵ"), ("varepsilon", "ε"), ("zeta", "ζ"),
			("eta", "η"), ("theta", "θ"), ("vartheta", "ϑ"), ("iota", "ι"), ("kappa", "κ"), ("lambda", "λ"), ("mu", "μ"), ("nu", "ν"),
			("xi", "ξ"), ("pi", "π"), ("varpi", "ϖ"), ("rho", "ρ"), ("varrho", "ϱ"), ("sigma", "σ"), ("varsigma", "ς"), ("tau", "τ"),
			("upsilon", "υ"), ("phi", "ϕ"), ("varphi", "φ"), ("chi", "χ"), ("psi", "ψ"), ("omega", "ω"), ("ell", "ℓ"), ("hbar", "ℏ"),
			("imath", "ı"), ("jmath", "ȷ"), ("partial", "∂"));

		Add(CommandType.UprightIdentifier,
			("Gamma", "Γ"), ("Delta", "Δ"), ("Theta", "Θ"), ("Lambda", "Λ"), ("Xi", "Ξ"), ("Pi", "Π"), ("Sigma", "Σ"), ("Upsilon", "Υ"),
			("Phi", "Φ"), ("Psi", "Ψ"), ("Omega", "Ω"), ("infty", "∞"), ("emptyset", "∅"), ("nabla", "∇"), ("aleph", "ℵ"), ("Re", "ℜ"),
			("Im", "ℑ"), ("forall", "∀"), ("exists", "∃"), ("neg", "¬"));

		Add(CommandType.Operator,
			("pm", "±"), ("mp", "∓"), ("times", "×"), ("div", "÷"), ("cdot", "⋅"), ("ast", "∗"), ("star", "⋆"), ("circ", "∘"),
			("bullet", "∙"), ("cap", "∩"), ("cup", "∪"), ("setminus", "∖"), ("wedge", "∧"), ("land", "∧"), ("vee", "∨"), ("lor", "∨"),
			("oplus", "⊕"), ("otimes", "⊗"), ("leq", "≤"), ("le", "≤"), ("geq", "≥"), ("ge", "≥"), ("neq", "≠"), ("ne", "≠"),
			("ll", "≪"), ("gg", "≫"), ("approx", "≈"), ("sim", "∼"), ("simeq", "≃"), ("cong", "≅"), ("equiv", "≡"), ("propto", "∝"),
			("in", "∈"), ("notin", "∉"), ("ni", "∋"), ("subset", "⊂"), ("supset", "⊃"), ("subseteq", "⊆"), ("supseteq", "⊇"),
			("perp", "⊥"), ("parallel", "∥"), ("mid", "∣"), ("to", "→"), ("rightarrow", "→"), ("leftarrow", "←"), ("gets", "←"),
			("leftrightarrow", "↔"), ("Rightarrow", "⇒"), ("Leftarrow", "⇐"), ("Leftrightarrow", "⇔"), ("implies", "⟹"), ("iff", "⟺"),
			("mapsto", "↦"), ("uparrow", "↑"), ("downarrow", "↓"), ("ldots", "…"), ("dots", "…"), ("cdots", "⋯"), ("vdots", "⋮"),
			("ddots", "⋱"), ("prime", "′"), ("colon", ":"), ("langle", "⟨"), ("rangle", "⟩"), ("lfloor", "⌊"), ("rfloor", "⌋"),
			("lceil", "⌈"), ("rceil", "⌉"), ("vert", "|"), ("Vert", "‖"), ("|", "‖"), ("{", "{"), ("}", "}"), ("lbrace", "{"),
			("rbrace", "}"), ("backslash", "\\"), ("%", "%"), ("$", "$"), ("&", "&"), ("#", "#"), ("_", "_"));

		Add(CommandType.LargeOperator,
			("sum", "∑"), ("prod", "∏"), ("coprod", "∐"), ("bigcup", "⋃"), ("bigcap", "⋂"), ("bigoplus", "⨁"), ("bigotimes", "⨂"));

		Add(CommandType.NamedOperator,
			("lim", "lim"), ("max", "max"), ("min", "min"), ("sup", "sup"), ("inf", "inf"), ("det", "det"), ("gcd", "gcd"));

		Add(CommandType.Integral, ("int", "∫"), ("iint", "∬"), ("iiint", "∭"), ("oint", "∮"));

		Add(CommandType.Function,
			("sin", "sin"), ("cos", "cos"), ("tan", "tan"), ("cot", "cot"), ("sec", "sec"), ("csc", "csc"), ("arcsin", "arcsin"),
			("arccos", "arccos"), ("arctan", "arctan"), ("sinh", "sinh"), ("cosh", "cosh"), ("tanh", "tanh"), ("coth", "coth"),
			("log", "log"), ("ln", "ln"), ("lg", "lg"), ("exp", "exp"), ("arg", "arg"), ("deg", "deg"), ("dim", "dim"), ("hom", "hom"),
			("ker", "ker"), ("Pr", "Pr"));

		Add(CommandType.Space,
			(",", "margin-left:0.167em"), (":", "margin-left:0.222em"), (">", "margin-left:0.222em"), (";", "margin-left:0.278em"),
			("!", "margin-left:-0.167em"), (" ", "margin-left:0.25em"), ("quad", "margin-left:1em"), ("qquad", "margin-left:2em"));

		Add(CommandType.Accent,
			("hat", "^"), ("widehat", "^"), ("bar", "¯"), ("vec", "→"), ("overrightarrow", "→"), ("dot", "˙"), ("ddot", "¨"),
			("tilde", "~"), ("widetilde", "~"), ("acute", "´"), ("grave", "`"), ("check", "ˇ"), ("breve", "˘"));

		Add(CommandType.Font,
			("mathrm", "upright"), ("rm", "upright"), ("operatorname", "upright"), ("mathbf", "bold"), ("bf", "bold"),
			("boldsymbol", "bold-italic"), ("mathit", "italic"), ("mathsf", "sans-serif"), ("mathtt", "monospace"),
			("mathbb", "double-struck"), ("mathcal", "script"), ("mathscr", "script"), ("mathfrak", "fraktur"));

		Add(CommandType.Overline, ("overline", ""));
		Add(CommandType.Underline, ("underline", ""));
		Add(CommandType.Overbrace, ("overbrace", ""));
		Add(CommandType.Underbrace, ("underbrace", ""));
		Add(CommandType.Fraction, ("frac", ""), ("dfrac", ""), ("tfrac", ""), ("cfrac", ""));
		Add(CommandType.Binomial, ("binom", ""), ("dbinom", ""), ("tbinom", ""));
		Add(CommandType.Root, ("sqrt", ""));
		Add(CommandType.Left, ("left", ""));
		Add(CommandType.Text, ("text", ""), ("textrm", ""), ("mbox", ""), ("textbf", ""), ("textit", ""));

		return commands.ToFrozenDictionary(StringComparer.Ordinal);
	}
}
