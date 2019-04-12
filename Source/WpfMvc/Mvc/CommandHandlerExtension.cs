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
using System.Windows.Input;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents an extension to handle a command event.
    /// </summary>
    internal sealed class CommandHandlerExtension : IWpfControllerExtension
    {
        private static readonly BindingFlags CommandHandlerBindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static readonly Regex CommandHandlerNamingConventionRegex = new Regex("^[^_]+_(?:Executed|CanExecute)$", RegexOptions.Compiled);

        private static readonly DependencyProperty CommandHandlerBasesProperty = DependencyProperty.RegisterAttached(
            "ShadowCommandHandlerBases", typeof(IDictionary<object, CommandHandlerBase>), typeof(CommandHandlerExtension), new PropertyMetadata(null)
        );

        void IControllerExtension<FrameworkElement>.Attach(object controller, FrameworkElement element)
        {
            if (element == null || controller == null) return;

            RetrieveCommandHandlers(element, controller).AddCommandHandler();
        }

        void IControllerExtension<FrameworkElement>.Detach(object controller, FrameworkElement element)
        {
            if (element == null || controller == null) return;

            RetrieveCommandHandlers(element, controller).RemoveCommandHandler();
        }

        object IControllerExtension<FrameworkElement>.Retrieve(object controller)
            => controller == null ? new CommandHandlerBase() : RetrieveCommandHandlers(null, controller);

        private CommandHandlerBase RetrieveCommandHandlers(FrameworkElement rootElement, object controller)
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

        private void RetrieveCommandHandlersFromField(object controller, FrameworkElement rootElement, CommandHandlerBase commandHandlers)
            => controller.GetType()
                .GetFields(CommandHandlerBindingFlags)
                .Where(field => field.GetCustomAttributes<CommandHandlerAttribute>().Any())
                .ForEach(field => AddCommandHandlers(field, rootElement, CreateCommandHandler(field.GetValue(controller) as Delegate), commandHandlers));

        private void RetrieveCommandHandlersFromProperty(object controller, FrameworkElement rootElement, CommandHandlerBase commandHandlers)
            => controller.GetType()
                .GetProperties(CommandHandlerBindingFlags)
                .Where(property => property.GetCustomAttributes<CommandHandlerAttribute>().Any())
                .ForEach(property => AddCommandHandlers(property, rootElement, CreateCommandHandler(property.GetValue(controller, null) as Delegate), commandHandlers));

        private void RetrieveCommandHandlersFromMethod(object controller, FrameworkElement rootElement, CommandHandlerBase commandHandlers)
            => controller.GetType()
                .GetMethods(CommandHandlerBindingFlags)
                .Where(method => method.GetCustomAttributes<CommandHandlerAttribute>().Any())
                .ForEach(method => AddCommandHandlers(method, rootElement, CreateCommandHandler(method, controller), commandHandlers));

        private void RetrieveCommandHandlersFromMethodUsingNamingConvention(object controller, FrameworkElement rootElement, CommandHandlerBase commandHandlers)
            => controller.GetType()
                .GetMethods(CommandHandlerBindingFlags)
                .Where(method => CommandHandlerNamingConventionRegex.IsMatch(method.Name))
                .Where(method => !method.GetCustomAttributes<CommandHandlerAttribute>(true).Any())
                .Where(method => !method.GetCustomAttributes<EventHandlerAttribute>(true).Any())
                .Select(method => new
                {
                    MethodInfo = method,
                    CommandHandlerAttribute = new CommandHandlerAttribute { CommandName = method.Name.Substring(0, method.Name.IndexOf("_", StringComparison.Ordinal)) }
                })
                .ForEach(x => AddCommandHandler(x.CommandHandlerAttribute, x.MethodInfo, rootElement, CreateCommandHandler(x.MethodInfo, controller), commandHandlers));

        private IDictionary<object, CommandHandlerBase> EnsureCommandHandlerBases(FrameworkElement rootElement)
        {
            if (rootElement == null) { return new Dictionary<object, CommandHandlerBase>(); }

            if (rootElement.GetValue(CommandHandlerBasesProperty) is IDictionary<object, CommandHandlerBase> commandHandlerBases) return commandHandlerBases;

            commandHandlerBases = new Dictionary<object, CommandHandlerBase>();
            rootElement.SetValue(CommandHandlerBasesProperty, commandHandlerBases);
            return commandHandlerBases;
        }

        private Delegate CreateCommandHandler(Delegate @delegate)
            => @delegate == null ? null : CreateCommandHandler(@delegate.Method, @delegate.Target);

        private Delegate CreateCommandHandler(MethodInfo method, object target)
        {
            if (method == null) return null;

            var parameters = method.GetParameters();
            switch (parameters.Length)
            {
                case 1:
                    if (parameters[0].ParameterType == typeof(ExecutedRoutedEventArgs))
                    {
                        return CreateCommandHandler<ExecutedRoutedEventArgs, ExecutedRoutedEventHandler>(method, target);
                    }
                    else if (parameters[0].ParameterType == typeof(CanExecuteRoutedEventArgs))
                    {
                        return CreateCommandHandler<CanExecuteRoutedEventArgs, CanExecuteRoutedEventHandler>(method, target);
                    }
                    else
                    {
                        throw new InvalidOperationException($"The type of the parameter must be {typeof(ExecutedRoutedEventArgs)} or {typeof(CanExecuteRoutedEventArgs)}.");
                    }
                case 2:
                    if (parameters[1].ParameterType == typeof(ExecutedRoutedEventArgs))
                    {
                        return CreateCommandHandler<ExecutedRoutedEventArgs, ExecutedRoutedEventHandler>(method, target);
                    }
                    else if (parameters[1].ParameterType == typeof(CanExecuteRoutedEventArgs))
                    {
                        return CreateCommandHandler<CanExecuteRoutedEventArgs, CanExecuteRoutedEventHandler>(method, target);
                    }
                    else
                    {
                        throw new InvalidOperationException($"The type of the second parameter must be {typeof(ExecutedRoutedEventArgs)} or {typeof(CanExecuteRoutedEventArgs)}.");
                    }
                default:
                    throw new InvalidOperationException("The length of the method parameters must be 1 or 2.");
            }
        }
        
        private Delegate CreateCommandHandler<TRoutedEventArgs, THandler>(MethodInfo method, object target) where TRoutedEventArgs : RoutedEventArgs
        {
            var action = new RoutedEventHandlerAction<TRoutedEventArgs>(method, target);
            return action.GetType()
                .GetMethod(nameof(RoutedEventHandlerAction<TRoutedEventArgs>.OnHandled))
                ?.CreateDelegate(typeof(THandler), action);
        }

        public class RoutedEventHandlerAction<T> where T : RoutedEventArgs
        {
            private readonly MethodInfo method;
            private readonly object target;

            public RoutedEventHandlerAction(MethodInfo method, object target)
            {
                this.target = target;
                this.method = method;
            }

            public void OnHandled(object sender, T e) => Handle(sender, e);

            public object Handle(object sender, T e)
            {
                switch (method.GetParameters().Length)
                {
                    case 1: return method.Invoke(target, new object[] { e });
                    case 2: return method.Invoke(target, new[] { sender, e });
                    default: throw new InvalidOperationException("The length of the method parameters must be 1 or 2.");
                }
            }
        }

        private void AddCommandHandlers(MemberInfo member, FrameworkElement rootElement, Delegate handler, CommandHandlerBase commandHandlers)
        {
            if (commandHandlers == null) return;

            member.GetCustomAttributes<CommandHandlerAttribute>(true)
                .ForEach(commandHandler => AddCommandHandler(commandHandler, member, rootElement, handler, commandHandlers));
        }

        private void AddCommandHandler(CommandHandlerAttribute commandHandler, MemberInfo member, FrameworkElement rootElement, Delegate handler, CommandHandlerBase commandHandlers)
        {
            rootElement.IfAbsent(() => AddCommandHandler(commandHandler.CommandName, null, rootElement, handler, commandHandlers));
            rootElement.IfPresent(_ => FindCommand(rootElement, commandHandler.CommandName)
                .ForEach(command => AddCommandHandler(commandHandler.CommandName, command, rootElement, handler, commandHandlers))
            );
        }

        private void AddCommandHandler(string commandName, ICommand command, FrameworkElement rootElement, Delegate handler, CommandHandlerBase commandHandlers)
        {
            (handler as ExecutedRoutedEventHandler).IfPresent(executedHandler => commandHandlers.Add(commandName, command, rootElement, executedHandler));
            (handler as CanExecuteRoutedEventHandler).IfPresent(canExecuteHandler => commandHandlers.Add(commandName, command, rootElement, canExecuteHandler));
        }

        private IEnumerable<ICommand> FindCommand(DependencyObject element, string commandName)
        {
            if (element == null) yield break;

            foreach (var child in LogicalTreeHelper.GetChildren(element))
            {
                var childElement = child as DependencyObject;
                if (childElement == null) { yield break; }

                var commandProperty = childElement.GetType().GetProperties().FirstOrDefault(p => typeof(ICommand).IsAssignableFrom(p.PropertyType));
                if (commandProperty != null && commandProperty.GetValue(child) is RoutedCommand routedCommand && routedCommand.Name == commandName) yield return routedCommand;

                foreach (var command in FindCommand(childElement, commandName))
                {
                    yield return command;
                }
            }
        }
    }
}
