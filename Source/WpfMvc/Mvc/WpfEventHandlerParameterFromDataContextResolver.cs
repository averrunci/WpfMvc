// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfEventHandlerParameterFromDataContextResolver : EventHandlerParameterFromDataContextResolver
{
    public WpfEventHandlerParameterFromDataContextResolver(object? associatedElement) : base(associatedElement)
    {
    }

    protected override object? FindDataContext()
        => AssociatedElement is FrameworkElement view ? WpfController.DataContextFinder.Find(view) : null;
}