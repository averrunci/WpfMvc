// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc.Bindings
{
    [Specification("SynchronizationObservableCollection Spec")]
    class SynchronizationObservableCollectionSpec : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        private const string WpfApplicationThreadIdKey = "WpfApplicationThreadId";
        private const string HolderKey = "Holder";

        public SynchronizationObservableCollectionSpec()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("Raises the CollectionChanged event on the WPF application thread when the collection is changed on another thread")]
        void Ex01()
        {
            Given("a SynchronizationObservableCollection", () => WpfRunner.Run((application, context) =>
            {
                context.Set(WpfApplicationThreadIdKey, Thread.CurrentThread.ManagedThreadId);
                context.Set(HolderKey, new SynchronizationObservableCollectionHolder());
            }));
            When("the collection is changed on another thread", () => WpfRunner.Run((application, context) =>
                Task.Run(() => context.Get<SynchronizationObservableCollectionHolder>(HolderKey).Add("Test")).Wait()
            ));
            Then("the managed thread id of the current thread should be the application thread id", () =>
                WpfRunner.Run((application, context) => context.Get<SynchronizationObservableCollectionHolder>(HolderKey).ManagedThreadIdOnCollectionChanged.Should().Be(context.Get<int>(WpfApplicationThreadIdKey)))
            );
        }

        [Example("Raises the CollectionChanged event on the WPF application thread when the collection is changed on same thread")]
        void Ex02()
        {
            Given("a SynchronizationObservableCollection", () => WpfRunner.Run((application, context) =>
            {
                context.Set(WpfApplicationThreadIdKey, Thread.CurrentThread.ManagedThreadId);
                context.Set(HolderKey, new SynchronizationObservableCollectionHolder());
            }));
            When("the collection is changed on same thread", () => WpfRunner.Run((application, context) =>
                context.Get<SynchronizationObservableCollectionHolder>(HolderKey).Add("Test"))
            );
            Then("the managed thread id of the current thread should be the application thread id", () =>
                WpfRunner.Run((application, context) => context.Get<SynchronizationObservableCollectionHolder>(HolderKey).ManagedThreadIdOnCollectionChanged.Should().Be(context.Get<int>(WpfApplicationThreadIdKey)))
            );
        }

        class SynchronizationObservableCollectionHolder : DispatcherObject
        {
            public int ManagedThreadIdOnCollectionChanged { get; private set; }

            private readonly SynchronizationObservableCollection<string> collection = new SynchronizationObservableCollection<string>();

            public SynchronizationObservableCollectionHolder() => collection.CollectionChanged += OnCollectionChanged;

            public void Add(string item) => collection.Add(item);
            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) => ManagedThreadIdOnCollectionChanged = Thread.CurrentThread.ManagedThreadId;
        }
    }
}
