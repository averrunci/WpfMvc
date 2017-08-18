// Copyright (C) 2016-2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Provides functions for the WPF controllers; a data context injection, elements injection,
    /// and routed event handlers injection.
    /// </summary>
    public class WpfController
    {
        private static readonly BindingFlags contextBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static readonly DependencyProperty ControllersProperty = DependencyProperty.RegisterAttached(
            "ShadowControllers", typeof(WpfControllerCollection), typeof(WpfController), new PropertyMetadata(OnControllersChanged)
        );

        /// <summary>
        /// Gets or sets the injector of WPF controllers.
        /// </summary>
        public static IWpfControllerInjector Injector { get; set; }

        /// <summary>
        /// Gets or sets the factory of WPF controller.
        /// </summary>
        public static IWpfControllerFactory Factory
        {
            get { return factory; }
            set { factory = value.RequireNonNull(nameof(value)); }
        }
        private static IWpfControllerFactory factory = new SimpleWpfControllerFactory();

        /// <summary>
        /// Gets or sets the type of WPF controller.
        /// </summary>
        public Type ControllerType { get; set; }

        /// <summary>
        /// Creates a WPF controller of the specified controller type.
        /// </summary>
        /// <returns>The new instance of a WPF controller of the specified controller type.</returns>
        public object Create() => ControllerType == null ? null : Factory.Create(ControllerType);

        private static IList<IWpfControllerExtension> Extensions { get; } = new List<IWpfControllerExtension>();

        static WpfController()
        {
            Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IWpfControllerExtension).IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract)
                .ForEach(t => Add(Activator.CreateInstance(t) as IWpfControllerExtension));
        }

        /// <summary>
        /// Adds an extension of a WPF controller.
        /// </summary>
        /// <param name="extensions">The extension of a WPF controller</param>
        public static void Add(IWpfControllerExtension extensions)
        {
            Extensions.Add(extensions.RequireNonNull(nameof(extensions)));
        }

        /// <summary>
        /// Removes an extension of a WPF controller.
        /// </summary>
        /// <param name="extensions">The extension of a WPF controller</param>
        public static void Remove(IWpfControllerExtension extensions)
        {
            Extensions.Remove(extensions.RequireNonNull(nameof(extensions)));
        }

        /// <summary>
        /// Gets WPF controllers attached to the specified depencency object.
        /// </summary>
        /// <param name="obj">The dependency object to which WPF controllers are attached.</param>
        /// <returns>
        /// WPF controllers attached to the specified dependency object.
        /// </returns>
        public static WpfControllerCollection GetControllers(DependencyObject obj)
        {
            var controllers = obj.RequireNonNull(nameof(obj)).GetValue(ControllersProperty) as WpfControllerCollection;
            if (controllers == null)
            {
                controllers = new WpfControllerCollection();
                obj.SetValue(ControllersProperty, controllers);
            }
            return controllers;
        }

        private static void OnControllersChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var oldControllers = args.OldValue as WpfControllerCollection;
            var newControllers = args.NewValue as WpfControllerCollection;
            if (oldControllers == newControllers) { return; }

            if (oldControllers != null)
            {
                if (oldControllers.AssociatedElement == null) { throw new InvalidOperationException("Associated element must not be null."); }

                oldControllers.AssociatedElement.Initialized -= ElementOnInitialized;
                oldControllers.AssociatedElement.Unloaded -= ElementOnUnloaded;
                oldControllers.AssociatedElement.DataContextChanged -= ElementOnDataContextChanged;
                oldControllers.Detach();
            }

            if (newControllers != null && obj != null)
            {
                if (newControllers.AssociatedElement != null) { throw new InvalidOperationException("Associated element must be null."); }

                var element = obj as FrameworkElement;
                if (element == null) { throw new InvalidOperationException("Dependency object must be FrameworkElement."); }

                newControllers.AttachTo(element);
                element.Initialized += ElementOnInitialized;
                element.Unloaded += ElementOnUnloaded;
                element.DataContextChanged += ElementOnDataContextChanged;
            }
        }

        /// <summary>
        /// Attaches the specified WPF controller to the specified element.
        /// </summary>
        /// <param name="controller">The WPF controller attached to the element.</param>
        /// <param name="element">The element to which the WPF controllers is attached.</param>
        public static void Attach(object controller, FrameworkElement element)
        {
            if (controller == null || element == null) { return; }

            if (Injector != null) { Injector.Inject(controller); }

            SetDataContext(element.DataContext, controller);
            if (element.IsLoaded) { ElementOnInitialized(element, EventArgs.Empty); }
        }

        /// <summary>
        /// Detaches the specified WPF controller from the specified element.
        /// </summary>
        /// <param name="controller">The WPF controller detached from the element.</param>
        /// <param name="element">The element from which the WPF controller is detached.</param>
        public static void Detach(object controller, FrameworkElement element)
        {
            if (controller == null || element == null) { return; }

            SetElement(null, controller);
            SetDataContext(null, controller);
            Extensions.ForEach(extension => extension.Detach(element, controller));
        }

        /// <summary>
        /// Gets an extension that the specified WPF controller has.
        /// </summary>
        /// <typeparam name="E">The type of the extension.</typeparam>
        /// <typeparam name="T">The type of the container of the extension.</typeparam>
        /// <param name="controller">The WPF controller that has the extension.</param>
        /// <returns>The container of the extension that the specified WPF controller has.</returns>
        public static T Retrieve<E, T>(object controller) where E : IWpfControllerExtension where T : class, new()
            => (Extensions.OfType<E>().FirstOrDefault()?.Retrieve(controller) as T) ?? new T();

        /// <summary>
        /// Gets routed event handlers that the specified WPF controller has.
        /// </summary>
        /// <param name="controller">The WPF controller that has routed event handlers.</param>
        /// <returns>
        /// The routed event handlers that the specified WPF controller has.
        /// </returns>
        public static RoutedEventHandlerBase RetrieveRoutedEventHandlers(object controller)
            => Retrieve<RoutedEventHandlerExtension, RoutedEventHandlerBase>(controller);

        /// <summary>
        /// Gets command handlers that the specified WPF controller has.
        /// </summary>
        /// <param name="controller">The WPF controller that has command handlers.</param>
        /// <returns>
        /// The command handlers that the specified WPF controller has.
        /// </returns>
        public static CommandHandlerBase RetrieveCommandHandlers(object controller)
            => Retrieve<CommandHandlerExtension, CommandHandlerBase>(controller);

        /// <summary>
        /// Sets the specified data context to the specified WPF controller.
        /// </summary>
        /// <param name="dataContext">The data context that is set to the WPF controller.</param>
        /// <param name="controller">The WPF controller to which the data context is set.</param>
        public static void SetDataContext(object dataContext, object controller)
        {
            if (controller == null) { return; }

            controller.GetType()
                .GetFields(contextBindingFlags)
                .Where(field => field.GetCustomAttribute<DataContextAttribute>(true) != null)
                .ForEach(field => field.SetValue(controller, dataContext));
            controller.GetType()
                .GetProperties(contextBindingFlags)
                .Where(property => property.GetCustomAttribute<DataContextAttribute>(true) != null)
                .ForEach(property => property.SetValue(controller, dataContext, null));
            controller.GetType()
                .GetMethods(contextBindingFlags)
                .Where(method => method.GetCustomAttribute<DataContextAttribute>(true) != null)
                .ForEach(method => method.Invoke(controller, new object[] { dataContext }));
        }

        /// <summary>
        /// Sets the specified element to the specified WPF controller.
        /// </summary>
        /// <param name="rootElement">The element that is set to the WPF controller.</param>
        /// <param name="controller">The WPF controller to which the element is set.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the UWP controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        public static void SetElement(FrameworkElement rootElement, object controller, bool foundElementOnly = false)
        {
            if (controller == null) { return; }

            controller.GetType()
                .GetFields(contextBindingFlags)
                .Select(Field => new { Field, Attribute = Field.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => {
                    var element = rootElement.FindElement<object>(t.Attribute.Name ?? t.Field.Name);
                    if (!foundElementOnly || element != null) {
                        t.Field.SetValue(controller, element);
                    }
                });
            controller.GetType()
                .GetProperties(contextBindingFlags)
                .Select(Property => new { Property, Attribute = Property.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t =>
                {
                    var element = rootElement.FindElement<object>(t.Attribute.Name ?? t.Property.Name);
                    if (!foundElementOnly || element != null)
                    {
                        t.Property.SetValue(controller, element, null);
                    }
                });
            controller.GetType()
                .GetMethods(contextBindingFlags)
                .Select(Method => new { Method, Attribute = Method.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t =>
                {
                    var element = rootElement.FindElement<object>(ResolveElementMethodName(t.Method, t.Attribute));
                    if (!foundElementOnly || element != null)
                    {
                        t.Method.Invoke(controller, new object[] { element });
                    }
                });
        }

        private static string ResolveElementMethodName(MethodInfo m, ElementAttribute a)
            => a.Name ?? (m.Name.StartsWith("Set") ? m.Name.Substring(3) : m.Name);


        private static void ElementOnInitialized(object sender, EventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) { return; }

            var controllers = element.GetValue(ControllersProperty) as WpfControllerCollection;
            if (controllers == null) { return; }

            controllers.ForEach(controller =>
            {
                SetElement(element, controller);
                Extensions.ForEach(Extension => Extension.Attach(element, controller));
            });
        }

        private static void ElementOnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) { return; }

            var controllers = element.GetValue(ControllersProperty) as WpfControllerCollection;
            if (controllers == null) { return; }

            controllers.ForEach(controller =>
            {
                SetElement(null, controller);
                Extensions.ForEach(Extension => Extension.Detach(element, controller));
            });
        }

        private static void ElementOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var dependencyObject = sender as DependencyObject;
            if (dependencyObject == null) { return; }

            var controllers = dependencyObject.GetValue(ControllersProperty) as WpfControllerCollection;
            if (controllers == null) { return; }

            controllers.ForEach(controller => SetDataContext(e.NewValue, controller));
        }
    }
}
