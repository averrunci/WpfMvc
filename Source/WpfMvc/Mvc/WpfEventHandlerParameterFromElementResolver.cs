// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfEventHandlerParameterFromElementResolver : EventHandlerParameterFromElementResolver
{
    public WpfEventHandlerParameterFromElementResolver(object? associatedElement) : base(associatedElement)
    {
    }

    protected override object? FindElement(string name)
        => AssociatedElement is FrameworkElement rootElement ? WpfController.ElementFinder.FindElement(rootElement, name) : null;
}