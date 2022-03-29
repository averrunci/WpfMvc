// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Provides functions for the controllers; a data context injection, elements injection,
/// routed event handlers, and command handlers injection.
/// </summary>
public class WpfController
{
    /// <summary>
    /// Occurs when an exception is not handled in event handlers.
    /// </summary>
    public static event UnhandledExceptionEventHandler? UnhandledException;

    /// <summary>
    /// Gets or sets the finder to find a data context in a view.
    /// </summary>
    public static IWpfDataContextFinder DataContextFinder
    {
        get => dataContextFinder;
        set
        {
            dataContextFinder = value;
            EnsureControllerTypeFinder();
        }
    }
    private static IWpfDataContextFinder dataContextFinder = new WpfDataContextFinder();

    /// <summary>
    /// Gets or sets the injector to inject a data context to a controller.
    /// </summary>
    public static IDataContextInjector DataContextInjector { get; set; } = new DataContextInjector();

    /// <summary>
    /// Gets or sets the finder to find a key of an element.
    /// </summary>
    public static IWpfElementKeyFinder ElementKeyFinder
    {
        get => elementKeyFinder;
        set
        {
            elementKeyFinder = value;
            EnsureControllerTypeFinder();
        }
    }
    private static IWpfElementKeyFinder elementKeyFinder = new WpfElementKeyFinder();

    /// <summary>
    /// Gets or sets the injector to inject elements in a view to a controller.
    /// </summary>
    public static IWpfElementInjector ElementInjector { get; set; } = new WpfElementInjector();

    /// <summary>
    /// Gets or sets the finder to find a type of a controller that controls a view.
    /// </summary>
    public static IWpfControllerTypeFinder ControllerTypeFinder { get; set; } = new WpfControllerTypeFinder(ElementKeyFinder, DataContextFinder);

    /// <summary>
    /// Gets or sets the factory to create a controller.
    /// </summary>
    public static IWpfControllerFactory ControllerFactory { get; set; } = new SimpleWpfControllerFactory();

    /// <summary>
    /// Gets the extension.
    /// </summary>
    protected static ICollection<IWpfControllerExtension> Extensions { get; } = new Collection<IWpfControllerExtension>();

    /// <summary>
    /// Adds an extension of a controller.
    /// </summary>
    /// <param name="extension">The extension of a controller.</param>
    public static void AddExtension(IWpfControllerExtension extension) => Extensions.Add(extension);

    /// <summary>
    /// Removes an extension of a controller.
    /// </summary>
    /// <param name="extension">The extension of a controller.</param>
    public static void RemoveExtension(IWpfControllerExtension extension) => Extensions.Remove(extension);

    /// <summary>
    /// Identifies the <see cref="KeyProperty"/> XAML attached property.
    /// </summary>
    public static readonly DependencyProperty KeyProperty = DependencyProperty.RegisterAttached(
        "Key", typeof(string), typeof(WpfController), new PropertyMetadata(default(string), OnKeyChanged)
    );

    /// <summary>
    /// Sets the value of the <see cref="KeyProperty"/> XAML attached property on the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="element">The element on which to set the <see cref="KeyProperty"/> XAML attached property.</param>
    /// <param name="value">The property value to set.</param>
    public static void SetKey(DependencyObject element, string? value) => element.SetValue(KeyProperty, value);

    /// <summary>
    /// Gets the value of the <see cref="KeyProperty"/> XAML attached property from the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="element">The element from which to read the property value.</param>
    /// <returns>The value of the <see cref="KeyProperty"/> XAML attached property on the target dependency object.</returns>
    public static string? GetKey(DependencyObject element) => (string?)element.GetValue(KeyProperty);

    private static void OnKeyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => SetIsEnabled(sender, true);

