// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class DataContextChangedEventHandlerItem : WpfEventHandlerItem
{
    public DataContextChangedEventHandlerItem(string elementName, FrameworkElement? element, string eventName, Delegate? handler, bool handledEventsToo) : base(elementName, element, eventName, null, null, handler, handledEventsToo)
    {
    }

    protected override void AddEventHandler(FrameworkElement element, Delegate handler, bool handledEventsToo)
    {
        element.DataContextChanged += (DependencyPropertyChangedEventHandler)handler;
    }

    protected override void RemoveEventHandler(FrameworkElement element, Delegate handler)
    {
        element.DataContextChanged -= (DependencyPropertyChangedEventHandler)handler;
    }

    protected override object? Handle(Delegate handler, object? sender, object? e)
    {
        if (e is not DependencyPropertyChangedEventArgs args) return null;

        return (handler.Target as DataContextChangedEventHandlerAction)?.Handle(sender, args);
    }
}