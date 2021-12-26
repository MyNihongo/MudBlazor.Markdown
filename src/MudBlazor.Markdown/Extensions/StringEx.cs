// ReSharper disable once CheckNamespace
namespace MudBlazor;

internal static class StringEx
{
	public static bool IsExternalUri(this string? @this, string? baseUri)
	{
		if (string.IsNullOrEmpty(@this) || string.IsNullOrEmpty(baseUri))
			return false;

		try
		{
			var uri = new Uri(@this, UriKind.RelativeOrAbsolute);
			return uri.IsAbsoluteUri && !@this.StartsWith(baseUri);
		}
		catch
		{
			return false;
		}
	}
}