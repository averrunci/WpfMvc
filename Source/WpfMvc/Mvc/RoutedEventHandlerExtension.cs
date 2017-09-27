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
    /// Represents an extension to handle a routed event.
    /// </summary>
    internal sealed class RoutedEventHandlerExtension : IWpfControllerExtension
    {
        private static readonly BindingFlags routedEventHandlerBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        private static readonly BindingFlags routedEventBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

        private static readonly DependencyProperty RoutedEventHandlerBasesProperty = DependencyProperty.RegisterAttached(
            "ShadowRoutedEventHandlerBases", typeof(IDictionary<object, RoutedEventHandlerBase>), typeof(RoutedEventHandlerExtension), new PropertyMetadata(null)
        );

        void IWpfControllerExtension.Attach(FrameworkElement element, object controller)
        {
            if (element == null || controller == null) { return; }

            var routedEventHandlers = RetrieveRoutedEventHandlers(element, controller);
            routedEventHandlers.RegisterRoutedEventHandler();
            routedEventHandlers.GetBy(element.Name).From(element).Raise(nameof(FrameworkElement.DataContextChanged));
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

            var action = new RoutedEventHandlerAction(method, target);
            return action?.GetType()
                .GetMethod(nameof(RoutedEventHandlerAction.OnHandled))
                .CreateDelegate(routedEvent?.HandlerType ?? typeof(Handler), action);
        }

        private delegate void Handler(object sender, RoutedEventArgs e);

        public class RoutedEventHandlerAction
        {
            private MethodInfo Method { get; }
            private object Target { get; }

            public RoutedEventHandlerAction(MethodInfo method, object target)
            {
                Target = target;
                Method = method;
            }

            public void OnHandled(object sender, RoutedEventArgs e)
            {
                Handle(sender, e);
            }

            public object Handle(object sender, RoutedEventArgs e)
            {
                switch (Method.GetParameters().Length)
                {
                    case 0:
                        return Method.Invoke(Target, null);
                    case 1:
                        return Method.Invoke(Target, new object[] { e });
                    case 2:
                        return Method.Invoke(Target, new object[] { sender, e });
                    default:
                        throw new InvalidOperationException("The length of the method parameters must be less than 3.");
                }
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
            if (string.IsNullOrWhiteSpace(name)) { return null; }

            return name.Contains(attachedEventSeparator) ? RetrieveRoutedEvent(name) : RetrieveRoutedEvent(element.GetType(), name);
        }

        private string EnsureRoutedEventName(string routedEventName)
        {
            return routedEventName != null && !routedEventName.EndsWith("Event") ? string.Format("{0}Event", routedEventName) : routedEventName;
        }

        private const char attachedEventSeparator = '.';

        private RoutedEvent RetrieveRoutedEvent(string name)
        {
            var fields = name.Split(attachedEventSeparator);
            if (fields.Length != 2) { return null; }

            var elementName = fields[0];
            var routedEventName = fields[1];

            var elementType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.Name == elementName)
                .FirstOrDefault();

            return RetrieveRoutedEvent(elementType, routedEventName);
        }

        private RoutedEvent RetrieveRoutedEvent(Type elementType, string name)
        {
            return elementType?.GetFields(routedEventBindingFlags)
                .Where(field => field.Name == EnsureRoutedEventName(name))
                .Select(field => field.GetValue(null))
                .FirstOrDefault() as RoutedEvent;
        }
    }
}
