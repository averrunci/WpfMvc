// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents the method that handles the <see cref="FrameworkElements.WindowRequestedEvent"/> routed event.
/// </summary>
/// <param name="sender">The object where the event handler is attached.</param>
/// <param name="e">The event data.</param>
public delegate void WindowRequestedEventHandler(object? sender, WindowRequestedEventArgs e);

/// <summary>
/// Provides data for the <see cref="FrameworkElements.WindowRequestedEvent"/> routed event.
/// </summary>
public class WindowRequestedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Gets or sets the content of the window.
    /// </summary>
    public object? Content { get; set; }

    /// <summary>
    /// Gets or sets the data context for an element when it participates in data binding.
    /// </summary>
    public object? DataContext { get; set; }

    /// <summary>
    /// Gets or sets the location of the window, in relation to the desktop.
    /// </summary>
    public Point Location { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether to show as the modal window.
    /// </summary>
    /// <remarks>
    /// <c>true</c> to show as the modal window; <c>false</c> to show as the modeless window.
    /// </remarks>
    public bool Modal { get; set; }

    /// <summary>
    /// Gets or sets a value that indicates whether the current active window owns the window.
    /// </summary>
    /// <remarks>
    /// <c>true</c> if the current active window owns the window; otherwise, <c>false</c>.
    /// </remarks>
    public bool OwnedWindow { get; set; }

    /// <summary>
    /// Gets or sets the style used by the window when it is rendered.
    /// </summary>
    public Style? Style { get; set; }

    /// <summary>
    /// Gets or sets the position of the window when first shown.
    /// </summary>
    public WindowStartupLocation WindowStartupLocation { get; set; }

    /// <summary>
    /// Gets or sets the type of the window.
    /// </summary>
    public Type? WindowType { get; set; }

    /// <summary>
    /// Gets or sets the action which is executed when the window is created.
    /// </summary>
    public Action<Window>? WindowCreated { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="Nullable"/> value of type <see cref="Boolean"/>
    /// that specifies whether the activity wa accepted (<c>true</c>) or canceled (<c>false</c>).
    /// </summary>
    public bool? Result { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowRequestedEventArgs"/> class.
    /// </summary>
    public WindowRequestedEventArgs()
    {
    }
}