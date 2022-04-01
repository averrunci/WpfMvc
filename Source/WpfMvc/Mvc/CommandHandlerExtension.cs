// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents an extension to handle a command event.
/// </summary>
internal sealed class CommandHandlerExtension : IWpfControllerExtension
{
    private const BindingFlags CommandHandlerBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    private const BindingFlags RoutedEventBindingFlags = BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy;

    private static readonly Regex CommandHandlerNamingConventionRegex = new("^[^_]+_(?:Preview)?(?:Executed|CanExecute)(?:Async)?$", RegexOptions.Compiled);

    private static readonly DependencyProperty CommandHandlerBasesProperty = DependencyProperty.RegisterAttached(
        "ShadowCommandHandlerBases", typeof(IDictionary<object, CommandHandlerBase>), typeof(CommandHandlerExtension), new PropertyMetadata()
    );

    void IControllerExtension<FrameworkElement>.Attach(object controller, FrameworkElement element)
    {
        RetrieveCommandHandlers(element, controller).AddCommandHandler();
    }

    void IControllerExtension<FrameworkElement>.Detach(object controller, FrameworkElement element)
    {
        RetrieveCommandHandlers(element, controller).RemoveCommandHandler();
    }

    object IControllerExtension<FrameworkElement>.Retrieve(object controller)
        => RetrieveCommandHandlers(null, controller);

    private CommandHandlerBase RetrieveCommandHandlers(FrameworkElement? rootElement, object controller)
    {
        var commandHandlerBases = EnsureCommandHandlerBases(rootElement);
        if (commandHandlerBases.ContainsKey(controller)) { return commandHandlerBases[controller]; }

        var commandHandlers = new CommandHandlerBase();
        commandHandlerBases[controller] = commandHandlers;

        RetrieveCommandHandlersFromField(controller, rootElement, commandHandlers);
        RetrieveCommandHandlersFromProperty(controller, rootElement, commandHandlers);
        RetrieveCommandHandlersFromMethod(controller, rootElement, commandHandlers);
        RetrieveCommandHandlersFromMethodUsingNamingConvention(controller, rootElement, commandHandlers);

        return commandHandlers;
    }

    private void RetrieveCommandHandlersFromField(object controller, FrameworkElement? rootElement, CommandHandlerBase commandHandlers)
        => controller.GetType()
            .GetFields(CommandHandlerBindingFlags)
            .Where(field => field.GetCustomAttributes<CommandHandlerAttribute>().Any())
            .ForEach(field => AddCommandHandlers(field, rootElement, handlerType => CreateCommandHandler(field.GetValue(controller) as Delegate, handlerType), commandHandlers));

    private void RetrieveCommandHandlersFromProperty(object controller, FrameworkElement? rootElement, CommandHandlerBase commandHandlers)
        => controller.GetType()
            .GetProperties(CommandHandlerBindingFlags)
            .Where(property => property.GetCustomAttributes<CommandHandlerAttribute>().Any())
            .Where(property => property.CanRead)
            .ForEach(property => AddCommandHandlers(property, rootElement, handlerType => CreateCommandHandler(property.GetValue(controller, null) as Delegate, handlerType), commandHandlers));

    private void RetrieveCommandHandlersFromMethod(object controller, FrameworkElement? rootElement, CommandHandlerBase commandHandlers)
        => controller.GetType()
            .GetMethods(CommandHandlerBindingFlags)
            .Where(method => method.GetCustomAttributes<CommandHandlerAttribute>().Any())
            .ForEach(method => AddCommandHandlers(method, rootElement, handlerType => CreateCommandHandler(method, controller, handlerType), commandHandlers));

    private void RetrieveCommandHandlersFromMethodUsingNamingConvention(object controller, FrameworkElement? rootElement, CommandHandlerBase commandHandlers)
        => controller.GetType()
            .GetMethods(CommandHandlerBindingFlags)
            .Where(method => CommandHandlerNamingConventionRegex.IsMatch(method.Name))
            .Where(method => !method.GetCustomAttributes<CommandHandlerAttribute>(true).Any())
            .Where(method => !method.GetCustomAttributes<EventHandlerAttribute>(true).Any())
            .Select(method =>
            {
                var separatorIndex = method.Name.IndexOf("_", StringComparison.Ordinal);
                return new
                {
                    MethodInfo = method,
                    CommandHandlerAttribute = new CommandHandlerAttribute
                    {
                        CommandName = method.Name[..separatorIndex],
                        Event = EnsureEventNameUsingNamingConvention(method.Name[(separatorIndex + 1)..])
                    }
                };
            })
            .ForEach(x => AddCommandHandler(x.CommandHandlerAttribute, rootElement, handlerType => CreateCommandHandler(x.MethodInfo, controller, handlerType), commandHandlers));

