// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Represents the method that handles the <see cref="WpfController.UnhandledException"/> event.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="WpfController.UnhandledException"/> event.
    /// </summary>
    public class UnhandledExceptionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the unhandled exception.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets or sets the value that indicates whether the exception is handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnhandledExceptionEventArgs"/> class
        /// with the specified unhandled exception.
        /// </summary>
        /// <param name="exception">The unhandled exception.</param>
        public UnhandledExceptionEventArgs(Exception exception)
        {
            Exception = exception;
        }
    }
}
