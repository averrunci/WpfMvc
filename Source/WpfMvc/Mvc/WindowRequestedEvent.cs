// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Raises a <see cref="FrameworkElements.WindowRequestedEvent"/> routed event.
/// </summary>
public class WindowRequestedEvent
{
    private RoutedEvent routedEvent = FrameworkElements.WindowRequestedEvent;
    private object? source;

    private object? content;
    private object? dataContext;
    private Point location;
    private bool modal;
    private bool ownedWindow;
    private Style? style;
    private WindowStartupLocation windowStartupLocation;
    private Type? windowType;
    private Action<Window>? windowCreated;

    /// <summary>
    /// Gets or sets the <see cref="Nullable"/> value of type <see cref="Boolean"/>
    /// that specifies whether the activity wa accepted (<c>true</c>) or canceled (<c>false</c>).
    /// </summary>
    public bool? Result { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowRequestedEvent"/> class.
    /// </summary>
    protected WindowRequestedEvent()
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="WindowRequestedEvent"/> class
    /// to request a modal window.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public static WindowRequestedEvent Modal() => new() { modal = true };

    /// <summary>
    /// Creates a new instance of the <see cref="WindowRequestedEvent"/> class
    /// to request a modeless window.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public static WindowRequestedEvent Modeless() => new();

    /// <summary>
    /// Raises a <see cref="FrameworkElements.WindowRequestedEvent"/> routed event
    /// from the specified element.
    /// </summary>
    /// <param name="element">
    /// The element on which a <see cref="FrameworkElements.WindowRequestedEvent"/>
    /// routed event is raised.
    /// </param>
    /// <returns>
    /// The data for the raised <see cref="FrameworkElements.WindowRequestedEvent"/> routed event.
    /// </returns>
    public WindowRequestedEventArgs RaiseFrom(FrameworkElement element)
    {
        var args = new WindowRequestedEventArgs
        {
            RoutedEvent = routedEvent,
            Source = source,
            Content = content,
            DataContext = dataContext,
            Location = location,
            Modal = modal,
            OwnedWindow = ownedWindow,
            Style = style,
            WindowStartupLocation = windowStartupLocation,
            WindowCreated = windowCreated,
            WindowType = windowType,
        };
        element.RaiseEvent(args);
        return args;
    }

    /// <summary>
    /// Sets the <see cref="RoutedEvent"/> associated with the
    /// <see cref="RoutedEventArgs"/> instance.
    /// </summary>
    /// <param name="routedEvent">
    /// The <see cref="RoutedEvent"/> associated with the
    /// <see cref="RoutedEventArgs"/> instance.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent At(RoutedEvent routedEvent)
    {
        this.routedEvent = routedEvent;
        return this;
    }

    /// <summary>
    /// Sets the reference to the object that raised the event.
    /// </summary>
    /// <param name="source">The reference to the object that raised the event.</param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent From(object? source)
    {
        this.source = source;
        return this;
    }

    /// <summary>
    /// Sets the content of the window.
    /// </summary>
    /// <param name="content">The content of the window.</param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent WithContent(object? content)
    {
        this.content = content;
        return this;
    }

    /// <summary>
    /// Sets the data context for an element when it participates in data binding.
    /// </summary>
    /// <param name="dataContext">
    /// The data context for an element when it participates in data binding.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent WithDataContext(object? dataContext)
    {
        this.dataContext = dataContext;
        return this;
    }

    /// <summary>
    /// Sets a value that specifies that the current active window owns the window.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent AsOwnedWindow() => WithOwnedWindow(true);

    /// <summary>
    /// Sets a value that indicates whether the current active window owns the window.
    /// </summary>
    /// <param name="ownedWindow">
    /// <c>true</c> if the current active window owns the window; otherwise, <c>false</c>.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent WithOwnedWindow(bool ownedWindow)
    {
        this.ownedWindow = ownedWindow;
        return this;
    }

    /// <summary>
    /// Sets the resource key of the style used by the window when it is rendered.
    /// </summary>
    /// <param name="resourceKey">
    /// The resource key of the style used by the window when it is rendered.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent WithStyleOf(string resourceKey) => With(Application.Current.FindResource(resourceKey) as Style);

    /// <summary>
    /// Sets the style used by the window when it is rendered.
    /// </summary>
    /// <param name="style">The style used by the window when it is rendered.</param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent With(Style? style)
    {
        this.style = style;
        return this;
    }

    /// <summary>
    /// Sets the window position to the center of the owner.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent StartupAtCenterOwner() => With(WindowStartupLocation.CenterOwner);

    /// <summary>
    /// Sets the window position to the center of the screen.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent StartupAtCenterScreen() => With(WindowStartupLocation.CenterScreen);

    /// <summary>
    /// Sets the location of the window, in relation to the desktop.
    /// </summary>
    /// <param name="left">
    /// The position of the window's left edge, in relation to the desktop.
    /// </param>
    /// <param name="top">
    /// The position of the window's top edge, in relation to the desktop.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent StartupAt(double left, double top) => StartupAt(new Point(left, top));

    /// <summary>
    /// Sets the location of the window, in relation to the desktop.
    /// </summary>
    /// <param name="location">
    /// The location of the window, in relation to the desktop.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent StartupAt(Point location)
    {
        this.location = location;
        return With(WindowStartupLocation.Manual);
    }

    /// <summary>
    /// Sets the position of the window when first shown.
    /// </summary>
    /// <param name="windowStartupLocation">
    /// The position of the window when first shown.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent With(WindowStartupLocation windowStartupLocation)
    {
        this.windowStartupLocation = windowStartupLocation;
        return this;
    }

    /// <summary>
    /// Sets the type of the window.
    /// </summary>
    /// <param name="windowType">The type of the window.</param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent WindowTypeOf(Type? windowType)
    {
        this.windowType = windowType;
        return this;
    }

    /// <summary>
    /// Sets the action which is executed when the window is created.
    /// </summary>
    /// <param name="windowCreated">
    /// The action which is executed when the window is created.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="WindowRequestedEvent"/> class.
    /// </returns>
    public WindowRequestedEvent With(Action<Window>? windowCreated)
    {
        this.windowCreated = windowCreated;
        return this;
    }
}