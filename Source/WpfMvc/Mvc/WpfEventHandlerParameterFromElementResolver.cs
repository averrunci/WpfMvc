// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfEventHandlerParameterFromElementResolver(object? associatedElement) : EventHandlerParameterFromElementResolver(associatedElement)
{
    protected override object? FindElement(string name)
        => AssociatedElement is FrameworkElement rootElement ? WpfController.ElementFinder.FindElement(rootElement, name) : null;
}