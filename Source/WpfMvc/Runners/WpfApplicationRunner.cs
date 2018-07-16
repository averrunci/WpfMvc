// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace Charites.Windows.Runners
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
            return CreateRunner<T>().Start(action.RequireNonNull(nameof(action)));
        }

        /// <summary>
        /// Starts the WPF application thread in a single thread apartment
        /// with the specified type of the application and the specified window
        /// that is a main window of the application.
        /// </summary>
        /// <typeparam name="T">The type of the application.</typeparam>
        /// <typeparam name="TWindow">The type of the window that is a main window of the application.</typeparam>
        /// <returns>
        /// A new instance that implements <see cref="IWpfApplicationRunner{T}"/> interface
        /// to run an action on the WPF application thread.
        /// </returns>
        public static IWpfApplicationRunner<T> Start<T, TWindow>() where T : Application, new() where TWindow : Window, new()
        {
            return CreateRunner<T>().Start<TWindow>(application => { });
        }

        /// <summary>
        /// Starts the WPF application thread in a single thread apartment
        /// with the specified type of the application, the specified window
        /// that is a main window of the application, and the specified action
        /// to run before running the application.
        /// </summary>
        /// <typeparam name="T">The type of the application.</typeparam>
        /// <typeparam name="TWindow">The type of the window that is a main window of the application.</typeparam>
        /// <param name="action">The action to run before running the application.</param>
        /// <returns>
        /// A new instance that implements <see cref="IWpfApplicationRunner{T}"/> interface
        /// to run an action on the WPF application thread.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <c>null</c>.
        /// </exception>
        public static IWpfApplicationRunner<T> Start<T, TWindow>(Action<T> action) where T : Application, new() where TWindow : Window, new()
        {
            return CreateRunner<T>().Start<TWindow>(action.RequireNonNull(nameof(action)));
        }

        private static WpfApplicationRunner<T> CreateRunner<T>() where T : Application, new()
        {
            return AppDomain.CreateDomain(Guid.NewGuid().ToString(), AppDomain.CurrentDomain.Evidence, AppDomain.CurrentDomain.SetupInformation)
                .CreateInstanceAndUnwrap(typeof(WpfApplicationRunner<T>).Assembly.FullName, typeof(WpfApplicationRunner<T>).FullName ?? throw new InvalidOperationException()) as WpfApplicationRunner<T>;
        }

        /// <summary>
        /// Drains events by pushing empty frame.
        /// </summary>
        /// <param name="this">The WPF application that is executed.</param>
        public static void DrainEvents(this Application @this)
        {
            var frame = new DispatcherFrame();
            @this?.Dispatcher?.BeginInvoke(
                DispatcherPriority.SystemIdle,
                new Func<object, object>(f =>
                {
                    ((DispatcherFrame)f).Continue = false;
                    return null;
                }),
                frame
            );
            Dispatcher.PushFrame(frame);
        }
    }

    internal class WpfApplicationRunner<T> : MarshalByRefObject, IWpfApplicationRunner<T> where T : Application, new()
    {
        private readonly AutoResetEvent shutdownEvent = new AutoResetEvent(false);
        private T application;
        private Dispatcher dispatcher;
        private WpfApplicationDataContext dataContext;

        public IWpfApplicationRunner<T> Run(Action<T> action)
        {
            StaActionRunner.Run(() => dispatcher.Invoke(() =>
            {
                action(application);
            }));
            return this;
        }

        public IWpfApplicationRunner<T> Run(Action<T, WpfApplicationDataContext> action)
        {
            StaActionRunner.Run(() => dispatcher.Invoke(() =>
            {
                action(application, dataContext);
            }));
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

        internal WpfApplicationRunner<T> Start<TWindow>(Action<T> action) where TWindow : Window, new()
        {
            return StartApplication(action, application => application.Run(new TWindow()));
        }

        private WpfApplicationRunner<T> StartApplication(Action<T> action, Action<T> runApplication)
        {
            var applicationRunEvent = new AutoResetEvent(false);

            StaActionRunner.RunAsync(() =>
            {
                application = new T();
                dispatcher = application.Dispatcher;
                dataContext = new WpfApplicationDataContext();
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
