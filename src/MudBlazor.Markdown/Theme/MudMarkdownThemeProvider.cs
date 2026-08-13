#if NET10_0_OR_GREATER
using System.Runtime.CompilerServices;
#else
using System.Reflection;
#endif
using System.Text;
using MudBlazor.State;

namespace MudBlazor;

public class MudMarkdownThemeProvider : MudThemeProvider
{
	protected override void GenerateTheme(StringBuilder themeStringBuilder)
	{
		base.GenerateTheme(themeStringBuilder);

		var theme = GetTheme();
		var palette = IsDarkModeState(this).Value ? theme.PaletteDark : theme.PaletteLight;

		if (palette is not IMudMarkdownPalette mudMarkdownPalette)
			return;
	}

#if NET10_0_OR_GREATER
	[UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_isDarkModeState")]
	private static extern ref ParameterState<bool> IsDarkModeState(MudThemeProvider provider);
#else
	private static ParameterState<bool> IsDarkModeState(MudThemeProvider provider)
	{
		var field = typeof(MudThemeProvider).GetField("_isDarkModeState", BindingFlags.Instance | BindingFlags.NonPublic)
		            ?? throw new InvalidOperationException($"Unable to find `_isDarkModeState` on ${nameof(MudThemeProvider)}");

		return (ParameterState<bool>)field.GetValue(provider)!;
	}
#endif
}
