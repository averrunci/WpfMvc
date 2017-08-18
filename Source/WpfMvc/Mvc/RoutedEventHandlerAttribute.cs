// Copyright (C) 2016-2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Specifies the target to inject a routed event handler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class RoutedEventHandlerAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the element on which the routed event is raised.
        /// </summary>
        public string ElementName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the routed event which is raised on the element.
        /// </summary>
        public string RoutedEvent { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether to register the handler such that
        /// it is invoked even when the routed event is marked handled in its event data.
        /// </summary>
        /// <remarks>
        /// <c>true</c> to register the handler such that it is invoked even when the
        /// routed event is marked handled in its event data; <c>false</c> to register
        /// the handler with the default condition that it will not be invoked if  the
        /// routed event is already marked handled.
        /// </remarks>
        public bool HandledEventsToo { get; set; }
    }
}
