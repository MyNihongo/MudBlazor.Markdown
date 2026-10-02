using System.Text;

namespace MudBlazor;

/// <summary>
/// Converts a subset of TeX into MathML which is rendered by browsers natively
/// </summary>
internal sealed class TexToMathMl
{
	private enum CommandType
	{
		Identifier,
		UprightIdentifier,
		Operator,
		LargeOperator,
		Integral,
		Function,
		Space,
		Accent,
		Font,
		Fraction,
		Binomial,
		Root,
		Left,
		Text,
		Underline,
		Underbrace,
	}

	private readonly record struct Command(CommandType Type, string Value = "", bool HasLimits = false);

	private const string MathNamespace = "http://www.w3.org/1998/Math/MathML";

	private static readonly FrozenDictionary<string, Command> Commands = CreateCommands();

#if NET9_0_OR_GREATER
	private static readonly FrozenDictionary<string, Command>.AlternateLookup<ReadOnlySpan<char>> CommandsLookup = Commands.GetAlternateLookup<ReadOnlySpan<char>>();
#endif

	[ThreadStatic]
	private static StringBuilder? _cachedStringBuilder;

	private readonly StringBuilder _sb;
	private readonly string _text;
	private readonly int _end;
	private readonly bool _display;
	private int _i;

	private TexToMathMl(StringBuilder sb, in StringSlice value, bool display)
	{
		_sb = sb;
		_text = value.Text ?? string.Empty;
		_i = value.Start;
		_end = value.Text == null ? 0 : value.End + 1;
		_display = display;
	}

	/// <summary>
	/// Renders the <c>math</c> element for the TeX expression without delimiters
	/// </summary>
	public static void Render(RenderTreeBuilder builder, int sequence, in StringSlice value, bool display)
	{
		var sb = _cachedStringBuilder ??= new StringBuilder(256);
		sb.Clear();

		sb.Append("<math xmlns=\"").Append(MathNamespace).Append('"');
		if (display)
			sb.Append(" display=\"block\"");

		sb.Append('>');
		new TexToMathMl(sb, value, display).WriteRow(stop: null);
		sb.Append("</math>");

		builder.AddMarkupContent(sequence, sb.ToString());
	}

	/// <summary>
	/// Writes elements until the end, the <paramref name="stop"/> character or <c>\right</c>
	/// </summary>
	private void WriteRow(char? stop)
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

			if (IsAt("\\right"))
				return;

