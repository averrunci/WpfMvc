﻿// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to inject elements in a view to a controller.
/// </summary>
public interface IWpfElementInjector : IElementInjector<FrameworkElement>;