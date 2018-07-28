// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc
{
    internal sealed class DataContextChangedEventHandlerItem : WpfEventHandlerItem
    {
        public DataContextChangedEventHandlerItem(string elementName, FrameworkElement element, string eventName, Delegate handler, bool handledEventsToo) : base(elementName, element, eventName, null, null, handler, handledEventsToo)
        {
        }

        protected override void AddEventHandler(FrameworkElement element, Delegate handler, bool handledEventsToo)
        {
            if (element == null || handler == null) return;

            element.DataContextChanged += (DependencyPropertyChangedEventHandler)handler;
        }

        protected override void RemoveEventHandler(FrameworkElement element, Delegate handler)
        {
            if (element == null || handler == null) return;

            element.DataContextChanged -= (DependencyPropertyChangedEventHandler)handler;
        }

        protected override object Handle(Delegate handler, object sender, object e)
            => (handler.Target as EventHandlerAction)?.Handle(sender, (DependencyPropertyChangedEventArgs)e);

        public class EventHandlerAction
        {
            private readonly MethodInfo method;
            private readonly object target;

            public EventHandlerAction(MethodInfo method, object target)
            {
                this.method = method ?? throw new ArgumentNullException(nameof(method));
                this.target = target;
            }

            public void OnHandled(object sender, DependencyPropertyChangedEventArgs e) => Handle(sender, e);

            public object Handle(object sender, DependencyPropertyChangedEventArgs e)
            {
                switch (method.GetParameters().Length)
                {
                    case 0: return method.Invoke(target, null);
                    case 1: return method.Invoke(target, new object[] { e });
                    case 2: return method.Invoke(target, new[] { sender, e });
                    default: throw new InvalidOperationException("The length of the method parameters must be less than 3.");
                }
            }
        }
    }
}
