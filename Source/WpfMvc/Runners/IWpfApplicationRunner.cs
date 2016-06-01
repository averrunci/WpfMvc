// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;

namespace Fievus.Windows.Runners
{
    /// <summary>
    /// Provides the function to run an action on the WPF application thread
    /// in a single thread apartment.
    /// </summary>
    /// <typeparam name="T">The type of the application.</typeparam>
    public interface IWpfApplicationRunner<T> where T : Application
    {
        /// <summary>
        /// Runs the specified action on the WPF application thread.
        /// </summary>
        /// <param name="action">The action to run on the WPF application thread.</param>
        /// <returns>The instance of the <see cref="IWpfApplicationRunner{T}"/> interface.</returns>
        IWpfApplicationRunner<T> Run(Action<T> action);

        /// <summary>
        /// Shut down the application.
        /// </summary>
        void Shutdown();
    }
}
