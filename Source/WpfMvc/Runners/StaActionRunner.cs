// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;

namespace Fievus.Windows.Runners
{
    /// <summary>
    /// Runs an action in a single thread apartment.
    /// </summary>
    public static class StaActionRunner
    {
        /// <summary>
        /// Runs the specified action in a single thread apartment.
        /// If the current apartment of the current thread is a STA,
        /// the specified action is executed on the current thread;
        /// otherwise on a new thread the apartment state of which is
        /// a single thread apartment.
        /// </summary>
        /// <param name="action">The action to execute in a single thread apartment.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <c>null</c>.
        /// </exception>
        public static void Run(Action action)
        {
            action.RequireNonNull(nameof(action));
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                action();
            }
            else
            {
                RunningContext.For(action).Run();
            }
        }

        /// <summary>
        /// Runs the specified action in a single thread apartment asynchronously.
        /// </summary>
        /// <param name="action">The action to execute in a single thread apartment.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="action"/> is <c>null</c>.
        /// </exception>
        public static void RunAsync(Action action)
        {
            RunningContext.For(action).RunAsync();
        }

        private sealed class RunningContext
        {
            private readonly Action action;
            private Thread thread;
            private Exception exception;

            private RunningContext(Action action)
            {
                this.action = action;
            }

            public static RunningContext For(Action action) => new RunningContext(action);

            public void Run()
            {
                RunAsync();
                thread.Join();
                ThrowExceptionIfPresent();
            }

            public void RunAsync()
            {
                thread = new Thread(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception exc)
                    {
                        exception = exc;
                    }
                });
                thread.IsBackground = true;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }

            [ReflectionPermission(SecurityAction.Demand)]
            private void ThrowExceptionIfPresent()
            {
                if (exception == null) { return; }

                typeof(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance | BindingFlags.NonPublic)
                    .SetValue(exception, exception.StackTrace + Environment.NewLine);
                throw exception;
            }
        }
    }
}
