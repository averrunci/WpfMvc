// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.Generic;

namespace Charites.Windows.Runners
{
    /// <summary>
    /// Represents data associated with the WPF application thread.
    /// </summary>
    public sealed class WpfApplicationDataContext
    {
        private readonly Dictionary<string, object> items = new Dictionary<string, object>();

        internal WpfApplicationDataContext()
        {
        }

        /// <summary>
        /// Gets the value associated with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="name">The name with which the value is associated.</param>
        /// <returns>
        /// The value associated with the specified name.
        /// If not found, the default value of the specified type is returned.
        /// </returns>
        public T Get<T>(string name) => items.ContainsKey(name) ? (T)items[name] : default(T);

        /// <summary>
        /// Gets the value associated with the specified name.
        /// </summary>
        /// <param name="name">The name with which the value is associated.</param>
        /// <returns>
        /// The value associated with the specified name.
        /// If not found, <c>null</c> is returned.
        /// </returns>
        public object Get(string name) => items.ContainsKey(name) ? items[name] : null;

        /// <summary>
        /// Sets the specified value with the specified name.
        /// </summary>
        /// <param name="name">The name to associate the value with.</param>
        /// <param name="item">The value to set.</param>
        public void Set(string name, object item) => items[name] = item;
    }
}
