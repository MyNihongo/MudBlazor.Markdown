namespace MudBlazor;

internal static class ParameterViewEx
{
	public static IDictionary<string, object?> ToMutableDictionary(this ParameterView @this)
	{
		var dictionary = @this.ToDictionary();

		if (dictionary is IDictionary<string, object?> mutableDictionary)
			return mutableDictionary;

		mutableDictionary = new Dictionary<string, object?>(dictionary.Count);

		foreach (var pair in dictionary)
			mutableDictionary.Add(pair.Key, pair.Value);

		return mutableDictionary;
	}
}
