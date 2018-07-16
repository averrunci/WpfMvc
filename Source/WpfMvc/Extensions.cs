// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Charites.Windows
{
    internal static class Extensions
    {
        public static void IfPresent<T>(this T @this, Action<T> action)
        {
            if (@this == null) return;

            action(@this);
        }

        public static void IfAbsent<T>(this T @this, Action action)
        {
            if (@this != null) return;

            action();
        }

        public static T RequireNonNull<T>(this T @this) => RequireNonNull(@this, null);

        public static T RequireNonNull<T>(this T @this, string name)
        {
            if (@this == null) throw new ArgumentNullException(name);

            return @this;
        }

        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            if (@this == null) return;

            foreach (var item in @this)
            {
                action(item);
            }
        }

        public static bool IsEmpty<T>(this IEnumerable<T> @this) => !@this.Any();

        public static TElement FindElement<TElement>(this FrameworkElement element, string name) where TElement : class
        {
            if (element == null) return null;
            if (string.IsNullOrEmpty(name)) return element as TElement;
            if (element.Name == name) return element as TElement;

            return LogicalTreeHelper.FindLogicalNode(element, name) as TElement ?? element.FindName(name) as TElement;
        }

        public static string GetFullNameWithoutParameters(this Type @this)
        {
            var dataTypeFullName = @this.ToString();
            var parameterStartIndex = dataTypeFullName.IndexOf('[');
            return parameterStartIndex < 0 ? null : dataTypeFullName.Substring(0, parameterStartIndex);
        }
    }
}
