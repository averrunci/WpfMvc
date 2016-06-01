// Copyright (C) 2016 Fievus
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
    public static class WpfController
    {
        private static readonly BindingFlags contextBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private static readonly BindingFlags routedEventBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private static readonly DependencyProperty ControllersProperty = DependencyProperty.RegisterAttached(
            "ShadowControllers", typeof(WpfControllerCollection), typeof(WpfController), new PropertyMetadata(OnControllersChanged)
        );

        private static readonly DependencyProperty RoutedEventHandlerBasesProperty = DependencyProperty.RegisterAttached(
            "ShadowRoutedEventHandlerBases", typeof(IDictionary<object, RoutedEventHandlerBase>), typeof(WpfController), new PropertyMetadata(null)
        );

        /// <summary>
        /// Gets or sets the injector of WPF controllers.
        /// </summary>
        public static IWpfControllerInjector Injector { get; set; }

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
                oldControllers.Detach();
            }

            if (newControllers != null && obj != null)
            {
                if (newControllers.AssociatedElement != null) { throw new InvalidOperationException("Associated element must be null."); }

                var element = obj as FrameworkElement;
                if (element == null) { throw new InvalidOperationException("Dependency object must be FrameworkElement."); }

                newControllers.AttachTo(element);
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
            element.Initialized += ElementOnInitialized;
            element.Unloaded += ElementOnUnloaded;
            element.DataContextChanged += ElementOnDataContextChanged;
        }

        /// <summary>
        /// Detaches the specified WPF controller from the specified element.
        /// </summary>
        /// <param name="controller">The WPF controller detached from the element.</param>
        /// <param name="element">The element from which the WPF controller is detached.</param>
        public static void Detach(object controller, FrameworkElement element)
        {
            if (controller == null || element == null) { return; }

            element.Initialized -= ElementOnInitialized;
            element.Unloaded -= ElementOnUnloaded;
            element.DataContextChanged -= ElementOnDataContextChanged;
            SetElement(null, controller);
            SetDataContext(null, controller);
            UnregisterRoutedEventHandler(element, controller);
        }

        /// <summary>
        /// Gets routed event handlers that the specified WPF controller has.
        /// </summary>
        /// <param name="controller">The WPF controller that has routed event handlers.</param>
        /// <returns>
        /// The routed event handlers that the specified WPF controller has.
        /// </returns>
        public static RoutedEventHandlerBase RetrieveRoutedEventHandlers(object controller)
            => controller == null ? new RoutedEventHandlerBase() : RetrieveRoutedEventHandlers(null, controller);

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
        public static void SetElement(FrameworkElement rootElement, object controller)
        {
            if (controller == null) { return; }

            controller.GetType()
                .GetFields(contextBindingFlags)
                .Select(Field => new { Field, Attribute = Field.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => t.Field.SetValue(controller, FindElement<FrameworkElement>(rootElement, t.Attribute.Name ?? t.Field.Name)));
            controller.GetType()
                .GetProperties(contextBindingFlags)
                .Select(Property => new { Property, Attribute = Property.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => t.Property.SetValue(controller, FindElement<FrameworkElement>(rootElement, t.Attribute.Name ?? t.Property.Name), null));
            controller.GetType()
                .GetMethods(contextBindingFlags)
                .Select(Method => new { Method, Attribute = Method.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t => t.Method.Invoke(controller, new object[] { FindElement<FrameworkElement>(rootElement, ResolveElementMethodName(t.Method, t.Attribute)) }));
        }

        private static string ResolveElementMethodName(MethodInfo m, ElementAttribute a)
            => a.Name ?? (m.Name.StartsWith("Set") ? m.Name.Substring(3) : m.Name);

        private static void RegisterRoutedEventHandler(FrameworkElement rootElement, object controller)
        {
            if (rootElement == null || controller == null) { return; }

            RetrieveRoutedEventHandlers(rootElement, controller).RegisterRoutedEventHandler();
        }

        private static void UnregisterRoutedEventHandler(FrameworkElement rootElement, object controller)
        {
            if (rootElement == null || controller == null) { return; }

            RetrieveRoutedEventHandlers(rootElement, controller).UnregisterRoutedEventHandler();
        }

        private static RoutedEventHandlerBase RetrieveRoutedEventHandlers(FrameworkElement rootElement, object controller)
        {
            var routedEventHandlerBases = EnsureRoutedEventHandlerBases(rootElement);
            if (routedEventHandlerBases.ContainsKey(controller)) { return routedEventHandlerBases[controller]; }

            var routedEventHandlers = new RoutedEventHandlerBase();
            routedEventHandlerBases[controller] = routedEventHandlers;

            controller.GetType().GetFields(contextBindingFlags)
                .Where(field => field.GetCustomAttributes<RoutedEventHandlerAttribute>().Any())
                .ForEach(field => AddRoutedEventHandlers(field, rootElement, e => CreateRoutedEventHandler(field.GetValue(controller) as Delegate, e), routedEventHandlers));
            controller.GetType().GetProperties(contextBindingFlags)
                .Where(property => property.GetCustomAttributes<RoutedEventHandlerAttribute>().Any())
                .ForEach(property => AddRoutedEventHandlers(property, rootElement, e => CreateRoutedEventHandler(property.GetValue(controller, null) as Delegate, e), routedEventHandlers));
            controller.GetType().GetMethods(contextBindingFlags)
                .Where(method => method.GetCustomAttributes<RoutedEventHandlerAttribute>().Any())
                .ForEach(method => AddRoutedEventHandlers(method, rootElement, e => CreateRoutedEventHandler(method, e, controller), routedEventHandlers));

            return routedEventHandlers;
        }

        private static IDictionary<object, RoutedEventHandlerBase> EnsureRoutedEventHandlerBases(FrameworkElement rootElement)
        {
            if (rootElement == null) { return new Dictionary<object, RoutedEventHandlerBase>(); }

            var routedEventHandlerBases = rootElement.GetValue(RoutedEventHandlerBasesProperty) as IDictionary<object, RoutedEventHandlerBase>;
            if (routedEventHandlerBases != null) { return routedEventHandlerBases; }

            routedEventHandlerBases = new Dictionary<object, RoutedEventHandlerBase>();
            rootElement.SetValue(RoutedEventHandlerBasesProperty, routedEventHandlerBases);
            return routedEventHandlerBases;
        }

        private static Delegate CreateRoutedEventHandler(Delegate @delegate, RoutedEvent routedEvent)
        {
            return @delegate == null ? null : CreateRoutedEventHandler(@delegate.Method, routedEvent, @delegate.Target);
        }

        private static Delegate CreateRoutedEventHandler(MethodInfo method, RoutedEvent routedEvent, object target)
        {
            if (method == null) { return null; }

            switch (method.GetParameters().Length)
            {
                case 0:
                    return new RoutedEventHandler((s, e) => method.Invoke(target, null));
                case 1:
                    return new RoutedEventHandler((s, e) => method.Invoke(target, new object[] { e }));
                case 2:
                    return routedEvent == null ? new RoutedEventHandler((s, e) => method.Invoke(target, new object[] { s, e })) : Delegate.CreateDelegate(routedEvent.HandlerType, target, method.Name);
                default:
                    throw new InvalidOperationException("The length of the method parameters must be less than 3.");
            }
        }

        private static void AddRoutedEventHandlers(MemberInfo member, FrameworkElement rootElement, Func<RoutedEvent, Delegate> createHandler, RoutedEventHandlerBase routedEventHandlers)
        {
            if (routedEventHandlers == null) { return; }

            member.GetCustomAttributes<RoutedEventHandlerAttribute>(true)
                .ForEach(routedEventHandler =>
                {
                    var element = FindElement<FrameworkElement>(rootElement, routedEventHandler.ElementName);
                    var routedEvent = RetrieveRoutedEvent(element, routedEventHandler.RoutedEvent);
                    routedEventHandlers.Add(
                        routedEventHandler.ElementName, element, 
                        routedEventHandler.RoutedEvent, routedEvent,
                        createHandler(routedEvent), routedEventHandler.HandledEventsToo
                    );
                });
        }

        private static E FindElement<E>(FrameworkElement element, string name) where E : FrameworkElement
        {
            if (element == null) { return null; }
            if (string.IsNullOrEmpty(name)) { return element as E; }
            if (element.Name == name) { return element as E; }

            return LogicalTreeHelper.FindLogicalNode(element, name) as E;
        }

        private static RoutedEvent RetrieveRoutedEvent(FrameworkElement element, string name)
        {
            if (element == null) { return null; }

            return element.GetType()
                .GetFields(routedEventBindingFlags)
                .Where(field => field.Name == EnsureRoutedEventName(name))
                .Select(field => field.GetValue(element))
                .FirstOrDefault() as RoutedEvent;
        }

        private static string EnsureRoutedEventName(string routedEventName)
        {
            return routedEventName != null && !routedEventName.EndsWith("Event") ? string.Format("{0}Event", routedEventName) : routedEventName;
        }

        private static void ElementOnInitialized(object sender, EventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) { return; }

            var controllers = element.GetValue(ControllersProperty) as WpfControllerCollection;
            if (controllers == null) { return; }

            controllers.ForEach(controller =>
            {
                SetElement(element, controller);
                RegisterRoutedEventHandler(element, controller);
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
                UnregisterRoutedEventHandler(element, controller);
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
