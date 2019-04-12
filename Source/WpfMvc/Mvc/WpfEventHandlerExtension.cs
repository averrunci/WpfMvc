// Copyright (C) 2018-2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;

namespace Charites.Windows.Mvc
{
    internal sealed class WpfEventHandlerExtension : EventHandlerExtension<FrameworkElement, WpfEventHandlerItem>, IWpfControllerExtension
    {
        private const BindingFlags RoutedEventBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
        private const char AttachedEventSeparator = '.';

        private static readonly Regex EventHandlerNamingConventionRegex = new Regex("^[^_]+_[^_]+$", RegexOptions.Compiled);

        private static readonly DependencyProperty EventHandlerBasesProperty = DependencyProperty.RegisterAttached(
            "ShadowEventHandlerBases", typeof(IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>), typeof(WpfEventHandlerExtension), new PropertyMetadata(default(IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>))
        );

        protected override IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>> EnsureEventHandlerBases(FrameworkElement element)
        {
            if (element == null) return new Dictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>();

            if (element.GetValue(EventHandlerBasesProperty) is IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>> eventHandlerBases) return eventHandlerBases;

            eventHandlerBases = new Dictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>();
            element.SetValue(EventHandlerBasesProperty, eventHandlerBases);
            return eventHandlerBases;
        }

        protected override void AddEventHandler(FrameworkElement element, EventHandlerAttribute eventHandlerAttribute, Func<Type, Delegate> handlerCreator, EventHandlerBase<FrameworkElement, WpfEventHandlerItem> eventHandlers)
        {
            var targetElement = element.FindElement<FrameworkElement>(eventHandlerAttribute.ElementName);

            if (EnsureRoutedEventName(eventHandlerAttribute.Event) == EnsureRoutedEventName(nameof(FrameworkElement.DataContextChanged)))
            {
                eventHandlers.Add(new DataContextChangedEventHandlerItem(
                    eventHandlerAttribute.ElementName, targetElement,
                    eventHandlerAttribute.Event, handlerCreator(typeof(DependencyPropertyChangedEventHandler)), eventHandlerAttribute.HandledEventsToo)
                );
                return;
            }

            var routedEvent = RetrieveRoutedEvent(targetElement, eventHandlerAttribute.Event);
            var eventInfo = RetrieveEventInfo(element, eventHandlerAttribute.Event);
            eventHandlers.Add(new WpfEventHandlerItem(
                eventHandlerAttribute.ElementName, targetElement,
                eventHandlerAttribute.Event, routedEvent, eventInfo,
                handlerCreator(routedEvent?.HandlerType ?? eventInfo?.EventHandlerType), eventHandlerAttribute.HandledEventsToo
            ));
        }

        protected override void RetrieveEventHandlersFromMethodUsingNamingConvention(object controller, FrameworkElement element, EventHandlerBase<FrameworkElement, WpfEventHandlerItem> eventHandlers)
            => controller.GetType()
                .GetMethods(EventHandlerBindingFlags)
                .Where(method => EventHandlerNamingConventionRegex.IsMatch(method.Name))
                .Where(method => !method.Name.StartsWith("get_"))
                .Where(method => !method.Name.StartsWith("set_"))
                .Where(method => !method.GetCustomAttributes<EventHandlerAttribute>(true).Any())
                .Where(method => !method.GetCustomAttributes<CommandHandlerAttribute>(true).Any())
                .Select(method =>
                {
                    var separatorIndex = method.Name.IndexOf("_", StringComparison.Ordinal);
                    return new
                    {
                        MethodInfo = method,
                        EventHanndlerAttribute = new EventHandlerAttribute
                        {
                            ElementName = method.Name.Substring(0, separatorIndex),
                            Event = method.Name.Substring(separatorIndex + 1)
                        }
                    };
                })
                .ForEach(x => AddEventHandler(element, x.EventHanndlerAttribute, handlerType => CreateEventHandler(x.MethodInfo, controller, handlerType), eventHandlers));

        protected override Delegate CreateEventHandler(MethodInfo method, object target, Type handlerType)
        {
            if (handlerType != typeof(DependencyPropertyChangedEventHandler)) return base.CreateEventHandler(method, target, handlerType);
            if (method == null) return null;

            var action = new DataContextChangedEventHandlerItem.EventHandlerAction(method, target);
            return action.GetType()
                .GetMethod(nameof(DataContextChangedEventHandlerItem.EventHandlerAction.OnHandled))
                ?.CreateDelegate(handlerType, action);
        }

        protected override EventHandlerAction CreateEventHandlerAction(MethodInfo method, object target) => new WpfEventHandlerAction(method, target);

        protected override void OnEventHandlerAdded(EventHandlerBase<FrameworkElement, WpfEventHandlerItem> eventHandlers, FrameworkElement element)
        {
            base.OnEventHandlerAdded(eventHandlers, element);

            eventHandlers.GetBy(element.Name).From(element).With(new DependencyPropertyChangedEventArgs(FrameworkElement.DataContextProperty, null, element.DataContext)).Raise(nameof(FrameworkElement.DataContextChanged));
        }

        private RoutedEvent RetrieveRoutedEvent(FrameworkElement element, string name)
        {
            if (element == null || string.IsNullOrWhiteSpace(name)) return null;

            return name.Contains(AttachedEventSeparator) ? RetrieveRoutedEvent(name) : RetrieveRoutedEvent(element.GetType(), name);
        }

        private RoutedEvent RetrieveRoutedEvent(string name)
        {
            var fields = name.Split(AttachedEventSeparator);
            if (fields.Length != 2) return null;

            var elementName = fields[0];
            var routedEventName = fields[1];

            var elementType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(type => type.Name == elementName);

            return RetrieveRoutedEvent(elementType, routedEventName);
        }

        private RoutedEvent RetrieveRoutedEvent(Type elementType, string name)
            => elementType.GetFields(RoutedEventBindingFlags)
                .Where(field => field.Name == EnsureRoutedEventName(name))
                .Select(field => field.GetValue(null))
                .FirstOrDefault() as RoutedEvent;

        private string EnsureRoutedEventName(string routedEventName)
            => routedEventName != null && !routedEventName.EndsWith("Event") ? $"{routedEventName}Event" : routedEventName;

        private EventInfo RetrieveEventInfo(FrameworkElement element, string name)
            => element?.GetType()
                .GetEvents()
                .FirstOrDefault(e => e.Name == name);
    }
}
