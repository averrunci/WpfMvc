// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Fievus.Windows.Runners
{
    /// <summary>
    /// Starts the WPF application thread in a single thread apartment.
    /// </summary>
    public static class WpfApplicationRunner
    {
        /// <summary>
        /// Starts the WPF application thread in a single thread apartment
        /// with the specified type of the application.
        /// </summary>
        /// <typeparam name="T">The type of the application.</typeparam>
        /// <returns>
        /// A new instance that implements <see cref="IWpfApplicationRunner{T}"/> interface
        /// to run an action on the WPF application thread.
        /// </returns>
        public static IWpfApplicationRunner<T> Start<T>() where T : Application, new()
        {
            return CreateRunner<T>().Start(application => { });
        }

        /// <summary>
        /// Starts the WPF application thread in a single thread apartment
        /// with the specified type of the application and the specified action
        /// to run before running the application.
        /// </summary>
        /// <typeparam name="T">The type of the application.</typeparam>
        /// <param name="action">The action to run before running the application.</param>
        /// <returns>
        /// A new instance that implements <see cref="IWpfApplicationRunner{T}"/> interface
        /// to run an action on the WPF application thread.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <c>null</c>.
        /// </exception>
        public static IWpfApplicationRunner<T> Start<T>(Action<T> action) where T : Application, new()
        {
            action.RequireNonNull(nameof(action));
            return CreateRunner<T>().Start(action);
        }

        /// <summary>
        /// Starts the WPF application thread in a single thread apartment
        /// with the specified type of the application and the specified window
        /// that is a main window of the application.
        /// </summary>
        /// <typeparam name="T">The type of the application.</typeparam>
        /// <typeparam name="W">The type of the window that is a main window of the application.</typeparam>
        /// <returns>
        /// A new instance that implements <see cref="IWpfApplicationRunner{T}"/> interface
        /// to run an action on the WPF application thread.
        /// </returns>
        public static IWpfApplicationRunner<T> Start<T, W>() where T : Application, new() where W : Window, new()
        {
            return CreateRunner<T>().Start<W>(application => { });
        }

        /// <summary>
        /// Starts the WPF application thread in a single thread apartment
        /// with the specified type of the application, the specified window
        /// that is a main window of the application, and the specified action
        /// to run before running the application.
        /// </summary>
        /// <typeparam name="T">The type of the application.</typeparam>
        /// <typeparam name="W">The type of the window that is a main window of the application.</typeparam>
        /// <param name="action">The action to run before running the application.</param>
        /// <returns>
        /// A new instance that implements <see cref="IWpfApplicationRunner{T}"/> interface
        /// to run an action on the WPF application thread.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <c>null</c>.
        /// </exception>
        public static IWpfApplicationRunner<T> Start<T, W>(Action<T> action) where T : Application, new() where W : Window, new()
        {
            return CreateRunner<T>().Start<W>(action);
        }

        private static WpfApplicationRunner<T> CreateRunner<T>() where T : Application, new()
        {
            return AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation)
                .CreateInstanceAndUnwrap(typeof(WpfApplicationRunner<T>).Assembly.FullName, typeof(WpfApplicationRunner<T>).FullName) as WpfApplicationRunner<T>;
        }
    }

    internal class WpfApplicationRunner<T> : MarshalByRefObject, IWpfApplicationRunner<T> where T : Application, new()
    {
        private readonly AutoResetEvent shutdownEvent = new AutoResetEvent(false);
        private T application;
        private Dispatcher dispatcher;

        public IWpfApplicationRunner<T> Run(Action<T> action)
        {
            StaActionRunner.Run(() => dispatcher.Invoke(() => action(application)));
            return this;
        }

        public void Shutdown()
        {
            Run(application => application.Shutdown());
            shutdownEvent.WaitOne();
        }

        internal WpfApplicationRunner<T> Start(Action<T> action)
        {
            return StartApplication(action, application => application.Run());
        }

        internal WpfApplicationRunner<T> Start<W>(Action<T> action) where W : Window, new()
        {
            return StartApplication(action, application => application.Run(new W()));
        }

        private WpfApplicationRunner<T> StartApplication(Action<T> action, Action<T> runApplication)
        {
            var applicationRunEvent = new AutoResetEvent(false);

            StaActionRunner.RunAsync(() =>
            {
                application = new T();
                dispatcher = application.Dispatcher;
                action(application);
                applicationRunEvent.Set();

                runApplication(application);
                shutdownEvent.Set();
            });

            applicationRunEvent.WaitOne();
            return this;
        }
    }
}
