// Copyright (C) 2016-2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Represents the base of routed event handlers.
    /// </summary>
    public class RoutedEventHandlerBase
    {
        private readonly ICollection<Item> items = new Collection<Item>();

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventHandlerBase"/> class.
        /// </summary>
        public RoutedEventHandlerBase()
        {
        }

        /// <summary>
        /// Adds a routed event handler to the <see cref="RoutedEventHandlerBase"/>.
        /// </summary>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="element">The element that raises the routed event.</param>
        /// <param name="routedEventName">The name of the routed event.</param>
        /// <param name="routedEvent"><see cref="RoutedEvent"/> that is handled.</param>
        /// <param name="handler">The routed event handler.</param>
        /// <param name="handledEventsToo">
        /// The value that indicates whether to register the handler such that
        /// it is invoked even when the routed event is marked handled in its event data.
        /// </param>
        public void Add(string elementName, FrameworkElement element, string routedEventName, RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
        {
            items.Add(new Item(elementName, element, routedEventName, routedEvent, handler, handledEventsToo));
        }

        /// <summary>
        /// Removes all routed event handlers from the <see cref="RoutedEventHandlerBase"/>.
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// Gets an executor that raises a routed event for the specified name of the element.
        /// </summary>
        /// <param name="elementName">The name of the element that has routed event handlers.</param>
        /// <returns>
        /// <see cref="Executor"/> that raises a routed event.
        /// </returns>
        public Executor GetBy(string elementName) => new Executor(items.Where(i => i.Has(elementName)));

        /// <summary>
        /// Registers routed event handlers to the element.
        /// </summary>
        public void RegisterRoutedEventHandler() => items.ForEach(i => i.RegisterRoutedEventHandler());

        /// <summary>
        /// Unregisters routed event handlers from the element.
        /// </summary>
        public void UnregisterRoutedEventHandler() => items.ForEach(i => i.UnregisterRoutedEventHandler());

        /// <summary>
        /// Provides a routed event execution.
        /// </summary>
        public sealed class Executor
        {
            private readonly IEnumerable<Item> items;
            private object sender;
            private RoutedEventArgs e;

            /// <summary>
            /// Initializes a new instance of the <see cref="Executor"/> class,
            /// using the supplied items.
            /// </summary>
            /// <param name="items">The routed event handler items.</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="items"/> is <c>null</c>.
            /// </exception>
            public Executor(IEnumerable<Item> items)
            {
                this.items = items.RequireNonNull(nameof(items));
            }

            /// <summary>
            /// Sets the object where the routed event handler is attached.
            /// </summary>
            /// <param name="sender">The object where the routed event handler is attached.</param>
            /// <returns>
            /// The instance of the <see cref="Executor"/> class.
            /// </returns>
            public Executor From(object sender)
            {
                this.sender = sender;
                return this;
            }

            /// <summary>
            /// Sets the event data.
            /// </summary>
            /// <typeparam name="T">The type of the event data.</typeparam>
            /// <param name="e">The event data.</param>
            /// <returns>
            /// The instance of the <see cref="Executor"/> class.
            /// </returns>
            public Executor With<T>(T e) where T : RoutedEventArgs
            {
                this.e = e;
                return this;
            }

            /// <summary>
            /// Raises the routed event of the specified name.
            /// </summary>
            /// <param name="routedEventName">The name of the routed event.</param>
            public void Raise(string routedEventName) => items.ForEach(i => i.Raise(routedEventName, sender, e));

            /// <summary>
            /// Raises the routed event of the specified name asynchronously.
            /// </summary>
            /// <param name="routedEventName">The name of the routed event.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task RaiseAsync(string routedEventName)
            {
                foreach (var item in items)
                {
                    await item.RaiseAsync(routedEventName, sender, e);
                }
            }
        }

        /// <summary>
        /// Represents an item of a routed event handler.
        /// </summary>
        public sealed class Item
        {
            /// <summary>
            /// Gets the name of the element.
            /// </summary>
            public string ElementName { get; }

            /// <summary>
            /// Gets the element that has routed event handlers.
            /// </summary>
            public FrameworkElement Element { get; }

            /// <summary>
            /// Gets the name of the routed event.
            /// </summary>
            public string RoutedEventName { get; }

            /// <summary>
            /// Gets <see cref="RoutedEvent"/> that is handled.
            /// </summary>
            public RoutedEvent RoutedEvent { get; }

            /// <summary>
            /// Gets the routed event handler.
            /// </summary>
            public Delegate Handler { get; }

            /// <summary>
            /// Gets the value that indicates whether to register the handler such that
            /// it is invoked even when the routed event is marked handled in its event data.
            /// </summary>
            public bool HandledEventsToo { get; }

            internal Item(string elementName, FrameworkElement element, string routedEventName, RoutedEvent routedEvent, Delegate handler, bool handledEventsToo)
            {
                ElementName = elementName;
                Element = element;
                RoutedEventName = routedEventName;
                RoutedEvent = routedEvent;
                Handler = handler;
                HandledEventsToo = handledEventsToo;
            }

            /// <summary>
            /// Gets a value that indicates whether <see cref="Item"/> has the specified element name.
            /// </summary>
            /// <param name="elementName">An element name.</param>
            /// <returns>
            /// <c>true</c> if <see cref="Item"/> has the specified element name; otherwise, <c>false</c>.
            /// </returns>
            public bool Has(string elementName) => ElementName == (elementName ?? string.Empty);

            /// <summary>
            /// Registers the routed event handler to the element.
            /// </summary>
            public void RegisterRoutedEventHandler()
            {
                if (Element == null || RoutedEvent == null || Handler == null) { return; }

                Element.AddHandler(RoutedEvent, Handler, HandledEventsToo);
            }

            /// <summary>
            /// Unregisters the routed event handler from the element.
            /// </summary>
            public void UnregisterRoutedEventHandler()
            {
                if (Element == null || RoutedEvent == null || Handler == null) { return; }

                Element.RemoveHandler(RoutedEvent, Handler);
            }

            /// <summary>
            /// Raises the routed event of the specified name.
            /// </summary>
            /// <param name="routedEventName">The name of the routed event.</param>
            /// <param name="sender">The object where the routed event handler is attached.</param>
            /// <param name="e">The event data.</param>
            public void Raise(string routedEventName, object sender, RoutedEventArgs e)
            {
                if (RoutedEventName != routedEventName || Handler == null) { return; }

                switch (Handler.Method.GetParameters().Length)
                {
                    case 0:
                        Handler.DynamicInvoke();
                        break;
                    case 1:
                        Handler.DynamicInvoke(new object[] { e });
                        break;
                    case 2:
                        Handler.DynamicInvoke(new object[] { sender, e });
                        break;
                }
            }

            /// <summary>
            /// Raises the routed event of the specified name asynchronously.
            /// </summary>
            /// <param name="routedEventName">The name of the routed event.</param>
            /// <param name="sender">The object where the routed event handler is attached.</param>
            /// <param name="e">The event data.</param>
            /// <returns>A task that represents the asynchronous raise operation.</returns>
            public async Task RaiseAsync(string routedEventName, object sender, RoutedEventArgs e)
            {
                if (RoutedEventName != routedEventName || Handler == null) { return; }

                var action = Handler.Target as RoutedEventHandlerAction;
                if (action == null) { return; }

                var task = action.Handle(sender, e) as Task;
                if (task != null)
                {
                    await task;
                }
            }
        }
    }
}
