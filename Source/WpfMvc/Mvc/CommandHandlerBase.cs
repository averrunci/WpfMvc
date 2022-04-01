// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents the base of command handlers.
/// </summary>
public class CommandHandlerBase
{
    private readonly ICollection<CommandHandlerItem> items = new Collection<CommandHandlerItem>();

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
    /// <param name="eventName">The name of the command event.</param>
    /// <param name="command">The command that is bound.</param>
    /// <param name="element">The element that binds the command.</param>
    /// <param name="handler">The command event handler.</param>
    public void Add(string commandName, string eventName, ICommand? command, FrameworkElement? element, Delegate? handler)
    {
        var item = items.FirstOrDefault(i => i.Has(commandName, element));
        if (item is null)
        {
            items.Add(item = new CommandHandlerItem(commandName, command, element));
        }

        item.Apply(eventName, handler);
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
    public Executor GetBy(string commandName) => new(items.Where(i => i.Has(commandName)));

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
        private readonly IEnumerable<CommandHandlerItem> items;
        private object? sender;
        private ICommand? command;
        private readonly IDictionary<Type, Func<object?>> dependencyResolver = new Dictionary<Type, Func<object?>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Executor"/> class,
        /// using the supplied items.
        /// </summary>
        /// <param name="items">The command event handler items.</param>
        public Executor(IEnumerable<CommandHandlerItem> items)
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
        /// Raises the command PreviewExecuted event using the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        public void RaisePreviewExecuted(object? parameter = null) => items.ForEach(i => i.RaisePreviewExecuted(command, sender, parameter, dependencyResolver));

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
        /// Raises the command the PreviewCanExecute event using the specified parameter.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the PreviewCanExecute event.</returns>
        public IEnumerable<CanExecuteRoutedEventArgs> RaisePreviewCanExecute(object? parameter = null)
            => items.Select(item => item.RaisePreviewCanExecute(command, sender, parameter, dependencyResolver))
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

        /// <summary>
        /// Raises the command PreviewExecuted event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task RaisePreviewExecutedAsync(object? parameter = null)
        {
            foreach (var item in items)
            {
                await item.RaisePreviewExecutedAsync(command, sender, parameter, dependencyResolver);
            }
        }

        /// <summary>
        /// Raises the command PreviewCanExecute event using the specified parameter asynchronously.
        /// </summary>
        /// <param name="parameter">The parameter of the command.</param>
        /// <returns>A task that represents the asynchronous raise operation.</returns>
        public async Task<IEnumerable<CanExecuteRoutedEventArgs>> RaisePreviewCanExecuteAsync(object? parameter = null)
        {
            var args = new List<CanExecuteRoutedEventArgs>();
            foreach (var item in items)
            {
                var e = await item.RaisePreviewCanExecuteAsync(command, sender, parameter, dependencyResolver);
                if (e is not null) args.Add(e);
            }
            return args.AsReadOnly();
        }
    }
}