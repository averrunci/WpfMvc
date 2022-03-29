// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

namespace Charites.Windows.Mvc.Bindings;

/// <summary>
/// Represents a synchronization dynamic data collection that provides notifications
/// when items are added, removed, or when the whole list is refreshed.
/// </summary>
/// <typeparam name="T">The type of elements in the collection.</typeparam>
public class SynchronizationObservableCollection<T> : ObservableCollection<T>
{
    /// <summary>
    /// Occurs when an item is added, removed, changed, moved, or the entire list is refreshed.
    /// </summary>
    public override event NotifyCollectionChangedEventHandler? CollectionChanged;

    /// <summary>
    /// Initializes a new instance of the <see cref="SynchronizationObservableCollection{T}"/> class.
    /// </summary>
    public SynchronizationObservableCollection()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SynchronizationObservableCollection{T}"/> class
    /// that contains elements copied from the specified collection.
    /// </summary>
    /// <param name="collection">The collection from which the elements are copied.</param>
    public SynchronizationObservableCollection(IEnumerable<T> collection) : base(collection)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SynchronizationObservableCollection{T}"/> class
    /// that contains elements copied from the specified list.
    /// </summary>
    /// <param name="list">The list from which the elements are copied.</param>
    public SynchronizationObservableCollection(IList<T> list) : base(list)
    {
    }

    /// <summary>
    /// Raises the <see cref="CollectionChanged"/> event with the provided arguments.
    /// </summary>
    /// <param name="e">Arguments of the event being raise.</param>
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        using (BlockReentrancy())
        {
            var eventHandler = CollectionChanged;
            if (eventHandler is null) return;

            eventHandler.GetInvocationList()
                .OfType<NotifyCollectionChangedEventHandler>()
                .ForEach(handler =>
                {
                    if (handler.Target is not DispatcherObject dispatcherObject || dispatcherObject.CheckAccess())
                    {
                        handler(this, e);
                    }
                    else
                    {
                        dispatcherObject.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, handler, this, e);
                    }
                });
        }
    }
}