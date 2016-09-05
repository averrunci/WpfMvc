// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Represents an extension to handle a command event.
    /// </summary>
    public sealed class CommandHandlerExtension : IWpfControllerExtension
    {
        private static readonly BindingFlags commandHandlerBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static readonly DependencyProperty CommandHandlerBasesProperty = DependencyProperty.RegisterAttached(
            "ShadowCommandHandlerBases", typeof(IDictionary<object, CommandHandlerBase>), typeof(CommandHandlerExtension), new PropertyMetadata(null)
        );

        void IWpfControllerExtension.Attach(FrameworkElement element, object controller)
        {
            if (element == null || controller == null) { return; }

            RetrieveCommandHandlers(element, controller).RegisterCommandHandler();
        }

        void IWpfControllerExtension.Detach(FrameworkElement element, object controller)
        {
            if (element == null || controller == null) { return; }

            RetrieveCommandHandlers(element, controller).UnregisterCommandHandler();
        }

        object IWpfControllerExtension.Retrieve(object controller)
        {
            return controller == null ? new CommandHandlerBase() : RetrieveCommandHandlers(null, controller);
        }

        private CommandHandlerBase RetrieveCommandHandlers(FrameworkElement rootElement, object controller)
        {
            var commandHandlerBases = EnsureCommandHandlerBases(rootElement);
            if (commandHandlerBases.ContainsKey(controller)) { return commandHandlerBases[controller]; }

            var commandHandlers = new CommandHandlerBase();
            commandHandlerBases[controller] = commandHandlers;

            controller.GetType().GetFields(commandHandlerBindingFlags)
                .Where(field => field.GetCustomAttributes<CommandHandlerAttribute>().Any())
                .ForEach(field => AddCommandHandlers(field, rootElement, CreateCommandHandler(field.GetValue(controller) as Delegate), commandHandlers));
            controller.GetType().GetProperties(commandHandlerBindingFlags)
                .Where(property => property.GetCustomAttributes<CommandHandlerAttribute>().Any())
                .ForEach(property => AddCommandHandlers(property, rootElement, CreateCommandHandler(property.GetValue(controller, null) as Delegate), commandHandlers));
            controller.GetType().GetMethods(commandHandlerBindingFlags)
                .Where(method => method.GetCustomAttributes<CommandHandlerAttribute>().Any())
                .ForEach(method => AddCommandHandlers(method, rootElement, CreateCommandHandler(method, controller), commandHandlers));

            return commandHandlers;
        }

        private IDictionary<object, CommandHandlerBase> EnsureCommandHandlerBases(FrameworkElement rootElement)
        {
            if (rootElement == null) { return new Dictionary<object, CommandHandlerBase>(); }

            var commandHandlerBases = rootElement.GetValue(CommandHandlerBasesProperty) as IDictionary<object, CommandHandlerBase>;
            if (commandHandlerBases != null) { return commandHandlerBases; }

            commandHandlerBases = new Dictionary<object, CommandHandlerBase>();
            rootElement.SetValue(CommandHandlerBasesProperty, commandHandlerBases);
            return commandHandlerBases;
        }

        private Delegate CreateCommandHandler(Delegate @delegate)
        {
            return @delegate == null ? null : CreateCommandHandler(@delegate.Method, @delegate.Target);
        }

        private Delegate CreateCommandHandler(MethodInfo method, object target)
        {
            if (method == null) { return null; }

            var paramters = method.GetParameters();
            switch (paramters.Length)
            {
                case 1:
                    if (paramters[0].ParameterType == typeof(ExecutedRoutedEventArgs))
                    {
                        return new ExecutedRoutedEventHandler((s, e) => method.Invoke(target, new object[] { e }));
                    }
                    else if (paramters[0].ParameterType == typeof(CanExecuteRoutedEventArgs))
                    {
                        return new CanExecuteRoutedEventHandler((s, e) => method.Invoke(target, new object[] { e }));
                    }
                    else
                    {
                        throw new InvalidOperationException($"The type of the parameter must be {typeof(ExecutedRoutedEventArgs)} or {typeof(CanExecuteRoutedEventArgs)}.");
                    }
                case 2:
                    if (paramters[1].ParameterType == typeof(ExecutedRoutedEventArgs))
                    {
                        return new ExecutedRoutedEventHandler((s, e) => method.Invoke(target, new object[] { s, e }));
                    }
                    else if (paramters[1].ParameterType == typeof(CanExecuteRoutedEventArgs))
                    {
                        return new CanExecuteRoutedEventHandler((s, e) => method.Invoke(target, new object[] { s, e }));
                    }
                    else
                    {
                        throw new InvalidOperationException($"The type of the second parameter must be {typeof(ExecutedRoutedEventArgs)} or {typeof(CanExecuteRoutedEventArgs)}.");
                    }
                default:
                    throw new InvalidOperationException("The length of the method parameters must be 1 or 2.");
            }
        }

        private void AddCommandHandlers(MemberInfo member, FrameworkElement rootElement, Delegate handler, CommandHandlerBase commandHandlers)
        {
            if (commandHandlers == null) { return; }

            member.GetCustomAttributes<CommandHandlerAttribute>(true)
                .ForEach(commandHandler =>
                {
                    rootElement.IfAbsent(() =>
                        AddCommandHandler(commandHandler.CommandName, null, rootElement, handler, commandHandlers));
                    rootElement.IfPresent(_ =>
                        FindCommand(rootElement).ForEach(command =>
                            AddCommandHandler(commandHandler.CommandName, command, rootElement, handler, commandHandlers)
                        ));
                });
        }

        private void AddCommandHandler(string commandName, ICommand command, FrameworkElement rootElement, Delegate handler, CommandHandlerBase commandHandlers)
        {
            (handler as ExecutedRoutedEventHandler).IfPresent(executedHandler =>
                commandHandlers.Add(commandName, command, rootElement, executedHandler));
            (handler as CanExecuteRoutedEventHandler).IfPresent(canExecuteHandler =>
                commandHandlers.Add(commandName, command, rootElement, canExecuteHandler));
        }

        private IEnumerable<ICommand> FindCommand(DependencyObject element)
        {
            if (element == null) { yield break; }

            foreach (var child in LogicalTreeHelper.GetChildren(element))
            {
                var childElement = child as DependencyObject;
                if (childElement == null) { yield break; }

                var commandProperty = childElement.GetType().GetProperties().Where(p => typeof(ICommand).IsAssignableFrom(p.PropertyType)).FirstOrDefault();
                if (commandProperty != null)
                {
                    var command = commandProperty.GetValue(child) as ICommand;
                    if (command != null) { yield return command; }
                }

                foreach (var command in FindCommand(childElement))
                {
                    yield return command;
                }
            }
        }
    }
}
