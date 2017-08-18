// Copyright (C) 2016-2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fievus.Windows.Mvc
{
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
        public void Add(string commandName, ICommand command, FrameworkElement element, ExecutedRoutedEventHandler executedHandler)
        {
            var item = items.Where(i => i.CommandName == commandName && i.Element == element).FirstOrDefault();
            item.IfAbsent(() => items.Add(new Item(commandName, command, element, executedHandler)));
            item.IfPresent(_ => item.ExecutedHandler = executedHandler);
        }

        /// <summary>
        /// Adds a command handler to the <see cref="CommandHandlerBase"/>.
        /// </summary>
        /// <param name="commandName">The name of the command.</param>
        /// <param name="command">The command that is bound.</param>
        /// <param name="element">The element that binds the command.</param>
        /// <param name="canExecuteHandler">The command CanExecute handler.</param>
        public void Add(string commandName, ICommand command, FrameworkElement element, CanExecuteRoutedEventHandler canExecuteHandler)
        {
            var item = items.Where(i => i.CommandName == commandName && i.Element == element).FirstOrDefault();
            item.IfAbsent(() => items.Add(new Item(commandName, command, element, canExecuteHandler)));
            item.IfPresent(_ => item.CanExecuteHandler = canExecuteHandler);
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
        public Executor GetBy(string commandName) => new Executor(items.Where(i => i.CommandName == commandName));

        /// <summary>
        /// Registers command handlers to the element.
        /// </summary>
        public void RegisterCommandHandler() => items.ForEach(i => i.RegisterCommandHandler());

        /// <summary>
        /// Unregisters command handlers from the element.
        /// </summary>
        public void UnregisterCommandHandler() => items.ForEach(i => i.UnregisterCommandHandler());

        /// <summary>
        /// Provides command events execution functions.
        /// </summary>
        public sealed class Executor
        {
            private readonly IEnumerable<Item> items;
            private object sender;
            private ICommand command;

            /// <summary>
            /// Initializes a new instance of the <see cref="Executor"/> class,
            /// using the supplied items.
            /// </summary>
            /// <param name="items">The command event handler items.</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="items"/> is <c>null</c>.
            /// </exception>
            public Executor(IEnumerable<Item> items)
            {
                this.items = items.RequireNonNull(nameof(items));
            }

            /// <summary>
            /// Sets the object where the event handler is attached.
            /// </summary>
            /// <param name="sender">The object where the event handler is attached.</param>
            /// <returns>
            /// The instance of the <see cref="Executor"/> class.
            /// </returns>
            public Executor From(object sender)
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
            public Executor With(ICommand command)
            {
                this.command = command.RequireNonNull(nameof(command));
                return this;
            }

            /// <summary>
            /// Raises the command Executed event using the specified parameter.
            /// </summary>
            /// <param name="parameter">The parameter of the command.</param>
            public void RaiseExecuted(object parameter) => items.ForEach(i => i.RaiseExecuted(command, sender, parameter));

            /// <summary>
            /// Raises the command CanExecute event using the specified parameter.
            /// </summary>
            /// <param name="parameter">The parameter of the command.</param>
            /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
            public IEnumerable<CanExecuteRoutedEventArgs> RaiseCanExecute(object parameter)
            {
                var args = new List<CanExecuteRoutedEventArgs>();
                items.ForEach(item =>
                {
                    var e = item.RaiseCanExecute(command, sender, parameter);
                    if (e != null) { args.Add(e); }
                });
                return args.AsReadOnly();
            }

            /// <summary>
            /// Raises the command Executed event using the specified parameter asynchronously.
            /// </summary>
            /// <param name="parameter">The parameter of the command.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task RaiseExecutedAsync(object parameter)
            {
                foreach (var item in items)
                {
                    await item.RaiseExecutedAsync(command, sender, parameter);
                }
            }

            /// <summary>
            /// Raises the command CanExecute event using the specified parameter asynchronously.
            /// </summary>
            /// <param name="parameter">The parameter of the command.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task<IEnumerable<CanExecuteRoutedEventArgs>> RaiseCanExecuteAsync(object parameter)
            {
                var args = new List<CanExecuteRoutedEventArgs>();
                foreach (var item in items)
                {
                    var e = await item.RaiseCanExecuteAsync(command, sender, parameter);
                    if (e != null) { args.Add(e); }
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
            public ICommand Command { get; }

            /// <summary>
            /// Gets the element that has command event handlers.
            /// </summary>
            public FrameworkElement Element { get; }

            /// <summary>
            /// Gets the command Executed event handler.
            /// </summary>
            public ExecutedRoutedEventHandler ExecutedHandler { get; set; }

            /// <summary>
            /// Gets the command CanExecute event handler.
            /// </summary>
            public CanExecuteRoutedEventHandler CanExecuteHandler { get; set; }

            /// <summary>
            /// Gets the <see cref="CommandBinding"/>.
            /// </summary>
            public CommandBinding CommandBinding
            {
                get
                {
                    commandBinding.IfAbsent(() => commandBinding = new CommandBinding(Command, ExecutedHandler, CanExecuteHandler));
                    return commandBinding;
                }
            }
            private CommandBinding commandBinding;

            internal Item(string commandName, ICommand command, FrameworkElement element, ExecutedRoutedEventHandler executedHandler) : this(commandName, command, element, executedHandler, null)
            {
            }

            internal Item(string commandName, ICommand command, FrameworkElement element, CanExecuteRoutedEventHandler canExecuteHandler) : this(commandName, command, element, null, canExecuteHandler)
            {
            }

            internal Item(string commandName, ICommand command, FrameworkElement element, ExecutedRoutedEventHandler executedHandler, CanExecuteRoutedEventHandler canExecuteHandler)
            {
                CommandName = commandName;
                Command = command;
                Element = element;
                ExecutedHandler = executedHandler;
                CanExecuteHandler = canExecuteHandler;
            }

            /// <summary>
            /// Registers the command handler to the element.
            /// </summary>
            public void RegisterCommandHandler()
            {
                if (Element == null || CommandBinding == null) { return; }

                Element.CommandBindings.Add(CommandBinding);
            }

            /// <summary>
            /// Unregisters the command handler from the element.
            /// </summary>
            public void UnregisterCommandHandler()
            {
                if (Element == null) { return; }

                Element.CommandBindings.Remove(CommandBinding);
            }

            /// <summary>
            /// Raises the command Executed event using the specified parameter.
            /// </summary>
            /// <param name="command">The command that is executed.</param>
            /// <param name="sender">The object where the event handler is attached.</param>
            /// <param name="parameter">The parameter of the command.</param>
            public void RaiseExecuted(ICommand command, object sender, object parameter)
            {
                if (ExecutedHandler == null) { return; }

                ExecutedHandler(sender, CreateEventArgs<ExecutedRoutedEventArgs>(command, parameter));
            }

            /// <summary>
            /// Raises the command CanExecute event using the specified parameter.
            /// </summary>
            /// <param name="command">The command that is executed.</param>
            /// <param name="sender">The object where the event handler is attached.</param>
            /// <param name="parameter">The parameter of the command.</param>
            /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
            public CanExecuteRoutedEventArgs RaiseCanExecute(ICommand command, object sender, object parameter)
            {
                if (CanExecuteHandler == null) { return null; }

                var e = CreateEventArgs<CanExecuteRoutedEventArgs>(command, parameter);
                CanExecuteHandler(sender, e);

                return e;
            }

            /// <summary>
            /// Raises the command Executed event using the specified parameter asynchronously.
            /// </summary>
            /// <param name="command">The command that is executed.</param>
            /// <param name="sender">The object where the event handler is attached.</param>
            /// <param name="parameter">The parameter of the command.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task RaiseExecutedAsync(ICommand command, object sender, object parameter)
            {
                if (ExecutedHandler == null) { return; }

                var action = ExecutedHandler.Target as CommandHandlerExtension.RoutedEventHandlerAction<ExecutedRoutedEventArgs>;
                if (action == null) { return; }

                var task = action.Handle(sender, CreateEventArgs<ExecutedRoutedEventArgs>(command, parameter)) as Task;
                if (task != null)
                {
                    await task;
                }
            }

            /// <summary>
            /// Raises the command CanExecute event using the specified parameter asynchronously.
            /// </summary>
            /// <param name="command">The command that is executed.</param>
            /// <param name="sender">The object where the event handler is attached.</param>
            /// <param name="parameter">The parameter of the command.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task<CanExecuteRoutedEventArgs> RaiseCanExecuteAsync(ICommand command, object sender, object parameter)
            {
                if (CanExecuteHandler == null) { return null; }

                var action = CanExecuteHandler.Target as CommandHandlerExtension.RoutedEventHandlerAction<CanExecuteRoutedEventArgs>;
                if (action == null) { return null; }

                var e = CreateEventArgs<CanExecuteRoutedEventArgs>(command, parameter);
                var task = action.Handle(sender, e) as Task;
                if (task != null)
                {
                    await task;
                }
                return e;
            }

            private T CreateEventArgs<T>(ICommand command, object parameter)
            {
                return (T)typeof(T).Assembly
                    .CreateInstance(
                        typeof(T).FullName, false,
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null, new[] { command ?? Command, parameter }, null, null
                    );
            }
        }
    }
}
