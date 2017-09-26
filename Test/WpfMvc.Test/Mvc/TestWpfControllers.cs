// Copyright (C) 2016-2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Fievus.Windows.Mvc
{
    internal class TestWpfControllers
    {
        public static readonly RoutedUICommand TestCommand = new RoutedUICommand("Test", "TestCommand", typeof(TestWpfControllers));
        public static readonly RoutedUICommand AnotherTestCommand = new RoutedUICommand("Another Test", "AnotherTestCommand", typeof(TestWpfControllers));

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

            [CommandHandler(CommandName = "TestCommand")]
            private void TestCommand_Executed(ExecutedRoutedEventArgs e)
            {
                ExecutedAssertionHandler?.Invoke();
            }

            [CommandHandler(CommandName = "TestCommand")]
            private void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                CanExecuteAssertionHandler?.Invoke();
                e.CanExecute = true;
            }

            public Action AssertionHandler { get; set; }
            public Action ExecutedAssertionHandler { get; set; }
            public Action CanExecuteAssertionHandler { get; set; }

            public TestWpfController() { }
        }

        public class TestWpfControllerAsync
        {
            [DataContext]
            public object Context { get; set; }

            [Element]
            public FrameworkElement Element { get; set; }

            [RoutedEventHandler(ElementName = "Element", RoutedEvent = "Loaded")]
            private async Task Element_Loaded()
            {
                await Task.Run(() => AssertionHandler?.Invoke());
            }

            [CommandHandler(CommandName = "TestCommand")]
            private async Task TestCommand_Executed(ExecutedRoutedEventArgs e)
            {
                await Task.Run(() => ExecutedAssertionHandler?.Invoke());
            }

            [CommandHandler(CommandName = "TestCommand")]
            private async Task TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                await Task.Run(() =>
                {
                    CanExecuteAssertionHandler?.Invoke();
                    e.CanExecute = true;
                });
            }

            public Action AssertionHandler { get; set; }
            public Action ExecutedAssertionHandler { get; set; }
            public Action CanExecuteAssertionHandler { get; set; }

            public TestWpfControllerAsync() { }
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

                [CommandHandler(CommandName = "TestCommand")]
                private Action<ExecutedRoutedEventArgs> executedHandler;

                [CommandHandler(CommandName = "TestCommand")]
                private Action<CanExecuteRoutedEventArgs> canExecuteHandler;

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    handler = e => assertionHandler(e);
                }

                public OneArgumentHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler) : this(executedAssertionHandler, null)
                {
                }

                public OneArgumentHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
                {
                    executedHandler = e => executedAssertionHandler(e);
                    if (canExecuteAssertionHandler != null)
                    {
                        canExecuteHandler = e =>
                        {
                            canExecuteAssertionHandler(e);
                            e.CanExecute = true;
                        };
                    }
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

            public class OneArgumentExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private Action<ExecutedRoutedEventArgs> executedHandler;

                public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                }
            }

            public class OneArgumentExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private Action<ExecutedRoutedEventArgs> executedHandler;

                [CommandHandler(CommandName = "TestCommand")]
                private Action<CanExecuteRoutedEventArgs> canExecuteHandler;

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private Action<ExecutedRoutedEventArgs> anotherExecutedHandler;

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private Action<CanExecuteRoutedEventArgs> anotherCanExecuteHandler;

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler)
                {
                    executedHandler = e => executedAssertionHandler(e);
                    canExecuteHandler = e =>
                    {
                        canExecuteAssertionHandler(e);
                        e.CanExecute = true;
                    };

                    anotherExecutedHandler = e => anotherExecutedAssertionHandler?.Invoke(e);
                    anotherCanExecuteHandler = e =>
                    {
                        anotherCanExecuteAssertionHandler?.Invoke(e);
                        e.CanExecute = true;
                    };
                }
            }

            public class ExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private ExecutedRoutedEventHandler executedHandler;

                public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
                {
                    executedHandler = (s, e) => executedAssertionHandler(s, e);
                }
            }

            public class ExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private ExecutedRoutedEventHandler executedHandler;

                [CommandHandler(CommandName = "TestCommand")]
                private CanExecuteRoutedEventHandler canExecuteHandler;

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private ExecutedRoutedEventHandler anotherExecutedHandler;

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private CanExecuteRoutedEventHandler anotherCanExecuteHandler;

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler)
                {
                    executedHandler = (s, e) => executedAssertionHandler(s, e);
                    canExecuteHandler = (s, e) =>
                    {
                        canExecuteAssertionHandler(s, e);
                        e.CanExecute = true;
                    };

                    anotherExecutedHandler = (s, e) => anotherExecutedAssertionHandler?.Invoke(s, e);
                    anotherCanExecuteHandler = (s, e) =>
                    {
                        anotherCanExecuteAssertionHandler?.Invoke(s, e);
                        e.CanExecute = true;
                    };
                }
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

            public class OneArgumentExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; set; }

                public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                }
            }

            public class OneArgumentExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; set; }

                [CommandHandler(CommandName = "TestCommand")]
                private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private Action<ExecutedRoutedEventArgs> AnotherExecutedHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private Action<CanExecuteRoutedEventArgs> AnotherCanExecuteHandler { get; set; }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler)
                {
                    ExecutedHandler = e => executedAssertionHandler(e);
                    CanExecuteHandler = e =>
                    {
                        canExecuteAssertionHandler(e);
                        e.CanExecute = true;
                    };

                    AnotherExecutedHandler = e => anotherExecutedAssertionHandler?.Invoke(e);
                    AnotherCanExecuteHandler = e =>
                    {
                        anotherCanExecuteAssertionHandler?.Invoke(e);
                        e.CanExecute = true;
                    };
                }
            }

            public class ExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private ExecutedRoutedEventHandler ExecutedHandler { get; set; }

                public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
                {
                    ExecutedHandler = (s, e) => executedAssertionHandler(s, e);
                }
            }

            public class ExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                private ExecutedRoutedEventHandler ExecutedHandler { get; set; }

                [CommandHandler(CommandName = "TestCommand")]
                private CanExecuteRoutedEventHandler CanExecuteHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private ExecutedRoutedEventHandler AnotherExecutedHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                private CanExecuteRoutedEventHandler AnotherCanExecuteHandler { get; set; }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler)
                {
                    ExecutedHandler = (s, e) => executedAssertionHandler(s, e);
                    CanExecuteHandler = (s, e) =>
                    {
                        canExecuteAssertionHandler(s, e);
                        e.CanExecute = true;
                    };

                    AnotherExecutedHandler = (s, e) => anotherExecutedAssertionHandler?.Invoke(s, e);
                    AnotherCanExecuteHandler = (s, e) =>
                    {
                        anotherCanExecuteAssertionHandler?.Invoke(s, e);
                        e.CanExecute = true;
                    };
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

            public class OneArgumentExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                public void TestCommand_Executed(ExecutedRoutedEventArgs e)
                {
                    executedHandler(e);
                }
                private Action<ExecutedRoutedEventArgs> executedHandler { get; set; }

                public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                }
            }

            public class OneArgumentExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                public void TestCommand_Executed(ExecutedRoutedEventArgs e)
                {
                    executedHandler(e);
                }
                private Action<ExecutedRoutedEventArgs> executedHandler { get; set; }

                [CommandHandler(CommandName = "TestCommand")]
                public void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
                {
                    canExecuteHandler?.Invoke(e);
                    e.CanExecute = true;
                }
                private Action<CanExecuteRoutedEventArgs> canExecuteHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                public void AnotherTestCommand_Executed(ExecutedRoutedEventArgs e)
                {
                    anotherExecutedHandler?.Invoke(e);
                }
                private Action<ExecutedRoutedEventArgs> anotherExecutedHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                public void AnotherTestCommand_CanExecute(CanExecuteRoutedEventArgs e)
                {
                    anotherCanExecuteHandler?.Invoke(e);
                    e.CanExecute = true;
                }
                private Action<CanExecuteRoutedEventArgs> anotherCanExecuteHandler { get; set; }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler)
                {
                    executedHandler = e => executedAssertionHandler(e);
                    canExecuteHandler = e => canExecuteAssertionHandler(e);

                    anotherExecutedHandler = e => anotherExecutedAssertionHandler?.Invoke(e);
                    anotherCanExecuteHandler = e => anotherCanExecuteAssertionHandler?.Invoke(e);
                }
            }

            public class ExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                public void TestCommand_Executed(object sender, ExecutedRoutedEventArgs e)
                {
                    executedHandler(sender, e);
                }
                private ExecutedRoutedEventHandler executedHandler { get; set; }

                public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
                {
                    executedHandler = (s, e) => executedAssertionHandler(s, e);
                }
            }

            public class ExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = "TestCommand")]
                public void TestCommand_Executed(object sender, ExecutedRoutedEventArgs e)
                {
                    executedHandler(sender, e);
                }
                private ExecutedRoutedEventHandler executedHandler { get; set; }

                [CommandHandler(CommandName = "TestCommand")]
                public void TestCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
                {
                    canExecuteHandler?.Invoke(sender, e);
                    e.CanExecute = true;
                }
                private CanExecuteRoutedEventHandler canExecuteHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                public void AnotherTestCommand_Executed(object sender, ExecutedRoutedEventArgs e)
                {
                    anotherExecutedHandler?.Invoke(sender, e);
                }
                private ExecutedRoutedEventHandler anotherExecutedHandler { get; set; }

                [CommandHandler(CommandName = "AnotherTestCommand")]
                public void AnotherTestCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
                {
                    anotherCanExecuteHandler?.Invoke(sender, e);
                    e.CanExecute = true;
                }
                private CanExecuteRoutedEventHandler anotherCanExecuteHandler { get; set; }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler)
                {
                    executedHandler = (s, e) => executedAssertionHandler(s, e);
                    canExecuteHandler = (s, e) => canExecuteAssertionHandler(s, e);

                    anotherExecutedHandler = (s, e) => anotherExecutedAssertionHandler?.Invoke(s, e);
                    anotherCanExecuteHandler = (s, e) => anotherCanExecuteAssertionHandler?.Invoke(s, e);
                }
            }
        }
    }
}
