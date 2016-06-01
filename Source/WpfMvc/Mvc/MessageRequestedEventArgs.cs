// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Represents the method that handles the <see cref="FrameworkElements.MessageRequestedEvent"/> routed evnet.
    /// </summary>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="e">The event data.</param>
    public delegate void MessageRequestedEventHandler(object sender, MessageRequestedEventArgs e);

    /// <summary>
    /// Provides data for the <see cref="FrameworkElements.MessageRequestedEvent"/> routed event.
    /// </summary>
    public class MessageRequestedEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Gets or sets the string that specifies the text to display.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the string that specifies the title bar caption to display.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxButton"/> value that specifies
        /// which button or buttons to display.
        /// </summary>
        public MessageBoxButton Button { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxImage"/> value that specifies
        /// the icon to display.
        /// </summary>
        public MessageBoxImage Icon { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxResult"/> value that specifies
        /// the default result of the message box.
        /// </summary>
        public MessageBoxResult DefaultButton { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxOptions"/> value that specifies
        /// the options.
        /// </summary>
        public MessageBoxOptions Options { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="MessageBoxResult"/> value that specifies
        /// which message box button is clicked by the user.
        /// </summary>
        public MessageBoxResult Result { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRequestedEventArgs"/> class.
        /// </summary>
        public MessageRequestedEventArgs()
        {
        }
    }
}
