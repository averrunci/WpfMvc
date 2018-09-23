// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Charites.Windows.Mvc
{
    internal class TestWpfControllers
    {
        public static readonly RoutedUICommand TestCommand = new RoutedUICommand("Test", nameof(TestCommand), typeof(TestWpfControllers));
        public static readonly RoutedUICommand AnotherTestCommand = new RoutedUICommand("Another Test", nameof(AnotherTestCommand), typeof(TestWpfControllers));

        public class TestWpfControllerBase
        {
            [DataContext]
            public object DataContext { get; set; }

            [Element]
            public FrameworkElement Element { get; set; }

            [EventHandler(ElementName = nameof(Element), Event = nameof(FrameworkElement.Loaded))]
            protected void Element_Loaded() => LoadedAssertionHandler?.Invoke();

            [EventHandler(ElementName = nameof(Element), Event = "Button.Click")]
            protected void Element_ButtonClick() => ButtonClickAssertionHandler?.Invoke();

            [EventHandler(ElementName = nameof(Element), Event = nameof(TestElement.Changed))]
            protected void Element_Changed() => ChangedAssertionHandler?.Invoke();

            [EventHandler(Event = nameof(FrameworkElement.DataContextChanged))]
            protected void OnDataContextChanged() { }

            [CommandHandler(CommandName = nameof(TestCommand))]
            protected void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedAssertionHandler?.Invoke();

            [CommandHandler(CommandName = nameof(TestCommand))]
            protected void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                CanExecuteAssertionHandler?.Invoke();
                e.CanExecute = true;
            }

            public Action LoadedAssertionHandler { get; set; }
            public Action ButtonClickAssertionHandler { get; set; }
            public Action ChangedAssertionHandler { get; set; }
            public Action ExecutedAssertionHandler { get; set; }
            public Action CanExecuteAssertionHandler { get; set; }
        }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+TestDataContext")]
        public class TestWpfController : TestWpfControllerBase { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+MultiTestDataContext")]
        public class MultiTestWpfControllerA : TestWpfControllerBase { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+MultiTestDataContext")]
        public class MultiTestWpfControllerB : TestWpfControllerBase { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+MultiTestDataContext")]
        public class MultiTestWpfControllerC : TestWpfControllerBase { }

        public class TestController {[DataContext] public object DataContext { get; set; } }

        [View(Key = "AttachingTestDataContext")]
        public class TestDataContextController : TestController { }

        [View(Key = "BaseAttachingTestDataContext")]
        public class BaseTestDataContextController : TestController { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+AttachingTestDataContextFullName")]
        public class TestDataContextFullNameController : TestController { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+BaseAttachingTestDataContextFullName")]
        public class BaseTestDataContextFullNameController : TestController { }

        [View(Key = "GenericAttachingTestDataContext`1")]
        public class GenericTestDataContextController : TestController { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+GenericAttachingTestDataContextFullName`1[System.String]")]
        public class GenericTestDataContextFullNameController : TestController { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+GenericAttachingTestDataContextFullName`1")]
        public class GenericTestDataContextFullNameWithoutParametersController : TestController { }

        [View(Key = "IAttachingTestDataContext")]
        public class InterfaceImplementedTestDataContextController : TestController { }

        [View(Key = "Charites.Windows.Mvc.TestDataContexts+IAttachingTestDataContextFullName")]
        public class InterfaceImplementedTestDataContextFullNameController : TestController { }

        [View(Key = "TestElement")]
        public class KeyTestDataContextController : TestController { }

        public class TestWpfControllerAsync
        {
            [DataContext]
            public object DataContext { get; set; }

            [Element]
            public FrameworkElement Element { get; set; }

            [EventHandler(ElementName = nameof(Element), Event = nameof(FrameworkElement.Loaded))]
            private async Task Element_Loaded()
            {
                await Task.Run(() => LoadedAssertionHandler?.Invoke());
            }

            [CommandHandler(CommandName = nameof(TestCommand))]
            private async Task TestCommand_Executed(ExecutedRoutedEventArgs e)
            {
                await Task.Run(() => ExecutedAssertionHandler?.Invoke());
            }

            [CommandHandler(CommandName = nameof(TestCommand))]
            private async Task TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                await Task.Run(() =>
                {
                    CanExecuteAssertionHandler?.Invoke();
                    e.CanExecute = true;
                });
            }

            public Action LoadedAssertionHandler { get; set; }
            public Action ExecutedAssertionHandler { get; set; }
            public Action CanExecuteAssertionHandler { get; set; }
        }

        public class ExceptionTestWpfController
        {
            [EventHandler(Event = "Changed")]
            private void OnChanged()
            {
                throw new Exception();
            }
        }

        public class AttributedToField
        {
            public class NoArgumentHandlerController
            {
                [DataContext]
                private object dataContext;

                [Element]
                private FrameworkElement element;

                [Element]
                private FrameworkElement childElement;

                [EventHandler(ElementName = nameof(childElement), Event = nameof(FrameworkElement.Loaded))]
                private Action handler;

                public NoArgumentHandlerController(Action assertionHandler)
                {
                    handler = assertionHandler;
                }

                public object DataContext => dataContext;
                public FrameworkElement Element => element;
                public FrameworkElement ChildElement => childElement;
            }

            public class OneArgumentHandlerController
            {
                [DataContext]
                private object dataContext;

                [Element]
                private FrameworkElement element;

                [Element]
                private FrameworkElement childElement;

                [EventHandler(ElementName = nameof(childElement), Event = nameof(FrameworkElement.Loaded))]
                private Action<RoutedEventArgs> handler;

                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<ExecutedRoutedEventArgs> executedHandler;

                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<CanExecuteRoutedEventArgs> canExecuteHandler;

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    handler = assertionHandler;
                }

                public OneArgumentHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler) : this(executedAssertionHandler, null)
                {
                }

                public OneArgumentHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                    if (canExecuteAssertionHandler != null)
                    {
                        canExecuteHandler = e =>
                        {
                            e.CanExecute = true;
                            canExecuteAssertionHandler(e);
                        };
                    }
                }

                public object DataContext => dataContext;
                public FrameworkElement Element => element;
                public FrameworkElement ChildElement => childElement;
            }

            public class RoutedEventHandlerController
            {
                [DataContext]
                private object dataContext;

                [Element]
                private FrameworkElement element;

                [Element]
                private FrameworkElement childElement;

                [EventHandler(ElementName = nameof(childElement), Event = nameof(FrameworkElement.Loaded))]
                private RoutedEventHandler handler;

                public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
                {
                    handler = assertionHandler;
                }

                public object DataContext => dataContext;
                public FrameworkElement Element => element;
                public FrameworkElement ChildElement => childElement;
            }

            public class OneArgumentExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<ExecutedRoutedEventArgs> executedHandler;

                public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                }
            }

            public class OneArgumentExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<ExecutedRoutedEventArgs> executedHandler;

                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<CanExecuteRoutedEventArgs> canExecuteHandler;

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private Action<ExecutedRoutedEventArgs> anotherExecutedHandler;

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private Action<CanExecuteRoutedEventArgs> anotherCanExecuteHandler;

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                    canExecuteHandler = e =>
                    {
                        e.CanExecute = true;
                        canExecuteAssertionHandler(e);
                    };

                    anotherExecutedHandler = e => anotherExecutedAssertionHandler?.Invoke(e);
                    anotherCanExecuteHandler = e =>
                    {
                        e.CanExecute = true;
                        anotherCanExecuteAssertionHandler?.Invoke(e);
                    };
                }
            }

            public class ExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private ExecutedRoutedEventHandler executedHandler;

                public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                }
            }

            public class ExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private ExecutedRoutedEventHandler executedHandler;

                [CommandHandler(CommandName = nameof(TestCommand))]
                private CanExecuteRoutedEventHandler canExecuteHandler;

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private ExecutedRoutedEventHandler anotherExecutedHandler;

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private CanExecuteRoutedEventHandler anotherCanExecuteHandler;

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler)
                {
                    executedHandler = executedAssertionHandler;
                    canExecuteHandler = (s, e) =>
                    {
                        e.CanExecute = true;
                        canExecuteAssertionHandler(s, e);
                    };

                    anotherExecutedHandler = (s, e) => anotherExecutedAssertionHandler?.Invoke(s, e);
                    anotherCanExecuteHandler = (s, e) =>
                    {
                        e.CanExecute = true;
                        anotherCanExecuteAssertionHandler?.Invoke(s, e);
                    };
                }
            }
        }

        public class AttributedToProperty
        {
            public class NoArgumentHandlerController
            {
                [DataContext]
                public object DataContext { get; private set; }

                [Element(Name = "element")]
                public FrameworkElement Element { get; private set; }

                [Element]
                public FrameworkElement ChildElement { get; private set; }

                [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
                private Action Handler { get; set; }

                public NoArgumentHandlerController(Action assertionHandler)
                {
                    Handler = assertionHandler;
                }
            }

            public class OneArgumentHandlerController
            {
                [DataContext]
                public object DataContext { get; private set; }

                [Element(Name = "element")]
                public FrameworkElement Element { get; private set; }

                [Element]
                public FrameworkElement ChildElement { get; private set; }

                [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
                private Action<RoutedEventArgs> Handler { get; set; }

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    Handler = assertionHandler;
                }
            }

            public class RoutedEventHandlerController
            {
                [DataContext]
                public object DataContext { get; private set; }

                [Element(Name = "element")]
                public FrameworkElement Element { get; private set; }

                [Element]
                public FrameworkElement ChildElement { get; private set; }

                [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
                private RoutedEventHandler Handler { get; set; }

                public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
                {
                   Handler = assertionHandler;
                }
            }

            public class OneArgumentExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; set; }

                public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                }
            }

            public class OneArgumentExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; set; }

                [CommandHandler(CommandName = nameof(TestCommand))]
                private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; set; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private Action<ExecutedRoutedEventArgs> AnotherExecutedHandler { get; set; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private Action<CanExecuteRoutedEventArgs> AnotherCanExecuteHandler { get; set; }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                    CanExecuteHandler = e =>
                    {
                        e.CanExecute = true;
                        canExecuteAssertionHandler(e);
                    };

                    AnotherExecutedHandler = e => anotherExecutedAssertionHandler?.Invoke(e);
                    AnotherCanExecuteHandler = e =>
                    {
                        e.CanExecute = true;
                        anotherCanExecuteAssertionHandler?.Invoke(e);
                    };
                }
            }

            public class ExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private ExecutedRoutedEventHandler ExecutedHandler { get; set; }

                public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                }
            }

            public class ExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                private ExecutedRoutedEventHandler ExecutedHandler { get; set; }

                [CommandHandler(CommandName = nameof(TestCommand))]
                private CanExecuteRoutedEventHandler CanExecuteHandler { get; set; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private ExecutedRoutedEventHandler AnotherExecutedHandler { get; set; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                private CanExecuteRoutedEventHandler AnotherCanExecuteHandler { get; set; }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                    CanExecuteHandler = (s, e) =>
                    {
                        e.CanExecute = true;
                        canExecuteAssertionHandler(s, e);
                    };

                    AnotherExecutedHandler = (s, e) => anotherExecutedAssertionHandler?.Invoke(s, e);
                    AnotherCanExecuteHandler = (s, e) =>
                    {
                        e.CanExecute = true;
                        anotherCanExecuteAssertionHandler?.Invoke(s, e);
                    };
                }
            }
        }

        public class AttributedToMethod
        {
            public class NoArgumentHandlerController
            {
                [DataContext]
                public void SetDataContext(object dataContext) => DataContext = dataContext;
                public object DataContext { get; private set; }

                [Element(Name = "element")]
                public void SetElement(FrameworkElement element) => Element = element;
                public FrameworkElement Element { get; private set; }

                [Element]
                public void SetChildElement(FrameworkElement childElement) => ChildElement = childElement;
                public FrameworkElement ChildElement { get; private set; }

                [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
                public void ChildElement_Loaded() => handler();
                private readonly Action handler;

                public NoArgumentHandlerController(Action assertionHandler)
                {
                    handler = assertionHandler;
                }
            }

            public class OneArgumentHandlerController
            {
                [DataContext]
                public void SetDataContext(object dataContext) => DataContext = dataContext;
                public object DataContext { get; private set; }

                [Element(Name = "element")]
                public void SetElement(FrameworkElement element) => Element = element;
                public FrameworkElement Element { get; private set; }

                [Element]
                public void SetChildElement(FrameworkElement childElement) => ChildElement = childElement;
                public FrameworkElement ChildElement { get; private set; }

                [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
                public void ChildElement_Loaded(RoutedEventArgs e) => handler(e);
                private readonly Action<RoutedEventArgs> handler;

                public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
                {
                    handler = assertionHandler;
                }
            }

            public class RoutedEventHandlerController
            {
                [DataContext]
                public void SetDataContext(object dataContext) => DataContext = dataContext;
                public object DataContext { get; private set; }

                [Element(Name = "element")]
                public void SetElement(FrameworkElement element) => Element = element;
                public FrameworkElement Element { get; private set; }

                [Element]
                public void SetChildElement(FrameworkElement childElement) => ChildElement = childElement;
                public FrameworkElement ChildElement { get; private set; }

                [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
                public void ChildElement_Loaded(object sender, RoutedEventArgs e) => handler(sender, e);
                private readonly RoutedEventHandler handler;

                public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
                {
                    handler = assertionHandler;
                }
            }

            public class OneArgumentExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                public void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
                private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

                public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                }
            }

            public class OneArgumentExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                public void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
                private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

                [CommandHandler(CommandName = nameof(TestCommand))]
                public void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
                {
                    e.CanExecute = true;
                    CanExecuteHandler?.Invoke(e);
                }
                private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                public void AnotherTestCommand_Executed(ExecutedRoutedEventArgs e) => AnotherExecutedHandler?.Invoke(e);
                private Action<ExecutedRoutedEventArgs> AnotherExecutedHandler { get; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                public void AnotherTestCommand_CanExecute(CanExecuteRoutedEventArgs e)
                {
                    e.CanExecute = true;
                    AnotherCanExecuteHandler?.Invoke(e);
                }
                private Action<CanExecuteRoutedEventArgs> AnotherCanExecuteHandler { get; }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                    CanExecuteHandler = canExecuteAssertionHandler;

                    AnotherExecutedHandler = e => anotherExecutedAssertionHandler?.Invoke(e);
                    AnotherCanExecuteHandler = e => anotherCanExecuteAssertionHandler?.Invoke(e);
                }
            }

            public class ExecutedOnlyHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                public void TestCommand_Executed(object sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
                private ExecutedRoutedEventHandler ExecutedHandler { get; }

                public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                }
            }

            public class ExecutedAndCanExecuteHandlerController
            {
                [CommandHandler(CommandName = nameof(TestCommand))]
                public void TestCommand_Executed(object sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
                private ExecutedRoutedEventHandler ExecutedHandler { get; }

                [CommandHandler(CommandName = nameof(TestCommand))]
                public void TestCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
                {
                    e.CanExecute = true;
                    CanExecuteHandler?.Invoke(sender, e);
                }
                private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                public void AnotherTestCommand_Executed(object sender, ExecutedRoutedEventArgs e) => AnotherExecutedHandler?.Invoke(sender, e);
                private ExecutedRoutedEventHandler AnotherExecutedHandler { get; }

                [CommandHandler(CommandName = nameof(AnotherTestCommand))]
                public void AnotherTestCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
                {
                    e.CanExecute = true;
                    AnotherCanExecuteHandler?.Invoke(sender, e);
                }
                private CanExecuteRoutedEventHandler AnotherCanExecuteHandler { get; }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler, null, null)
                {
                }

                public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler)
                {
                    ExecutedHandler = executedAssertionHandler;
                    CanExecuteHandler = canExecuteAssertionHandler;

                    AnotherExecutedHandler = (s, e) => anotherExecutedAssertionHandler?.Invoke(s, e);
                    AnotherCanExecuteHandler = (s, e) => anotherCanExecuteAssertionHandler?.Invoke(s, e);
                }
            }
        }
    }
}
