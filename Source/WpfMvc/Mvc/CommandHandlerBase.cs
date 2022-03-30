﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents the base of command handlers.
/// </summary>
public class CommandHandlerBase
{
    private readonly ICollection<Item> items = new Collection<Item>();

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerBase"/> class.
    /// </summary>
    public CommandHandlerBase()
    {
    }

    /// <summary>
    /// Adds a command handler to the <see cref="CommandHandlerBase"/>.
    /// </summary>
    /// <param name="commandName">The name of the command.</param>
    /// <param name="command">The command that is bound.</param>
    /// <param name="element">The element that binds the command.</param>
    /// <param name="executedHandler">The command Executed handler.</param>
    public void Add(string commandName, ICommand? command, FrameworkElement? element, ExecutedRoutedEventHandler executedHandler)
    {
        var item = items.FirstOrDefault(i => i.CommandName == commandName && i.Element == element);
        if (item is null)
        {
            items.Add(new Item(commandName, command, element, executedHandler));
        }
        else
        {
            item.ExecutedHandler = executedHandler;
        }
    }

    /// <summary>
    /// Adds a command handler to the <see cref="CommandHandlerBase"/>.
    /// </summary>
    /// <param name="commandName">The name of the command.</param>
    /// <param name="command">The command that is bound.</param>
    /// <param name="element">The element that binds the command.</param>
    /// <param name="canExecuteHandler">The command CanExecute handler.</param>
    public void Add(string commandName, ICommand? command, FrameworkElement? element, CanExecuteRoutedEventHandler canExecuteHandler)
    {
        var item = items.FirstOrDefault(i => i.CommandName == commandName && i.Element == element);
        if (item is null)
        {
            items.Add(new Item(commandName, command, element, canExecuteHandler));
        }
        else
        {
            item.CanExecuteHandler = canExecuteHandler;
        }
    }

    /// <summary>
    /// Removes all routed event handlers from the <see cref="CommandHandlerBase"/>.
    /// </summary>
    public void Clear()
    {
        items.Clear();
    }

    /// <summary>
    /// Gets command handlers by the specified name of the command.
    /// </summary>
    /// <param name="commandName">The name of the command that has command event handlers.</param>
    /// <returns>
    /// <see cref="Executor"/> that raises command event.
    /// </returns>
    public Executor GetBy(string commandName) => new(items.Where(i => i.CommandName == commandName));

    /// <summary>
    /// Adds command handlers to the element.
    /// </summary>
    public void AddCommandHandler() => items.ForEach(i => i.AddCommandHandler());

    /// <summary>
    /// Removes command handlers from the element.
    /// </summary>
    public void RemoveCommandHandler() => items.ForEach(i => i.RemoveCommandHandler());

