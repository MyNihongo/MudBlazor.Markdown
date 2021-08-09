using System;
using System.Linq.Expressions;
using Bunit;
using Microsoft.AspNetCore.Components;

namespace MudBlazor.Markdown.Tests
{
    public static class ComponentParameterCollectionBuilderEx
    {
        public static ComponentParameterCollectionBuilder<TComponent> AddIfNotNull<TComponent, TValue>(
            this ComponentParameterCollectionBuilder<TComponent> @this,
            Expression<Func<TComponent, TValue>> parameterSelector,
            TValue value)
            where TComponent : IComponent
        {
            return value != null
                ? @this.Add(parameterSelector, value)
                : @this;
        }
    }
}