// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Mvc.Bindings
{
    /// <summary>
    /// Represents the method that handles the <see cref="ObservableProperty{T}.PropertyValueChanged"/> event
    /// raised when a value of the property is changed.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PropertyValueChangedEventArgs{T}"/> that contains the event data.</param>
    public delegate void PropertyValueChangedEventHandler<T>(object sender, PropertyValueChangedEventArgs<T> e);

    /// <summary>
    /// Provides the data for the <see cref="ObservableProperty{T}.PropertyChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public class PropertyValueChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the name of the property the value of which is changed.
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the value before the change.
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// Gets the value after the change.
        /// </summary>
        public T NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChangedEventArgs{T}"/> class
        /// with the specified name of the property, the value before the change, and the value
        /// after the change.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="oldValue">The value before the change.</param>
        /// <param name="newValue">The value after the change.</param>
        public PropertyValueChangedEventArgs(string propertyName, T oldValue, T newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
