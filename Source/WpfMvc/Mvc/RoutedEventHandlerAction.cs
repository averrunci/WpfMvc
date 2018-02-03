// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Represents the action of an event handler.
    /// </summary>
    public class RoutedEventHandlerAction
    {
        private MethodInfo Method { get; }
        private object Target { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedEventHandlerAction"/> class
        /// with the specified method to handle an event and target to invoke it.
        /// </summary>
        /// <param name="method">The method to handle an event.</param>
        /// <param name="target">The target object to invoke the method to handle an event.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is <c>null</c>.
        /// </exception>
        public RoutedEventHandlerAction(MethodInfo method, object target)
        {
            Method = method.RequireNonNull(nameof(method));
            Target = target;
        }

        /// <summary>
        /// Handles the event with the specified a source of the event and the event data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        public void OnHandled(object sender, RoutedEventArgs e)
        {
            Handle(sender, e);
        }

        /// <summary>
        /// Handles the event with the specified a source of the event and the event data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        /// <returns>An object containing the return value of the invoked method.</returns>
        public object Handle(object sender, RoutedEventArgs e)
        {
            switch (Method.GetParameters().Length)
            {
                case 0:
                    return Method.Invoke(Target, null);
                case 1:
                    return Method.Invoke(Target, new object[] { e });
                case 2:
                    return Method.Invoke(Target, new object[] { sender, e });
                default:
                    throw new InvalidOperationException("The length of the method parameters must be less than 3.");
            }
        }
    }
}
