// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
        //public Executor GetBy(string commandName) => new Executor(items.Where(i => i.CommandName == commandName));
        public Executor GetBy(string commandName)
        {
            return new Executor(items.Where(i => i.CommandName == commandName));
        }

        /// <summary>
        /// Registers command handlers to the element.
        /// </summary>
        public void RegisterCommandHandler()
        {
            items.Where(i => i.Element != null && i.CommandBinding != null)
                .ForEach(i => i.Element.CommandBindings.Add(i.CommandBinding));
        }

        /// <summary>
        /// Unregisters command handlers from the element.
        /// </summary>
        public void UnregisterCommandHandler()
        {
            items.Where(i => i.Element != null)
                .ForEach(i => i.Element.CommandBindings.Remove(i.CommandBinding));
        }

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
            public void RaiseExecuted(object parameter)
            {
                items.Where(item => item.ExecutedHandler != null)
                    .ForEach(item =>
                        item.ExecutedHandler(sender, CreateEventArgs<ExecutedRoutedEventArgs>(item, parameter))
                    );
            }

            /// <summary>
            /// Raises the command CanExecute event using the specified parameter.
            /// </summary>
            /// <param name="parameter">The parameter of the command.</param>
            public void RaiseCanExecute(object parameter)
            {
                items.Where(item => item.CanExecuteHandler != null)
                    .ForEach(item =>
                        item.CanExecuteHandler(sender, CreateEventArgs<CanExecuteRoutedEventArgs>(item, parameter))
                    );
            }

            private T CreateEventArgs<T>(Item item, object parameter)
            {
                return (T)typeof(T).Assembly
                    .CreateInstance(
                        typeof(T).FullName, false,
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null, new[] { command ?? item.Command, parameter }, null, null
                    );
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
        }
    }
}