    /// <summary>
    /// Provides command events execution functions.
    /// </summary>
    public sealed class Executor
    {
        private readonly IEnumerable<Item> items;
        private object? sender;
        private ICommand? command;
        private readonly IDictionary<Type, Func<object?>> dependencyResolver = new Dictionary<Type, Func<object?>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Executor"/> class,
        /// using the supplied items.
        /// </summary>
        /// <param name="items">The command event handler items.</param>
        public Executor(IEnumerable<Item> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Sets the object where the event handler is attached.
        /// </summary>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <returns>
        /// The instance of the <see cref="Executor"/> class.
        /// </returns>
        public Executor From(object? sender)
        {
            this.sender = sender;
            return this;
        }

        /// <summary>
        /// Sets the command that is executed.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <returns>
        /// The instance of the <see cref="Executor"/> class.
        /// </returns>
        public Executor With(ICommand? command)
        {
            this.command = command;
            return this;
        }

        /// <summary>
        /// Resolves a parameter of the specified type using the specified resolver.
        /// </summary>
        /// <typeparam name="T">The type of the parameter to inject to.</typeparam>
        /// <param name="resolver">The function to resolve the parameter of the specified type.</param>
        /// <returns>
        /// The instance of the <see cref="Executor"/> class.
        /// </returns>
        public Executor Resolve<T>(Func<object?> resolver)
        {
            dependencyResolver[typeof(T)] = resolver;
            return this;
        }

        /// <summary>
        /// Raises the command Executed event using the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        public void RaiseExecuted(object? parameter = null) => items.ForEach(i => i.RaiseExecuted(command, sender, parameter, dependencyResolver));

        /// <summary>
        /// Raises the command CanExecute event using the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
        public IEnumerable<CanExecuteRoutedEventArgs> RaiseCanExecute(object? parameter = null)
            => items.Select(item => item.RaiseCanExecute(command, sender, parameter, dependencyResolver))
                .OfType<CanExecuteRoutedEventArgs>()
                .ToList()
                .AsReadOnly();

        /// <summary>
        /// Raises the command Executed event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task RaiseExecutedAsync(object? parameter = null)
        {
            foreach (var item in items)
            {
                await item.RaiseExecutedAsync(command, sender, parameter, dependencyResolver);
            }
        }

        /// <summary>
        /// Raises the command CanExecute event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task<IEnumerable<CanExecuteRoutedEventArgs>> RaiseCanExecuteAsync(object? parameter = null)
        {
            var args = new List<CanExecuteRoutedEventArgs>();
            foreach (var item in items)
            {
                var e = await item.RaiseCanExecuteAsync(command, sender, parameter, dependencyResolver);
                if (e is not null) args.Add(e);
            }
            return args.AsReadOnly();
        }
    }

    /// <summary>
    /// Represents an item of a command event handler.
    /// </summary>
    public sealed class Item
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// Gets the command.
        /// </summary>
        public ICommand? Command { get; }

        /// <summary>
        /// Gets the element that has command event handlers.
        /// </summary>
        public FrameworkElement? Element { get; }

        /// <summary>
        /// Gets the command Executed event handler.
        /// </summary>
        public ExecutedRoutedEventHandler? ExecutedHandler { get; set; }

        /// <summary>
        /// Gets the command CanExecute event handler.
        /// </summary>
        public CanExecuteRoutedEventHandler? CanExecuteHandler { get; set; }

        /// <summary>
        /// Gets the <see cref="CommandBinding"/>.
        /// </summary>
        public CommandBinding? CommandBinding => Command is null ? null : commandBinding ??= new CommandBinding(Command, ExecutedHandler, CanExecuteHandler);
        private CommandBinding? commandBinding;

        internal Item(string commandName, ICommand? command, FrameworkElement? element)
        {
            CommandName = commandName;
            Command = command;
            Element = element;
        }

        internal Item(string commandName, ICommand? command, FrameworkElement? element, ExecutedRoutedEventHandler executedHandler) : this(commandName, command, element)
        {
            ExecutedHandler = executedHandler;
        }

        internal Item(string commandName, ICommand? command, FrameworkElement? element, CanExecuteRoutedEventHandler canExecuteHandler) : this(commandName, command, element)
        {
            CanExecuteHandler = canExecuteHandler;
        }

        /// <summary>
        /// Adds the command handler to the element.
        /// </summary>
        public void AddCommandHandler()
        {
            if (Element is null || CommandBinding is null) return;

            Element.CommandBindings.Add(CommandBinding);
        }

        /// <summary>
        /// Removes the command handler from the element.
        /// </summary>
        public void RemoveCommandHandler()
        {
            if (Element is null || commandBinding is null) return;

            Element.CommandBindings.Remove(CommandBinding);
        }

        /// <summary>
        /// Raises the command Executed event using the specified parameter.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        public void RaiseExecuted(ICommand? command, object? sender, object? parameter)
        {
            if (ExecutedHandler is null) return;

            Handle<ExecutedRoutedEventArgs>(ExecutedHandler, command, sender, parameter);
        }

        /// <summary>
        /// Raises the command Executed event using the specified parameter.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <param name="dependencyResolvers">The resolver to resolve dependencies of parameters.</param>
        public void RaiseExecuted(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolvers)
        {
            if (ExecutedHandler is null) return;

            Handle<ExecutedRoutedEventArgs>(ExecutedHandler, command, sender, parameter, dependencyResolvers);
        }

