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
    /// Represents an extension to handle a routed event.
    /// </summary>
    public sealed class RoutedEventHandlerExtension : IWpfControllerExtension
    {
        private static readonly BindingFlags routedEventHandlerBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private static readonly BindingFlags routedEventBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private static readonly DependencyProperty RoutedEventHandlerBasesProperty = DependencyProperty.RegisterAttached(
            "ShadowRoutedEventHandlerBases", typeof(IDictionary<object, RoutedEventHandlerBase>), typeof(RoutedEventHandlerExtension), new PropertyMetadata(null)
        );

        void IWpfControllerExtension.Attach(FrameworkElement element, object controller)
        {
            if (element == null || controller == null) { return; }

            RetrieveRoutedEventHandlers(element, controller).RegisterRoutedEventHandler();
        }

        void IWpfControllerExtension.Detach(FrameworkElement element, object controller)
        {
            if (element == null || controller == null) { return; }

            RetrieveRoutedEventHandlers(element, controller).UnregisterRoutedEventHandler();
        }

        object IWpfControllerExtension.Retrieve(object controller)
        {
            return controller == null ? new RoutedEventHandlerBase() : RetrieveRoutedEventHandlers(null, controller);
        }

        private RoutedEventHandlerBase RetrieveRoutedEventHandlers(FrameworkElement rootElement, object controller)
        {
            var routedEventHandlerBases = EnsureRoutedEventHandlerBases(rootElement);
            if (routedEventHandlerBases.ContainsKey(controller)) { return routedEventHandlerBases[controller]; }

            var routedEventHandlers = new RoutedEventHandlerBase();
            routedEventHandlerBases[controller] = routedEventHandlers;

            controller.GetType().GetFields(routedEventHandlerBindingFlags)
                .Where(field => field.GetCustomAttributes<RoutedEventHandlerAttribute>().Any())
                .ForEach(field => AddRoutedEventHandlers(field, rootElement, e => CreateRoutedEventHandler(field.GetValue(controller) as Delegate, e), routedEventHandlers));
            controller.GetType().GetProperties(routedEventHandlerBindingFlags)
                .Where(property => property.GetCustomAttributes<RoutedEventHandlerAttribute>().Any())
                .ForEach(property => AddRoutedEventHandlers(property, rootElement, e => CreateRoutedEventHandler(property.GetValue(controller, null) as Delegate, e), routedEventHandlers));
            controller.GetType().GetMethods(routedEventHandlerBindingFlags)
                .Where(method => method.GetCustomAttributes<RoutedEventHandlerAttribute>().Any())
                .ForEach(method => AddRoutedEventHandlers(method, rootElement, e => CreateRoutedEventHandler(method, e, controller), routedEventHandlers));

            return routedEventHandlers;
        }

        private IDictionary<object, RoutedEventHandlerBase> EnsureRoutedEventHandlerBases(FrameworkElement rootElement)
        {
            if (rootElement == null) { return new Dictionary<object, RoutedEventHandlerBase>(); }

            var routedEventHandlerBases = rootElement.GetValue(RoutedEventHandlerBasesProperty) as IDictionary<object, RoutedEventHandlerBase>;
            if (routedEventHandlerBases != null) { return routedEventHandlerBases; }

            routedEventHandlerBases = new Dictionary<object, RoutedEventHandlerBase>();
            rootElement.SetValue(RoutedEventHandlerBasesProperty, routedEventHandlerBases);
            return routedEventHandlerBases;
        }

        private Delegate CreateRoutedEventHandler(Delegate @delegate, RoutedEvent routedEvent)
        {
            return @delegate == null ? null : CreateRoutedEventHandler(@delegate.Method, routedEvent, @delegate.Target);
        }

        private Delegate CreateRoutedEventHandler(MethodInfo method, RoutedEvent routedEvent, object target)
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

        private void AddRoutedEventHandlers(MemberInfo member, FrameworkElement rootElement, Func<RoutedEvent, Delegate> createHandler, RoutedEventHandlerBase routedEventHandlers)
        {
            if (routedEventHandlers == null) { return; }

            member.GetCustomAttributes<RoutedEventHandlerAttribute>(true)
                .ForEach(routedEventHandler =>
                {
                    var element = rootElement.FindElement<FrameworkElement>(routedEventHandler.ElementName);
                    var routedEvent = RetrieveRoutedEvent(element, routedEventHandler.RoutedEvent);
                    routedEventHandlers.Add(
                        routedEventHandler.ElementName, element,
                        routedEventHandler.RoutedEvent, routedEvent,
                        createHandler(routedEvent), routedEventHandler.HandledEventsToo
                    );
                });
        }

        private RoutedEvent RetrieveRoutedEvent(FrameworkElement element, string name)
        {
            if (element == null) { return null; }

            return element.GetType()
                .GetFields(routedEventBindingFlags)
                .Where(field => field.Name == EnsureRoutedEventName(name))
                .Select(field => field.GetValue(element))
                .FirstOrDefault() as RoutedEvent;
        }

        private string EnsureRoutedEventName(string routedEventName)
        {
            return routedEventName != null && !routedEventName.EndsWith("Event") ? string.Format("{0}Event", routedEventName) : routedEventName;
        }
    }
}
