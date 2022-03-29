﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfDataContextFinder : IWpfDataContextFinder
{
    public object? Find(FrameworkElement view) => view.DataContext;
}