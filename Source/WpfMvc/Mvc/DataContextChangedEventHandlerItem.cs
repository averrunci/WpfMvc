// Copyright (C) 2018-2019 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
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
                return Handle(sender, e, null);
            }

            public object Handle(object sender, DependencyPropertyChangedEventArgs e, IDictionary<Type, Func<object>> dependencyResolver)
            {
                return Handle(CreateParameterDependencyResolver(dependencyResolver).Resolve(method, sender, e));
            }

            private object Handle(object[] parameters)
            {
                try
                {
                    var returnValue = method.Invoke(target, parameters);
                    if (returnValue is Task task) Await(task);
                    return returnValue;
                }
                catch (Exception exc)
                {
                    if (!HandleUnhandledException(exc)) throw;

                    return null;
                }
            }

            private async void Await(Task task)
            {
                try
                {
                    await task;
                }
                catch (Exception exc)
                {
                    if (!HandleUnhandledException(exc)) throw;
                }
            }

            private bool HandleUnhandledException(Exception exc) => WpfController.HandleUnhandledException(exc);

            private IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary<Type, Func<object>> dependencyResolver)
            {
                return new WpfParameterDependencyResolver(dependencyResolver);
            }
        }
    }
}
