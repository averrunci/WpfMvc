// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;

using NUnit.Framework;

using Fievus.Windows.Runners;

namespace Fievus.Windows.Mvc.Bindings
{
    [TestFixture]
    public class SynchronizationObervableCollectionTest
    {
        [Test]
        public void RaisesCollectionChangedEventOnWpfApplicationThreadWhenValueIsChangedOnAnotherThread()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var collection = new SynchronizationObservableCollection<string>();

                SynchronizationObservableCollectionTester<string>.Of(collection).OnThreadOf(Thread.CurrentThread.ManagedThreadId);

                Task.Factory.StartNew(() => collection.Add("Test")).Wait();
            }).Shutdown();
        }

        [Test]
        public void RaisesCollectionChangedEventOnWpfApplicationThreadWhenValueIsChangedOnSameThread()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var collection = new SynchronizationObservableCollection<string>();

                SynchronizationObservableCollectionTester<string>.Of(collection).OnThreadOf(Thread.CurrentThread.ManagedThreadId);

                collection.Add("Test");
            }).Shutdown();
        }

        private sealed class SynchronizationObservableCollectionTester<T> : DispatcherObject
        {
            private readonly int managedThreadId;
            private readonly SynchronizationObservableCollection<T> target;

            public SynchronizationObservableCollectionTester(Builder builder)
            {
                this.managedThreadId = builder.ManagedThreadId;
                this.target = builder.Target;
                target.CollectionChanged += OnNotifyCollectionChanged;
            }

            public static Builder Of(SynchronizationObservableCollection<T> target)
            {
                return new Builder(target);
            }

            private void OnNotifyCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                Assert.That(Thread.CurrentThread.ManagedThreadId, Is.EqualTo(managedThreadId));
            }

            public sealed class Builder
            {
                public SynchronizationObservableCollection<T> Target { get; }
                public int ManagedThreadId { get; private set; }

                public Builder(SynchronizationObservableCollection<T> target)
                {
                    Target = target;
                }

                public SynchronizationObservableCollectionTester<T> OnThreadOf(int managedThreadId)
                {
                    ManagedThreadId = managedThreadId;
                    return new SynchronizationObservableCollectionTester<T>(this);
                }
            }
        }
    }
}
