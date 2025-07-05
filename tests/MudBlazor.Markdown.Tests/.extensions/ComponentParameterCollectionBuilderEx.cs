using System.Linq.Expressions;

namespace MudBlazor.Markdown.Tests;

public static class ComponentParameterCollectionBuilderEx
{
	public static ComponentParameterCollectionBuilder<TComponent> TryAdd<TComponent, TValue>(
		this ComponentParameterCollectionBuilder<TComponent> @this,
		Expression<Func<TComponent, TValue>> parameterSelector,
		Optional<TValue> valueOptional)
		where TComponent : IComponent
	{
		return valueOptional.HasValue
			? @this.Add(parameterSelector, valueOptional.Value)
			: @this;
	}
}