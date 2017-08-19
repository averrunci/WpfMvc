// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.Generic;
using System.ComponentModel;

namespace Fievus.Windows.Mvc.Bindings
{
    /// <summary>
    /// Represents a context of the multi binding.
    /// </summary>
    public sealed class MultiBindingContext
    {
        private readonly List<INotifyPropertyChanged> bindingSources;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiBindingContext"/> class
        /// with the specified binding sources that need to be <see cref="ObservableProperty{T}"/>.
        /// </summary>
        /// <param name="sources">
        /// The binding sources that need to be <see cref="ObservableProperty{T}"/>.
        /// </param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="sources"/> is <c>null</c>.
        /// </exception>
        public MultiBindingContext(IEnumerable<INotifyPropertyChanged> sources)
        {
            bindingSources = new List<INotifyPropertyChanged>(sources.RequireNonNull(nameof(sources)));
        }

        /// <summary>
        /// Gets a value of the specified type at the specified index.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="index">The index of the binding source.</param>
        /// <returns>
        /// The value of the specified type at the specified index or
        /// the default value of the specified type if the binding source
        /// at the specified index does not exist or is not <see cref="ObservableProperty{T}"/>.
        /// </returns>
        public T GetValueAt<T>(int index)
        {
            if (index < 0 || index >= bindingSources.Count) { return default(T); }

            var observableProperty = bindingSources[index] as ObservableProperty<T>;
            return observableProperty == null ? default(T) : observableProperty.Value;
        }
    }
}
