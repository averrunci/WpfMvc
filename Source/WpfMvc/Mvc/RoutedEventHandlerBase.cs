// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        /// Gets event handlers by the specified name of the element.
        /// </summary>
        /// <param name="elementName">The name of the element that has event handlers.</param>
        /// <returns>
        /// <see cref="Executor"/> that raises routed event.
        /// </returns>
        public Executor GetBy(string elementName) => new Executor(items.Where(i => i.ElementName == elementName));

        /// <summary>
        /// Registers routed event handlers to the element.
        /// </summary>
        public void RegisterRoutedEventHandler()
        {
            items.Where(i => i.Element != null && i.RoutedEvent != null && i.Handler != null)
                .ForEach(i => i.Element.AddHandler(i.RoutedEvent, i.Handler, i.HandledEventsToo));
        }

        /// <summary>
        /// Unregisters routed event handlers from the element.
        /// </summary>
        public void UnregisterRoutedEventHandler()
        {
            items.Where(i => i.Element != null && i.RoutedEvent != null && i.Handler != null)
                .ForEach(i => i.Element.RemoveHandler(i.RoutedEvent, i.Handler));
        }

        /// <summary>
        /// Provides routed events execution functions.
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
            public void Raise(string routedEventName)
            {
                items.Where(i => i.RoutedEventName == routedEventName && i.Handler != null)
                    .ForEach(i =>
                    {
                        switch (i.Handler.Method.GetParameters().Length)
                        {
                            case 0:
                                i.Handler.DynamicInvoke();
                                break;
                            case 1:
                                i.Handler.DynamicInvoke(new object[] { e });
                                break;
                            case 2:
                                i.Handler.DynamicInvoke(new object[] { sender, e });
                                break;
                        }
                    });
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
        }
    }
}
