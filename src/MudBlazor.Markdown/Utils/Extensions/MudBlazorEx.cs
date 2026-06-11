#if NET10_0_OR_GREATER
using System.Runtime.CompilerServices;
#else
using System.Reflection;
#endif

namespace MudBlazor;

internal static class MudBlazorEx
{
#if NET10_0_OR_GREATER
	[UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "ToStringFast")]
	private static extern string MudToStringFast(
		[UnsafeAccessorType("MudBlazor.ColorExtensions, MudBlazor")]
		object _,
		Color color
	);

	[UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "ToStringFast")]
	private static extern string MudToStringFast(
		[UnsafeAccessorType("MudBlazor.UnderlineExtensions, MudBlazor")]
		object _,
		Underline underline
	);

	[UnsafeAccessor(UnsafeAccessorKind.StaticMethod, Name = "ToStringFast")]
	private static extern string MudToStringFast(
		[UnsafeAccessorType("MudBlazor.TypoExtensions, MudBlazor")]
		object _,
		Typo typo
	);
#else
	private static string MudToStringFast(object _, Color color)
	{
		var extensions = typeof(Color).Assembly.GetType("MudBlazor.ColorExtensions");
		var method = extensions?.GetMethod("ToStringFast", types: [typeof(Color)], bindingAttr: BindingFlags.Public | BindingFlags.Static);
		return (string?)method?.Invoke(null, [color]) ?? throw new InvalidOperationException("Unable to find Color from MudBlazor");
	}

	private static string MudToStringFast(object _, Underline underline)
	{
		var extensions = typeof(Underline).Assembly.GetType("MudBlazor.UnderlineExtensions");
		var method = extensions?.GetMethod("ToStringFast", types: [typeof(Underline)], bindingAttr: BindingFlags.Public | BindingFlags.Static);
		return (string?)method?.Invoke(null, [underline]) ?? throw new InvalidOperationException("Unable to find Underline from MudBlazor");
	}

	private static string MudToStringFast(object _, Typo typo)
	{
		var extensions = typeof(Typo).Assembly.GetType("MudBlazor.TypoExtensions");
		var method = extensions?.GetMethod("ToStringFast", types: [typeof(Typo)], bindingAttr: BindingFlags.Public | BindingFlags.Static);
		return (string?)method?.Invoke(null, [typo]) ?? throw new InvalidOperationException("Unable to find Typo from MudBlazor");
	}
#endif

	public static string ToStringFast(this Color color)
		=> MudToStringFast(null!, color);

	public static string ToStringFast(this Underline underline)
		=> MudToStringFast(null!, underline);

	public static string ToStringFast(this Typo typo)
		=> MudToStringFast(null!, typo);
}