    /// <summary>
    /// Identifies the <see cref="IsEnabledProperty"/> XAML attached property.
    /// </summary>
    public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
        "IsEnabled", typeof(bool), typeof(WpfController), new PropertyMetadata(default(bool), OnIsEnabledChanged)
    );

    /// <summary>
    /// Sets the value of the <see cref="IsEnabledProperty"/> XAML attached property on the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="element">The element on which to set the <see cref="IsEnabledProperty"/> XAML attached property.</param>
    /// <param name="value">The property value to set.</param>
    public static void SetIsEnabled(DependencyObject element, bool value) => element.SetValue(IsEnabledProperty, value);

    /// <summary>
    /// Gets the value of the <see cref="IsEnabledProperty"/> XAML attached property from the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="element">The element from which to read the property value.</param>
    /// <returns>The value of the <see cref="IsEnabledProperty"/> XAML attached property on the target dependency object.</returns>
    public static bool GetIsEnabled(DependencyObject element) => (bool)element.GetValue(IsEnabledProperty);

    private static void OnIsEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not FrameworkElement element) throw new InvalidOperationException("Dependency object must be FrameworkElement.");

        if ((bool)e.NewValue)
        {
            if (element.DataContext is null)
            {
                element.DataContextChanged += OnElementDataContextChanged;
            }
            else
            {
                AttachControllers(element);
            }
        }
        else
        {
            element.DataContextChanged -= OnElementDataContextChanged;

            DetachControllers(element);
        }
    }

    private static readonly DependencyProperty ControllersProperty = DependencyProperty.RegisterAttached(
        "ShadowControllers", typeof(WpfControllerCollection), typeof(WpfController), new PropertyMetadata(default(WpfControllerCollection))
    );

    private static void OnElementDataContextChanged(object? sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not FrameworkElement element) return;

        element.DataContextChanged -= OnElementDataContextChanged;

        AttachControllers(element);
    }

    private static void AttachControllers(FrameworkElement element)
    {
        var controllers = new WpfControllerCollection(DataContextFinder, DataContextInjector, ElementInjector, Extensions);
        controllers.AddRange(ControllerTypeFinder.Find(element).Select(ControllerFactory.Create).OfType<object>());
        controllers.AttachTo(element);
        element.SetValue(ControllersProperty, controllers);
    }

    private static void DetachControllers(FrameworkElement element)
    {
        var controllers = element.GetValue(ControllersProperty) as WpfControllerCollection;
        controllers?.Detach();
        element.SetValue(ControllersProperty, null);
    }

    static WpfController()
    {
        EnsureControllerTypeFinder();

        Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IWpfControllerExtension).IsAssignableFrom(type))
            .Where(type => type.IsClass && !type.IsAbstract)
            .Select(type => Activator.CreateInstance(type) as IWpfControllerExtension)
            .Where(extension => extension is not null)
            .ForEach(extension => AddExtension(extension!));
    }

    private static void EnsureControllerTypeFinder()
    {
        ControllerTypeFinder = new WpfControllerTypeFinder(ElementKeyFinder, DataContextFinder);
    }

    /// <summary>
    /// Gets the collection of the controller associated with the specified <see cref="DependencyObject"/>.
    /// </summary>
    /// <param name="element">The element with which the collection of the controller is associated.</param>
    /// <returns>The collection of the controller associated with the specified <see cref="DependencyObject"/>.</returns>
    public static WpfControllerCollection GetControllers(DependencyObject element)
    {
        if (element.GetValue(ControllersProperty) is WpfControllerCollection controllers) return controllers;

        controllers = new WpfControllerCollection(DataContextFinder, DataContextInjector, ElementInjector, Extensions);
        element.SetValue(ControllersProperty, controllers);
        return controllers;
    }

    /// <summary>
    /// Sets the specified data context to the specified controller.
    /// </summary>
    /// <param name="dataContext">The data context that is set to the controller.</param>
    /// <param name="controller">The controller to which the data context is set.</param>
    public static void SetDataContext(object? dataContext, object controller) => DataContextInjector.Inject(dataContext, controller);

    /// <summary>
    /// Sets the specified element to the specified controller.
    /// </summary>
    /// <param name="rootElement">The element that is set to the controller.</param>
    /// <param name="controller">The controller to which the element is set.</param>
    /// <param name="foundElementOnly">
    /// If <c>true</c>, an element is not set to the controller when it is not found in the specified element;
    /// otherwise, <c>null</c> is set.
    /// </param>
    public static void SetElement(FrameworkElement? rootElement, object controller, bool foundElementOnly = false)
        => ElementInjector.Inject(rootElement, controller, foundElementOnly);

    /// <summary>
    /// Gets a container of an extension that the specified controller has.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <typeparam name="T">The type of the container of the extension.</typeparam>
    /// <param name="controller">The controller that has the extension.</param>
    /// <returns>The container of the extension that the specified controller has.</returns>
    public static T Retrieve<TExtension, T>(object controller) where TExtension : IWpfControllerExtension where T : class, new()
        => Extensions.OfType<TExtension>().FirstOrDefault()?.Retrieve(controller) as T ?? new T();

    /// <summary>
    /// Gets event handlers that the specified controller has.
    /// </summary>
    /// <param name="controller">The controller that has event handlers.</param>
    /// <returns>The event handlers that the specified controller has.</returns>
    public static EventHandlerBase<FrameworkElement, WpfEventHandlerItem> EventHandlersOf(object controller)
        => Retrieve<WpfEventHandlerExtension, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>(controller);

    /// <summary>
    /// Gets command handlers that the specified controller has.
    /// </summary>
    /// <param name="controller">The controller that has command handlers.</param>
    /// <returns>The command handlers that the specified controller has.</returns>
    public static CommandHandlerBase CommandHandlersOf(object controller)
        => Retrieve<CommandHandlerExtension, CommandHandlerBase>(controller);

    internal static bool HandleUnhandledException(Exception exc)
    {
        var e = new UnhandledExceptionEventArgs(exc);
        OnUnhandledException(e);
        return e.Handled;
    }

    private static void OnUnhandledException(UnhandledExceptionEventArgs e) => UnhandledException?.Invoke(null, e);
}