// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Mvc.Bindings
{
    /// <summary>
    /// Represents the method that handles the <see cref="ObservableProperty{T}.PropertyValueChanging"/> event
    /// raised when a value of the property is changing.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PropertyValueChangingEventArgs{T}"/> that contains the event data.</param>
    public delegate void PropertyValueChangingEventHandler<T>(object sender, PropertyValueChangingEventArgs<T> e);

    /// <summary>
    /// Provides data for the <see cref="ObservableProperty{T}.PropertyValueChanging"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public class PropertyValueChangingEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the name of the property the value of which is changing.
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
        /// Gets the value that indicates whether to change the property value
        /// </summary>
        public bool CanChangePropertyValue { get; private set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueChangingEventArgs{T}"/> class
        /// with the specified name of the property, the value before the change, and the value
        /// after the change.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="oldValue">The value before the change.</param>
        /// <param name="newValue">The vlaue after the change.</param>
        public PropertyValueChangingEventArgs(string propertyName, T oldValue, T newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        /// <summary>
        /// Sets that the value of the property can not be changed.
        /// </summary>
        public void Disable()
        {
            CanChangePropertyValue = false;
        }
    }
}
