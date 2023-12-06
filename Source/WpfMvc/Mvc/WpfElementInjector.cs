// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfElementInjector(IElementFinder<FrameworkElement> elementFinder) : ElementInjector<FrameworkElement>(elementFinder), IWpfElementInjector;