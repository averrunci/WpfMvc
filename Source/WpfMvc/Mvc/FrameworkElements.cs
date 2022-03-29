// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides routed events related with <see cref="FrameworkElement"/>.
/// </summary>
public static class FrameworkElements
{
    /// <summary>
    /// Occurs when it is requested to show a message with a message box.
    /// </summary>
    public static readonly RoutedEvent MessageRequestedEvent = EventManager.RegisterRoutedEvent("MessageRequested", RoutingStrategy.Bubble, typeof(MessageRequestedEventHandler), typeof(FrameworkElements));

    /// <summary>
    /// Occurs when it is requested to show a window.
    /// </summary>
    public static readonly RoutedEvent WindowRequestedEvent = EventManager.RegisterRoutedEvent("WindowRequested", RoutingStrategy.Bubble, typeof(WindowRequestedEventHandler), typeof(FrameworkElements));
}