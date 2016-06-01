// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;

namespace Fievus.Windows.Mvc
{
    internal class TestWpfControllers
    {
        public class TestWpfController
        {
            [DataContext]
            public object Context { get; set; }

            [Element]
            public FrameworkElement Element { get; set; }

            [RoutedEventHandler(ElementName = "Element", RoutedEvent = "Loaded")]
            private void ChildElement_Loaded()
            {
                AssertionHandler?.Invoke();
            }

            public Action AssertionHandler { get; set; }

            public TestWpfController() { }
        }

        public class AttributedToField
        {
            public class NoArgumentHandlerController
            {
                [DataContext]
                private object context;

                [Element]
                private FrameworkElement element;

                [Element]
                private FrameworkElement childElement;

                [RoutedEventHandler(ElementName = "childElement", RoutedEvent = "Loaded")]
                private Action handler;

                public NoArgumentHandlerController(Action assertionHandler)
                {
                    handler = () => assertionHandler();
                }

                public object Context { get { return context; } }
                public FrameworkElement Element { get { return element; } }
                public FrameworkElement ChildElement { get { return childElement; } }
            }

            public class OneArgumentHandlerController
            {
                [DataContext]
                private object context;

                [Element]
                private FrameworkElement element;

                [Element]
                private FrameworkElement childElement;

                [RoutedEventHandler(ElementName = "childElement", RoutedEvent = "Loaded")]
                private Action<RoutedEventArgs> handler;

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    handler = e => assertionHandler(e);
                }

                public object Context { get { return context; } }
                public FrameworkElement Element { get { return element; } }
                public FrameworkElement ChildElement { get { return childElement; } }
            }

            public class RoutedEventHandlerController
            {
                [DataContext]
                private object context;

                [Element]
                private FrameworkElement element;

                [Element]
                private FrameworkElement childElement;

                [RoutedEventHandler(ElementName = "childElement", RoutedEvent = "Loaded")]
                private RoutedEventHandler handler;

                public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
                {
                    handler = (s, e) => assertionHandler(s, e);
                }

                public object Context { get { return context; } }
                public FrameworkElement Element { get { return element; } }
                public FrameworkElement ChildElement { get { return childElement; } }
            }
        }

        public class AttributedToProperty
        {
            public class NoArgumentHandlerController
            {
                [DataContext]
                public object Context { get; private set; }

                [Element(Name = "element")]
                public FrameworkElement Element { get; private set; }

                [Element]
                public FrameworkElement ChildElement { get; private set; }

                [RoutedEventHandler(ElementName = "ChildElement", RoutedEvent = "Loaded")]
                private Action Handler { get; set; }

                public NoArgumentHandlerController(Action assertionHandler)
                {
                    Handler = () => assertionHandler();
                }
            }

            public class OneArgumentHandlerController
            {
                [DataContext]
                public object Context { get; private set; }

                [Element(Name = "element")]
                public FrameworkElement Element { get; private set; }

                [Element]
                public FrameworkElement ChildElement { get; private set; }

                [RoutedEventHandler(ElementName = "ChildElement", RoutedEvent = "Loaded")]
                private Action<RoutedEventArgs> Handler { get; set; }

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    Handler = e => assertionHandler(e);
                }
            }

            public class RoutedEventHandlerController
            {
                [DataContext]
                public object Context { get; private set; }

                [Element(Name = "element")]
                public FrameworkElement Element { get; private set; }

                [Element]
                public FrameworkElement ChildElement { get; private set; }

                [RoutedEventHandler(ElementName = "ChildElement", RoutedEvent = "Loaded")]
                private RoutedEventHandler Handler { get; set; }

                public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
                {
                   Handler = (s, e) => assertionHandler(s, e);
                }
            }
        }

        public class AttributedToMethod
        {
            public class NoArgumentHandlerController
            {
                [DataContext]
                public void SetDataContext(object context) => Context = context;
                public object Context { get; private set; }

                [Element(Name = "element")]
                public void SetElement(FrameworkElement element) => Element = element;
                public FrameworkElement Element { get; private set; }

                [Element]
                public void SetChildElement(FrameworkElement childElement) => ChildElement = childElement;
                public FrameworkElement ChildElement { get; private set; }

                [RoutedEventHandler(ElementName = "ChildElement", RoutedEvent = "Loaded")]
                public void ChildElement_Loaded()
                {
                    handler();
                }
                private Action handler;

                public NoArgumentHandlerController(Action assertionHandler)
                {
                    handler = () => assertionHandler();
                }
            }

            public class OneArgumentHandlerController
            {
                [DataContext]
                public void SetDataContext(object context) => Context = context;
                public object Context { get; private set; }

                [Element(Name = "element")]
                public void SetElement(FrameworkElement element) => Element = element;
                public FrameworkElement Element { get; private set; }

                [Element]
                public void SetChildElement(FrameworkElement childElement) => ChildElement = childElement;
                public FrameworkElement ChildElement { get; private set; }

                [RoutedEventHandler(ElementName = "ChildElement", RoutedEvent = "Loaded")]
                public void ChildElement_Loaded(RoutedEventArgs e)
                {
                    handler(e);
                }
                private Action<RoutedEventArgs> handler;

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    handler = e => assertionHandler(e);
                }
            }

            public class RoutedEventHandlerController
            {
                [DataContext]
                public void SetDataContext(object context) => Context = context;
                public object Context { get; private set; }

                [Element(Name = "element")]
                public void SetElement(FrameworkElement element) => Element = element;
                public FrameworkElement Element { get; private set; }

                [Element]
                public void SetChildElement(FrameworkElement childElement) => ChildElement = childElement;
                public FrameworkElement ChildElement { get; private set; }

                [RoutedEventHandler(ElementName = "ChildElement", RoutedEvent = "Loaded")]
                public void ChildElement_Loaded(object sender, RoutedEventArgs e)
                {
                    handler(sender, e);
                }
                private RoutedEventHandler handler;

                public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
                {
                    handler = (s, e) => assertionHandler(s, e);
                }
            }
        }
    }
}
