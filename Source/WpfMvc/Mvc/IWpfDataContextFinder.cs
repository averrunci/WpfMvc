﻿// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides the function to find a data context in a view.
/// </summary>
public interface IWpfDataContextFinder : IDataContextFinder<FrameworkElement>;