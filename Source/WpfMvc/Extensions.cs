// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows;

internal static class Extensions
{
    public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
    {
        foreach (var item in @this)
        {
            action(item);
        }
    }

    public static TElement? FindElement<TElement>(this FrameworkElement? element, string name) where TElement : class
    {
        if (element is null) return null;
        if (string.IsNullOrEmpty(name)) return element as TElement;
        if (element.Name == name) return element as TElement;

        return LogicalTreeHelper.FindLogicalNode(element, name) as TElement ?? element.FindName(name) as TElement;
    }
}