        /// <summary>
        /// Raises the command CanExecute event using the specified parameter.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
        public CanExecuteRoutedEventArgs? RaiseCanExecute(ICommand? command, object? sender, object? parameter)
        {
            return CanExecuteHandler is null ? null : Handle<CanExecuteRoutedEventArgs>(CanExecuteHandler, command, sender, parameter);
        }

        /// <summary>
        /// Raises the command CanExecute event using the specified parameter.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <param name="dependencyResolver">the resolver to resolve dependencies of parameters.</param>
        /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
        public CanExecuteRoutedEventArgs? RaiseCanExecute(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
        {
            return CanExecuteHandler is null ? null : Handle<CanExecuteRoutedEventArgs>(CanExecuteHandler, command, sender, parameter, dependencyResolver);
        }

        /// <summary>
        /// Raises the command Executed event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task RaiseExecutedAsync(ICommand? command, object? sender, object? parameter)
        {
            if (ExecutedHandler is null) return;

            await HandleAsync<ExecutedRoutedEventArgs>(ExecutedHandler, command, sender, parameter);
        }

        /// <summary>
        /// Raises the command Executed event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task RaiseExecutedAsync(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
        {
            if (ExecutedHandler is null) return;

            await HandleAsync<ExecutedRoutedEventArgs>(ExecutedHandler, command, sender, parameter, dependencyResolver);
        }

        /// <summary>
        /// Raises the command CanExecute event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task<CanExecuteRoutedEventArgs?> RaiseCanExecuteAsync(ICommand? command, object? sender, object? parameter)
        {
            return CanExecuteHandler is null ? null : await HandleAsync<CanExecuteRoutedEventArgs>(CanExecuteHandler, command, sender, parameter);
        }

        /// <summary>
        /// Raises the command CanExecute event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="command">The command that is executed.</param>
        /// <param name="sender">The object where the event handler is attached.</param>
        /// <param name="parameter">The parameter of the command.</param>
        /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task<CanExecuteRoutedEventArgs?> RaiseCanExecuteAsync(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
        {
            return CanExecuteHandler is null ? null : await HandleAsync<CanExecuteRoutedEventArgs>(CanExecuteHandler, command, sender, parameter, dependencyResolver);
        }

        private T? Handle<T>(Delegate handler, ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>>? dependencyResolver = null) where T : RoutedEventArgs
        {
            var e = CreateEventArgs<T>(command, parameter);
            if (e is null) return null;

            Handle(handler, command, sender, e, dependencyResolver);

            return e;
        }

        private async Task<T?> HandleAsync<T>(Delegate handler, ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>>? dependencyResolver = null) where T : RoutedEventArgs
        {
            var e = CreateEventArgs<T>(command, parameter);
            if (e is null) return null;

            if (Handle(handler, command, sender, e, dependencyResolver) is Task task)
            {
                await task;
            }

            return e;
        }

        private object? Handle<T>(Delegate handler, ICommand? command, object? sender, T e, IDictionary<Type, Func<object?>>? dependencyResolver = null) where T : RoutedEventArgs
            => handler.Target is RoutedEventHandlerAction<T> action ?
                action.Handle(sender, e, dependencyResolver) :
                handler.DynamicInvoke(CreateDependencyResolver(dependencyResolver).Resolve(handler.Method, sender, e));

        private T? CreateEventArgs<T>(ICommand? command, object? parameter) where T : RoutedEventArgs
        {
            if (command is null && Command is null) return null;

            return Activator.CreateInstance(
                typeof(T), BindingFlags.NonPublic | BindingFlags.Instance,
                null, new[] { command ?? Command, parameter }, null
            ) as T;
        }

        private IParameterDependencyResolver CreateDependencyResolver(IDictionary<Type, Func<object?>>? dependencyResolver)
            => dependencyResolver is null ? new WpfParameterDependencyResolver() : new WpfParameterDependencyResolver(dependencyResolver);
    }
}