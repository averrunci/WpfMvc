// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfEventHandlerExtension : EventHandlerExtension<FrameworkElement, WpfEventHandlerItem>, IWpfControllerExtension
{
    private const BindingFlags RoutedEventBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;
    private const char AttachedEventSeparator = '.';

    private static readonly DependencyProperty EventHandlerBasesProperty = DependencyProperty.RegisterAttached(
        "ShadowEventHandlerBases", typeof(IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>), typeof(WpfEventHandlerExtension), new PropertyMetadata(default(IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>))
    );

    public WpfEventHandlerExtension()
    {
        Add<WpfEventHandlerParameterFromDIResolver>();
        Add<WpfEventHandlerParameterFromElementResolver>();
        Add<WpfEventHandlerParameterFromDataContextResolver>();
    }

    protected override IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>> EnsureEventHandlerBases(FrameworkElement? element)
    {
        if (element is null) return new Dictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>();

        if (element.GetValue(EventHandlerBasesProperty) is IDictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>> eventHandlerBases) return eventHandlerBases;

        eventHandlerBases = new Dictionary<object, EventHandlerBase<FrameworkElement, WpfEventHandlerItem>>();
        element.SetValue(EventHandlerBasesProperty, eventHandlerBases);
        return eventHandlerBases;
    }

    protected override void AddEventHandler(FrameworkElement? element, EventHandlerAttribute eventHandlerAttribute, Func<Type?, Delegate?> handlerCreator, EventHandlerBase<FrameworkElement, WpfEventHandlerItem> eventHandlers)
    {
        var targetElement = element.FindElement<FrameworkElement>(eventHandlerAttribute.ElementName);

        if (EnsureRoutedEventName(eventHandlerAttribute.Event) == EnsureRoutedEventName(nameof(FrameworkElement.DataContextChanged)))
        {
            eventHandlers.Add(new DataContextChangedEventHandlerItem(
                eventHandlerAttribute.ElementName, targetElement,
                eventHandlerAttribute.Event, handlerCreator(typeof(DependencyPropertyChangedEventHandler)), eventHandlerAttribute.HandledEventsToo,
                CreateParameterResolver(element)
            ));
            return;
        }

        var routedEvent = RetrieveRoutedEvent(targetElement, eventHandlerAttribute.Event);
        var eventInfo = RetrieveEventInfo(element, eventHandlerAttribute.Event);
        eventHandlers.Add(new WpfEventHandlerItem(
            eventHandlerAttribute.ElementName, targetElement,
            eventHandlerAttribute.Event, routedEvent, eventInfo,
            handlerCreator(routedEvent?.HandlerType ?? eventInfo?.EventHandlerType), eventHandlerAttribute.HandledEventsToo,
            CreateParameterResolver(element)
        ));
    }

    protected override bool FilterMethodUsingNamingConvention(MethodInfo method)
        => base.FilterMethodUsingNamingConvention(method) && !method.GetCustomAttributes<CommandHandlerAttribute>(true).Any();

    protected override Delegate? CreateEventHandler(MethodInfo method, object? target, Type? handlerType, FrameworkElement? element)
    {
        if (handlerType != typeof(DependencyPropertyChangedEventHandler)) return base.CreateEventHandler(method, target, handlerType, element);

        var action = new DataContextChangedEventHandlerAction(method, target, CreateParameterDependencyResolver(CreateParameterResolver(element)));
        return action.GetType()
            .GetMethod(nameof(DataContextChangedEventHandlerAction.OnHandled))
            ?.CreateDelegate(handlerType, action);
    }

    protected override EventHandlerAction CreateEventHandlerAction(MethodInfo method, object? target, FrameworkElement? element)
        => new WpfEventHandlerAction(method, target, CreateParameterDependencyResolver(CreateParameterResolver(element)));

    protected override void OnEventHandlerAdded(EventHandlerBase<FrameworkElement, WpfEventHandlerItem> eventHandlers, FrameworkElement element)
    {
        base.OnEventHandlerAdded(eventHandlers, element);

        eventHandlers.GetBy(element.Name)
            .From(element)
            .With(new DependencyPropertyChangedEventArgs(FrameworkElement.DataContextProperty, null, element.DataContext))
            .Raise(nameof(FrameworkElement.DataContextChanged));
    }

    private RoutedEvent? RetrieveRoutedEvent(FrameworkElement? element, string name)
    {
        if (element is null || string.IsNullOrWhiteSpace(name)) return null;

        return name.Contains(AttachedEventSeparator) ? RetrieveRoutedEvent(name) : RetrieveRoutedEvent(element.GetType(), name);
    }

    private RoutedEvent? RetrieveRoutedEvent(string name)
    {
        var fields = name.Split(AttachedEventSeparator);
        if (fields.Length != 2) return null;

        var elementName = fields[0];
        var routedEventName = fields[1];

        var elementType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .FirstOrDefault(type => type.Name == elementName);

        return elementType is null ? null : RetrieveRoutedEvent(elementType, routedEventName);
    }

    private RoutedEvent? RetrieveRoutedEvent(Type elementType, string name)
        => elementType.GetFields(RoutedEventBindingFlags)
            .Where(field => field.Name == EnsureRoutedEventName(name))
            .Select(field => field.GetValue(null))
            .FirstOrDefault() as RoutedEvent;

    private string EnsureRoutedEventName(string routedEventName)
        => routedEventName.EndsWith("Event") ? routedEventName : $"{routedEventName}Event";

    private EventInfo? RetrieveEventInfo(FrameworkElement? element, string name)
        => element?.GetType()
            .GetEvents()
            .FirstOrDefault(e => e.Name == name);
}