			WriteScripts();
		}
	}

	private void WriteScripts()
	{
		var baseStart = _sb.Length;
		var hasLimits = WriteAtom() && _display;
		var baseLength = _sb.Length - baseStart;

		bool hasSub = false, hasSup = false;
		int subStart = -1, supStart = -1, subLength = 0;
		while (true)
		{
			SkipSpaces();
			if (_i >= _end)
				break;

			var c = _text[_i];
			if (c == '_' && !hasSub)
			{
				_i++;
				hasSub = true;
				subStart = _sb.Length;
				WriteArgumentRow();
				subLength = _sb.Length - subStart;
			}
			else if (c == '^' && !hasSup)
			{
				_i++;
				hasSup = true;
				supStart = _sb.Length;
				WriteArgumentRow();
			}
			else if (c == '\'' && !hasSup)
			{
				hasSup = true;
				supStart = _sb.Length;
				_sb.Append("<mo>");
				while (_i < _end && _text[_i] == '\'')
				{
					_sb.Append('′');
					_i++;
				}

				_sb.Append("</mo>");
			}
			else
			{
				break;
			}
		}

		if (!hasSub && !hasSup)
			return;

		// MathML requires the order: base, subscript, superscript
		if (hasSub && hasSup && supStart < subStart)
		{
			var sub = _sb.ToString(subStart, subLength);
			_sb.Remove(subStart, subLength);
			_sb.Insert(baseStart + baseLength, sub);
		}

		var tag = (hasSub, hasSup, hasLimits) switch
		{
			(true, true, true) => "munderover",
			(true, true, false) => "msubsup",
			(true, false, true) => "munder",
			(true, false, false) => "msub",
			(false, true, true) => "mover",
			_ => "msup",
		};

		_sb.Insert(baseStart, '>').Insert(baseStart, tag).Insert(baseStart, '<');
		_sb.Append("</").Append(tag).Append('>');
	}

	/// <returns><see langword="true"/> if the atom is an operator with limits</returns>
	private bool WriteAtom()
	{
		var c = _text[_i];
		switch (c)
		{
			case '{':
				_i++;
				WriteRowElement('}');
				return false;
			case '\\':
				return WriteCommand();
			case '^' or '_':
				// a script without the base
				_sb.Append("<mrow></mrow>");
				return false;
		}

		_i++;
		if (char.IsLetter(c))
		{
			WriteToken("mi", c);
			return false;
		}

		if (char.IsAsciiDigit(c) || (c == '.' && _i < _end && char.IsAsciiDigit(_text[_i])))
		{
			_sb.Append("<mn>").Append(c);
			while (_i < _end && (char.IsAsciiDigit(_text[_i]) || (_text[_i] == '.' && _i + 1 < _end && char.IsAsciiDigit(_text[_i + 1]))))
				_sb.Append(_text[_i++]);

			_sb.Append("</mn>");
			return false;
		}

		switch (c)
		{
			case '-':
				WriteToken("mo", '−');
				break;
			case '*':
				WriteToken("mo", '∗');
				break;
			case '~':
				WriteSpace("0.25em");
				break;
			case '(' or ')' or '[' or ']' or '|':
				_sb.Append("<mo stretchy=\"false\">").Append(c).Append("</mo>");
				break;
			default:
				WriteToken("mo", c);
				break;
		}

		return false;
	}

	private bool WriteCommand()
	{
		var name = ReadCommandName();
		if (!TryGetCommand(name, out var command))
		{
			// unsupported commands are displayed as text like MathJax does
			_sb.Append("<mtext mathcolor=\"red\">\\");
			AppendEscaped(name);
			_sb.Append("</mtext>");
			return false;
		}

		switch (command.Type)
		{
			case CommandType.Identifier:
				WriteToken("mi", command.Value);
				break;
			case CommandType.UprightIdentifier:
				_sb.Append("<mi mathvariant=\"normal\">").Append(command.Value).Append("</mi>");
				break;
			case CommandType.Operator:
				WriteToken("mo", command.Value);
				break;
			case CommandType.LargeOperator:
				_sb.Append(command.Value.Length == 1 ? "<mo largeop=\"true\" movablelimits=\"true\">" : "<mo movablelimits=\"true\">").Append(command.Value).Append("</mo>");
				break;
			case CommandType.Integral:
				_sb.Append("<mo largeop=\"true\">").Append(command.Value).Append("</mo>");
				break;
			case CommandType.Function:
				_sb.Append("<mi>").Append(command.Value).Append("</mi><mo>&#x2061;</mo>");
				break;
			case CommandType.Space:
				WriteSpace(command.Value);
				break;
			case CommandType.Accent:
				_sb.Append("<mover accent=\"true\">");
				WriteArgumentRow();
				WriteToken("mo", command.Value);
				_sb.Append("</mover>");
				break;
			case CommandType.Font:
			{
				var text = ReadGroupText();
				var tag = command.HasLimits || IsLetters(text) ? "mi" : "mtext";
				_sb.Append('<').Append(tag).Append(" mathvariant=\"").Append(command.Value).Append("\">");
				AppendEscaped(text);
				_sb.Append("</").Append(tag).Append('>');
				break;
			}
			case CommandType.Fraction:
				_sb.Append("<mfrac>");
				WriteArgumentRow();
				WriteArgumentRow();
				_sb.Append("</mfrac>");
				break;
			case CommandType.Binomial:
				_sb.Append("<mrow><mo>(</mo><mfrac linethickness=\"0\">");
				WriteArgumentRow();
				WriteArgumentRow();
				_sb.Append("</mfrac><mo>)</mo></mrow>");
				break;
			case CommandType.Root:
				WriteRoot();
				break;
			case CommandType.Left:
			{
				_sb.Append("<mrow>");
				WriteFence();
				WriteRow(stop: null);
				if (IsAt("\\right"))
				{
					_i += "\\right".Length;
					WriteFence();
				}

				_sb.Append("</mrow>");
				break;
			}
			case CommandType.Text:
			{
				_sb.Append("<mtext>");
				foreach (var c in ReadGroupText())
				{
					if (c == ' ')
						_sb.Append("&#xA0;");
					else
						AppendEscaped(c);
				}

				_sb.Append("</mtext>");
				break;
			}
			case CommandType.Underline:
				_sb.Append("<munder accentunder=\"true\">");
				WriteArgumentRow();
				_sb.Append("<mo>_</mo></munder>");
				break;
			case CommandType.Underbrace:
				_sb.Append("<munder>");
				WriteArgumentRow();
				_sb.Append("<mo>⏟</mo></munder>");
				break;
		}

		return command.HasLimits;
	}

	private void WriteRoot()
	{
		SkipSpaces();
		if (_i >= _end || _text[_i] != '[')
		{
			_sb.Append("<msqrt>");
			WriteArgumentRow();
			_sb.Append("</msqrt>");
			return;
		}

		// MathML expects the radicand before the index
		_i++;
		var indexStart = _sb.Length;
		WriteRowElement(']');
		var index = _sb.ToString(indexStart, _sb.Length - indexStart);
		_sb.Length = indexStart;

		_sb.Append("<mroot>");
		WriteArgumentRow();
		_sb.Append(index).Append("</mroot>");
	}

	/// <summary>
	/// Writes a braced group or a single token wrapped in <c>mrow</c>
	/// </summary>
	private void WriteArgumentRow()
	{
		SkipSpaces();
		if (_i >= _end)
		{
			_sb.Append("<mrow></mrow>");
			return;
		}

		if (_text[_i] == '{')
		{
			_i++;
			WriteRowElement('}');
			return;
		}

		_sb.Append("<mrow>");
		if (_text[_i] == '\\')
		{
			WriteAtom();
		}
		else
		{
			// a single character, e.g. x^2 or \frac12
			var c = _text[_i++];
			WriteToken(char.IsAsciiDigit(c) ? "mn" : char.IsLetter(c) ? "mi" : "mo", c);
		}

		_sb.Append("</mrow>");
	}

	private void WriteRowElement(char stop)
	{
		_sb.Append("<mrow>");
		WriteRow(stop);
		_sb.Append("</mrow>");
	}

	private void WriteFence()
	{
		SkipSpaces();
		if (_i >= _end)
			return;

		string? delimiter;
		if (_text[_i] == '\\')
		{
			var name = ReadCommandName();
			delimiter = TryGetCommand(name, out var command) && command.Type == CommandType.Operator ? command.Value : null;
		}
		else
		{
			var c = _text[_i++];
			delimiter = c == '.' ? null : c.ToString();
		}

		if (delimiter == null)
			return;

		_sb.Append("<mo fence=\"true\" stretchy=\"true\">");
		AppendEscaped(delimiter);
		_sb.Append("</mo>");
	}

	private void WriteSpace(string width) =>
		_sb.Append("<mspace width=\"").Append(width).Append("\"></mspace>");

	private void WriteToken(string tag, char value)
	{
		_sb.Append('<').Append(tag).Append('>');
		AppendEscaped(value);
		_sb.Append("</").Append(tag).Append('>');
	}

	private void WriteToken(string tag, string value)
	{
		_sb.Append('<').Append(tag).Append('>');
		AppendEscaped(value);
		_sb.Append("</").Append(tag).Append('>');
	}

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

	private void SkipSpaces()
	{
		while (_i < _end && char.IsWhiteSpace(_text[_i]))
			_i++;
	}

	private bool IsAt(string value) =>
		_end - _i >= value.Length && _text.AsSpan(_i, value.Length).SequenceEqual(value);

	private void AppendEscaped(ReadOnlySpan<char> text)
	{
		foreach (var c in text)
			AppendEscaped(c);
	}

	private void AppendEscaped(char c)
	{
		switch (c)
		{
			case '<':
				_sb.Append("&lt;");
				break;
			case '>':
				_sb.Append("&gt;");
				break;
			case '&':
				_sb.Append("&amp;");
				break;
			case '"':
				_sb.Append("&quot;");
				break;
			default:
				_sb.Append(c);
				break;
		}
	}

	private static bool IsLetters(ReadOnlySpan<char> text)
	{
		foreach (var c in text)
			if (!char.IsLetter(c))
				return false;

		return true;
	}

	private static bool TryGetCommand(ReadOnlySpan<char> name, out Command command)
	{
#if NET9_0_OR_GREATER
		return CommandsLookup.TryGetValue(name, out command);
#else
		return Commands.TryGetValue(name.ToString(), out command);
#endif
	}

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

		Add(CommandType.Integral, ("int", "∫"), ("iint", "∬"), ("iiint", "∭"), ("oint", "∮"));

		Add(CommandType.Function,
			("sin", "sin"), ("cos", "cos"), ("tan", "tan"), ("cot", "cot"), ("sec", "sec"), ("csc", "csc"), ("arcsin", "arcsin"),
			("arccos", "arccos"), ("arctan", "arctan"), ("sinh", "sinh"), ("cosh", "cosh"), ("tanh", "tanh"), ("coth", "coth"),
			("log", "log"), ("ln", "ln"), ("lg", "lg"), ("exp", "exp"), ("arg", "arg"), ("deg", "deg"), ("dim", "dim"), ("hom", "hom"),
			("ker", "ker"), ("Pr", "Pr"));

		Add(CommandType.Space,
			(",", "0.167em"), (":", "0.222em"), (">", "0.222em"), (";", "0.278em"), ("!", "-0.167em"), (" ", "0.25em"),
			("quad", "1em"), ("qquad", "2em"));

		Add(CommandType.Accent,
			("hat", "^"), ("widehat", "^"), ("bar", "¯"), ("overline", "¯"), ("vec", "→"), ("overrightarrow", "→"), ("dot", "˙"),
			("ddot", "¨"), ("tilde", "~"), ("widetilde", "~"), ("acute", "´"), ("grave", "`"), ("check", "ˇ"), ("breve", "˘"));

		Add(CommandType.Font,
			("mathrm", "normal"), ("rm", "normal"), ("mathbf", "bold"), ("bf", "bold"), ("mathit", "italic"), ("mathbb", "double-struck"),
			("mathcal", "script"), ("mathscr", "script"), ("mathfrak", "fraktur"), ("mathsf", "sans-serif"), ("mathtt", "monospace"),
			("boldsymbol", "bold-italic"));

		Add(CommandType.Fraction, ("frac", ""), ("dfrac", ""), ("tfrac", ""), ("cfrac", ""));
		Add(CommandType.Binomial, ("binom", ""), ("dbinom", ""), ("tbinom", ""));
		Add(CommandType.Root, ("sqrt", ""));
		Add(CommandType.Left, ("left", ""));
		Add(CommandType.Text, ("text", ""), ("textrm", ""), ("mbox", ""), ("textbf", ""), ("textit", ""));
		Add(CommandType.Underline, ("underline", ""));

		// operators with the limits below and above in the display mode
		foreach (var (name, value) in new[]
		{
			("sum", "∑"), ("prod", "∏"), ("coprod", "∐"), ("bigcup", "⋃"), ("bigcap", "⋂"), ("bigoplus", "⨁"), ("bigotimes", "⨂"),
			("lim", "lim"), ("max", "max"), ("min", "min"), ("sup", "sup"), ("inf", "inf"), ("det", "det"), ("gcd", "gcd"),
		})
		{
			commands[name] = new Command(CommandType.LargeOperator, value, HasLimits: true);
		}

		commands["operatorname"] = new Command(CommandType.Font, "normal", HasLimits: true);
		commands["overbrace"] = new Command(CommandType.Accent, "⏞", HasLimits: true);
		commands["underbrace"] = new Command(CommandType.Underbrace, HasLimits: true);

		return commands.ToFrozenDictionary(StringComparer.Ordinal);
	}
}
