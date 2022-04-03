// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation;

internal static class Extensions
{
    public static void IfPresent<T>(this T? @this, Action<T> action)
    {
        if (@this is null) return;

        action(@this);
    }
}