    private string EnsureEventNameUsingNamingConvention(string eventName) => eventName.EndsWith("Async") ? eventName[..^5] : eventName;

    private IDictionary<object, CommandHandlerBase> EnsureCommandHandlerBases(FrameworkElement? rootElement)
    {
        if (rootElement is null) return new Dictionary<object, CommandHandlerBase>();

        if (rootElement.GetValue(CommandHandlerBasesProperty) is IDictionary<object, CommandHandlerBase> commandHandlerBases) return commandHandlerBases;

        commandHandlerBases = new Dictionary<object, CommandHandlerBase>();
        rootElement.SetValue(CommandHandlerBasesProperty, commandHandlerBases);
        return commandHandlerBases;
    }

    private Delegate? CreateCommandHandler(Delegate? @delegate, Type handlerType) => @delegate is null ? null : CreateCommandHandler(@delegate.Method, @delegate.Target, handlerType);

    private Delegate? CreateCommandHandler(MethodInfo method, object? target, Type handlerType)
    {
        var action = new WpfEventHandlerAction(method, target);
        return action.GetType()
            .GetMethod(nameof(EventHandlerAction.OnHandled))
            ?.CreateDelegate(handlerType, action);
    }

    private void AddCommandHandlers(MemberInfo member, FrameworkElement? rootElement, Func<Type, Delegate?> handlerCreator, CommandHandlerBase commandHandlers)
    {
        member.GetCustomAttributes<CommandHandlerAttribute>(true)
            .ForEach(commandHandler => AddCommandHandler(commandHandler, rootElement, handlerCreator, commandHandlers));
    }

    private void AddCommandHandler(CommandHandlerAttribute commandHandler, FrameworkElement? rootElement, Func<Type, Delegate?> handlerCreator, CommandHandlerBase commandHandlers)
    {
        if (rootElement is null)
        {
            AddCommandHandler(commandHandler, null, rootElement, handlerCreator, commandHandlers);
        }
        else
        {
            FindCommand(rootElement, commandHandler.CommandName)
                .ForEach(command => AddCommandHandler(commandHandler, command, rootElement, handlerCreator, commandHandlers));
        }
    }

    private void AddCommandHandler(CommandHandlerAttribute commandHandler, ICommand? command, FrameworkElement? rootElement, Func<Type, Delegate?> handlerCreator, CommandHandlerBase commandHandlers)
    {
        var routedEvent = RetrieveRoutedEvent(commandHandler.Event);
        if (routedEvent is null) return;

        commandHandlers.Add(commandHandler.CommandName, commandHandler.Event, command, rootElement, handlerCreator(routedEvent.HandlerType));
    }

    private RoutedEvent? RetrieveRoutedEvent(string name)
        => typeof(CommandManager).GetFields(RoutedEventBindingFlags)
            .Where(field => field.Name == EnsureRoutedEventName(name))
            .Select(field => field.GetValue(null))
            .FirstOrDefault() as RoutedEvent;
    private string EnsureRoutedEventName(string routedEventName)
        => routedEventName.EndsWith("Event") ? routedEventName : $"{routedEventName}Event";

    private IEnumerable<ICommand> FindCommand(DependencyObject element, string commandName)
    {
        foreach (var child in LogicalTreeHelper.GetChildren(element))
        {
            if (child is not DependencyObject childElement) yield break;

            var commandProperty = childElement.GetType().GetProperties().FirstOrDefault(p => typeof(ICommand).IsAssignableFrom(p.PropertyType));
            if (commandProperty?.GetValue(child) is RoutedCommand routedCommand && routedCommand.Name == commandName) yield return routedCommand;

            foreach (var command in FindCommand(childElement, commandName))
            {
                yield return command;
            }
        }
    }
}