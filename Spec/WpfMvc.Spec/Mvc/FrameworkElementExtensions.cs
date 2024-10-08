﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal static class FrameworkElementExtensions
{
    public static T GetController<T>(this FrameworkElement element) => WpfController.GetControllers(element).OfType<T>().First();
}