// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.ComponentModel;
using System.Windows;

namespace Fievus.Windows.Mvc.Bindings
{
    internal class PropertyChangedEventManager : WeakEventManager
    {
        public static PropertyChangedEventManager CurrentManager
        {
            get
            {
                var manager = GetCurrentManager(typeof(PropertyChangedEventManager)) as PropertyChangedEventManager;
                if (manager == null)
                {
                    manager = new PropertyChangedEventManager();
                    SetCurrentManager(typeof(PropertyChangedEventManager), manager);
                }
                return manager;
            }
        }

        public static void AddListener(INotifyPropertyChanged source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        public static void RemoveListener(INotifyPropertyChanged source, IWeakEventListener listener)
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        protected override void StartListening(object source)
        {
            ((INotifyPropertyChanged)source).PropertyChanged += DeliverEvent;
        }

        protected override void StopListening(object source)
        {
            ((INotifyPropertyChanged)source).PropertyChanged -= DeliverEvent;
        }
    }
}
