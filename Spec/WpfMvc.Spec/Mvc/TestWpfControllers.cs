// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Input;

namespace Charites.Windows.Mvc;

internal class TestWpfControllers
{
    public static readonly RoutedUICommand TestCommand = new("Test", nameof(TestCommand), typeof(TestWpfControllers));
    public static readonly RoutedUICommand AnotherTestCommand = new("Another Test", nameof(AnotherTestCommand), typeof(TestWpfControllers));

    public interface ITestWpfController
    {
        object? DataContext { get; }
        FrameworkElement? Element { get; }
        FrameworkElement? ChildElement { get; }
    }

    public class TestWpfControllerBase
    {
        [DataContext]
        public object? DataContext { get; set; }

        [Element]
        public FrameworkElement? Element { get; set; }

        [EventHandler(ElementName = nameof(Element), Event = nameof(FrameworkElement.Loaded))]
        protected void Element_Loaded() => LoadedAssertionHandler?.Invoke();

        [EventHandler(ElementName = nameof(Element), Event = "Button.Click")]
        protected void Element_ButtonClick() => ButtonClickAssertionHandler?.Invoke();

        [EventHandler(ElementName = nameof(Element), Event = nameof(TestElement.Changed))]
        protected void Element_Changed() => ChangedAssertionHandler?.Invoke();

        [EventHandler(Event = nameof(FrameworkElement.DataContextChanged))]
        protected void OnDataContextChanged() { }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
        protected void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedAssertionHandler?.Invoke();

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
        protected void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
        {
            CanExecuteAssertionHandler?.Invoke();
            e.CanExecute = true;
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
        protected void TestCommand_PreviewExecuted(ExecutedRoutedEventArgs e) => PreviewExecutedAssertionHandler?.Invoke();

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
        protected void TestCommand_PreviewCanExecute(CanExecuteRoutedEventArgs e)
        {
            PreviewCanExecuteAssertionHandler?.Invoke();
            e.CanExecute = true;
        }

        public Action? LoadedAssertionHandler { get; set; }
        public Action? ButtonClickAssertionHandler { get; set; }
        public Action? ChangedAssertionHandler { get; set; }
        public Action? ExecutedAssertionHandler { get; set; }
        public Action? CanExecuteAssertionHandler { get; set; }
        public Action? PreviewExecutedAssertionHandler { get; set; }
        public Action? PreviewCanExecuteAssertionHandler { get; set; }
    }

    [View(Key = "Charites.Windows.Mvc.TestDataContexts+TestDataContext")]
    public class TestWpfController : TestWpfControllerBase { }

    [View(Key = "Charites.Windows.Mvc.TestDataContexts+MultiTestDataContext")]
    public class MultiTestWpfControllerA : TestWpfControllerBase { }

    [View(Key = "Charites.Windows.Mvc.TestDataContexts+MultiTestDataContext")]
    public class MultiTestWpfControllerB : TestWpfControllerBase { }

    [View(Key = "Charites.Windows.Mvc.TestDataContexts+MultiTestDataContext")]
    public class MultiTestWpfControllerC : TestWpfControllerBase { }

    public class TestController {[DataContext] public object? DataContext { get; set; } }

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
        public object? DataContext { get; set; }

        [Element]
        public FrameworkElement? Element { get; set; }

        [EventHandler(ElementName = nameof(Element), Event = nameof(FrameworkElement.Loaded))]
        private async Task Element_LoadedAsync()
        {
            await Task.Run(() => LoadedAssertionHandler?.Invoke());
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
        private async Task TestCommand_ExecutedAsync(ExecutedRoutedEventArgs e)
        {
            await Task.Run(() => ExecutedAssertionHandler?.Invoke());
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
        private async Task TestCommand_CanExecuteAsync(CanExecuteRoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                CanExecuteAssertionHandler?.Invoke();
                e.CanExecute = true;
            });
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
        private async Task TestCommand_PreviewExecutedAsync(ExecutedRoutedEventArgs e)
        {
            await Task.Run(() => PreviewExecutedAssertionHandler?.Invoke());
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
        private async Task TestCommand_PreviewCanExecuteAsync(CanExecuteRoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                PreviewCanExecuteAssertionHandler?.Invoke();
                e.CanExecute = true;
            });
        }

        public Action? LoadedAssertionHandler { get; set; }
        public Action? ExecutedAssertionHandler { get; set; }
        public Action? CanExecuteAssertionHandler { get; set; }
        public Action? PreviewExecutedAssertionHandler { get; set; }
        public Action? PreviewCanExecuteAssertionHandler { get; set; }
    }

    public class ExceptionTestWpfController
    {
        [EventHandler(Event = "Changed")]
        private void OnChanged()
        {
            throw new Exception();
        }
    }

    public class CommandHandlerWithAttributedParametersController
    {
        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
        private void TestCommand_Executed(ExecutedRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            ExecutedAssertionHandler?.Invoke();
            ExecutedAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
        private void TestCommand_CanExecute(CanExecuteRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            CanExecuteAssertionHandler?.Invoke();
            CanExecuteAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
            e.CanExecute = true;
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
        private void TestCommand_PreviewExecuted(ExecutedRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            PreviewExecutedAssertionHandler?.Invoke();
            PreviewExecutedAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
        private void TestCommand_PreviewCanExecute(CanExecuteRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            PreviewCanExecuteAssertionHandler?.Invoke();
            PreviewCanExecuteAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
            e.CanExecute = true;
        }

        public Action? ExecutedAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? ExecutedAttributedArgumentsHandler { get; set; }
        public Action? CanExecuteAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? CanExecuteAttributedArgumentsHandler { get; set; }
        public Action? PreviewExecutedAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? PreviewExecutedAttributedArgumentsHandler { get; set; }
        public Action? PreviewCanExecuteAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? PreviewCanExecuteAttributedArgumentsHandler { get; set; }
    }

    public class CommandHandlerWithAttributedParametersControllerAsync
    {
        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
        private async Task TestCommand_ExecutedAsync(ExecutedRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement(Name = "Element")] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            ExecutedAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
            await Task.Run(() =>
            {
                ExecutedAssertionHandler?.Invoke();
            });
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
        private async Task TestCommand_CanExecuteAsync(CanExecuteRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement(Name = "Element")] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            CanExecuteAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
            await Task.Run(() =>
            {
                CanExecuteAssertionHandler?.Invoke();
                e.CanExecute = true;
            });
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
        private async Task TestCommand_PreviewExecutedAsync(ExecutedRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement(Name = "Element")] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            PreviewExecutedAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
            await Task.Run(() =>
            {
                PreviewExecutedAssertionHandler?.Invoke();
            });
        }

        [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
        private async Task TestCommand_PreviewCanExecuteAsync(CanExecuteRoutedEventArgs e, [FromDI] IDependency1 dependency1, [FromDI] IDependency2 dependency2, [FromDI] IDependency3 dependency3, [FromElement(Name = "Element")] TestElement element, [FromDataContext] TestDataContexts.TestDataContext dataContext)
        {
            PreviewCanExecuteAttributedArgumentsHandler?.Invoke(dependency1, dependency2, dependency3, element, dataContext);
            await Task.Run(() =>
            {
                PreviewCanExecuteAssertionHandler?.Invoke();
                e.CanExecute = true;
            });
        }

        public Action? ExecutedAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? ExecutedAttributedArgumentsHandler { get; set; }
        public Action? CanExecuteAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? CanExecuteAttributedArgumentsHandler { get; set; }
        public Action? PreviewExecutedAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? PreviewExecutedAttributedArgumentsHandler { get; set; }
        public Action? PreviewCanExecuteAssertionHandler { get; set; }
        public Action<IDependency1, IDependency2, IDependency3, TestElement, TestDataContexts.TestDataContext>? PreviewCanExecuteAttributedArgumentsHandler { get; set; }
    }

    public interface IDependency1 {}
    public interface IDependency2 { }
    public interface IDependency3 { }
    public class Dependency1 : IDependency1 { }
    public class Dependency2 : IDependency2 { }
    public class Dependency3 : IDependency3 { }

    public class AttributedToField
    {
        public class NoArgumentHandlerController : ITestWpfController
        {
            [DataContext]
            private object? dataContext;

            [Element]
            private FrameworkElement? element;

            [Element]
            private FrameworkElement? childElement;

            [EventHandler(ElementName = nameof(childElement), Event = nameof(FrameworkElement.Loaded))]
            private Action handler;

            public NoArgumentHandlerController(Action assertionHandler)
            {
                handler = assertionHandler;
            }

            public object? DataContext => dataContext;
            public FrameworkElement? Element => element;
            public FrameworkElement? ChildElement => childElement;
        }

        public class OneArgumentHandlerController : ITestWpfController
        {
            [DataContext]
            private object? dataContext;

            [Element]
            private FrameworkElement? element;

            [Element]
            private FrameworkElement? childElement;

            [EventHandler(ElementName = nameof(childElement), Event = nameof(FrameworkElement.Loaded))]
            private Action<RoutedEventArgs> handler;

            public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
            {
                handler = assertionHandler;
            }

            public object? DataContext => dataContext;
            public FrameworkElement? Element => element;
            public FrameworkElement? ChildElement => childElement;
        }

        public class RoutedEventHandlerController : ITestWpfController
        {
            [DataContext]
            private object? dataContext;

            [Element]
            private FrameworkElement? element;

            [Element]
            private FrameworkElement? childElement;

            [EventHandler(ElementName = nameof(childElement), Event = nameof(FrameworkElement.Loaded))]
            private RoutedEventHandler handler;

            public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
            {
                handler = assertionHandler;
            }

            public object? DataContext => dataContext;
            public FrameworkElement? Element => element;
            public FrameworkElement? ChildElement => childElement;
        }

        public class NoArgumentExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action executedHandler;

            public NoArgumentExecutedOnlyHandlerController(Action executedAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action previewExecutedHandler;

            public NoArgumentExecutedAndPreviewExecutedHandlerController(Action executedAssertionHandler, Action previewExecutedAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                previewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action canExecuteHandler;

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private Action? anotherExecutedHandler;

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action? anotherCanExecuteHandler;

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                canExecuteHandler = canExecuteAssertionHandler;
            }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action anotherExecutedAssertionHandler, Action anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                anotherExecutedHandler = anotherExecutedAssertionHandler;
                anotherCanExecuteHandler = anotherCanExecuteAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action canExecuteHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action? previewExecutedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private Action? previewCanExecuteHandler;

            public NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action previewExecutedAssertionHandler, Action previewCanExecuteAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                canExecuteHandler = canExecuteAssertionHandler;
                previewExecutedHandler = previewExecutedAssertionHandler;
                previewCanExecuteHandler = previewCanExecuteAssertionHandler;
            }
        }

        public class OneArgumentExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> executedHandler;

            public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action<ExecutedRoutedEventArgs> previewExecutedHandler;

            public OneArgumentExecutedAndPreviewExecutedHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                previewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action<CanExecuteRoutedEventArgs> canExecuteHandler;

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs>? anotherExecutedHandler;

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action<CanExecuteRoutedEventArgs>? anotherCanExecuteHandler;

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                canExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(e);
                };
            }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                anotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                anotherCanExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    anotherCanExecuteAssertionHandler.Invoke(e);
                };
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action<CanExecuteRoutedEventArgs> canExecuteHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action<ExecutedRoutedEventArgs>? previewExecutedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private Action<CanExecuteRoutedEventArgs>? previewCanExecuteHandler;

            public OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> previewCanExecuteAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                canExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(e);
                };
                previewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                previewCanExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    previewCanExecuteAssertionHandler.Invoke(e);
                };
            }
        }

        public class ExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler executedHandler;

            public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
            }
        }

        public class ExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private ExecutedRoutedEventHandler previewExecutedHandler;

            public ExecutedAndPreviewExecutedHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                previewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class ExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private CanExecuteRoutedEventHandler canExecuteHandler;

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler? anotherExecutedHandler;

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private CanExecuteRoutedEventHandler? anotherCanExecuteHandler;

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                canExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(s, e);
                };
            }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                anotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                anotherCanExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    anotherCanExecuteAssertionHandler.Invoke(s, e);
                };
            }
        }

        public class ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler executedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private CanExecuteRoutedEventHandler canExecuteHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private ExecutedRoutedEventHandler? previewExecutedHandler;

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private CanExecuteRoutedEventHandler? previewCanExecuteHandler;

            public ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler, CanExecuteRoutedEventHandler previewCanExecuteAssertionHandler)
            {
                executedHandler = executedAssertionHandler;
                canExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(s, e);
                };
                previewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                previewCanExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    previewCanExecuteAssertionHandler.Invoke(s, e);
                };
            }
        }
    }

    public class AttributedToProperty
    {
        public class NoArgumentHandlerController : ITestWpfController
        {
            [DataContext]
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public FrameworkElement? Element { get; private set; }

            [Element]
            public FrameworkElement? ChildElement { get; private set; }

            [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
            private Action Handler { get; }

            public NoArgumentHandlerController(Action assertionHandler)
            {
                Handler = assertionHandler;
            }
        }

        public class OneArgumentHandlerController : ITestWpfController
        {
            [DataContext]
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public FrameworkElement? Element { get; private set; }

            [Element]
            public FrameworkElement? ChildElement { get; private set; }

            [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
            private Action<RoutedEventArgs> Handler { get; }

            public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
            {
                Handler = assertionHandler;
            }
        }

        public class RoutedEventHandlerController : ITestWpfController
        {
            [DataContext]
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public FrameworkElement? Element { get; private set; }

            [Element]
            public FrameworkElement? ChildElement { get; private set; }

            [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
            private RoutedEventHandler Handler { get; }

            public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
            {
                Handler = assertionHandler;
            }
        }

        public class NoArgumentExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action ExecutedHandler { get; }

            public NoArgumentExecutedOnlyHandlerController(Action executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action ExecutedHandler { get; }
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action PreviewExecutedHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedHandlerController(Action executedAssertionHandler, Action previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private Action? AnotherExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action? AnotherCanExecuteHandler { get; }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action anotherExecutedAssertionHandler, Action anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action? PreviewExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private Action? PreviewCanExecuteHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action previewExecutedAssertionHandler, Action previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler;
            }
        }

        public class OneArgumentExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action<ExecutedRoutedEventArgs> PreviewExecutedHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs>? AnotherExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action<CanExecuteRoutedEventArgs>? AnotherCanExecuteHandler { get; }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(e);
                };
            }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    anotherCanExecuteAssertionHandler.Invoke(e);
                };
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private Action<ExecutedRoutedEventArgs>? PreviewExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private Action<CanExecuteRoutedEventArgs>? PreviewCanExecuteHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(e);
                };
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = e =>
                {
                    e.CanExecute = true;
                    previewCanExecuteAssertionHandler.Invoke(e);
                };
            }
        }

        public class ExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class ExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private ExecutedRoutedEventHandler PreviewExecutedHandler { get; }

            public ExecutedAndPreviewExecutedHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class ExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler? AnotherExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private CanExecuteRoutedEventHandler? AnotherCanExecuteHandler { get; }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(s, e);
                };
            }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    anotherCanExecuteAssertionHandler.Invoke(s, e);
                };
            }
        }

        public class ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private ExecutedRoutedEventHandler? PreviewExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private CanExecuteRoutedEventHandler? PreviewCanExecuteHandler { get; }

            public ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler, CanExecuteRoutedEventHandler previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    canExecuteAssertionHandler(s, e);
                };
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = (s, e) =>
                {
                    e.CanExecute = true;
                    previewCanExecuteAssertionHandler.Invoke(s, e);
                };
            }
        }
    }

    public class AttributedToMethod
    {
        public class NoArgumentHandlerController : ITestWpfController
        {
            [DataContext]
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
            public void ChildElement_Loaded() => handler();
            private readonly Action handler;

            public NoArgumentHandlerController(Action assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class OneArgumentHandlerController : ITestWpfController
        {
            [DataContext]
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
            public void ChildElement_Loaded(RoutedEventArgs e) => handler(e);
            private readonly Action<RoutedEventArgs> handler;

            public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class RoutedEventHandlerController : ITestWpfController
        {
            [DataContext]
            public void SetDataContext(object dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            [EventHandler(ElementName = nameof(ChildElement), Event = nameof(FrameworkElement.Loaded))]
            public void ChildElement_Loaded(object? sender, RoutedEventArgs e) => handler(sender, e);
            private readonly RoutedEventHandler handler;

            public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class NoArgumentExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            public NoArgumentExecutedOnlyHandlerController(Action executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private void TestCommand_CanExecute() => CanExecuteHandler();
            private Action CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            private void AnotherTestCommand_Executed() => AnotherExecutedHandler?.Invoke();
            private Action? AnotherExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            private void AnotherTestCommand_CanExecute() => AnotherCanExecuteHandler?.Invoke();

            private Action? AnotherCanExecuteHandler { get; }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action anotherExecutedAssertionHandler, Action anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private void TestCommand_PreviewExecuted() => PreviewExecutedHandler();
            private Action PreviewExecutedHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedHandlerController(Action executedAssertionHandler, Action previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            private void TestCommand_CanExecute() => CanExecuteHandler();
            private Action CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            private void AnotherTestCommand_PreviewExecuted() => PreviewExecutedHandler?.Invoke();
            private Action? PreviewExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            private void AnotherTestCommand_PreviewCanExecute() => PreviewCanExecuteHandler?.Invoke();

            private Action? PreviewCanExecuteHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action previewExecutedAssertionHandler, Action previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler;
            }
        }

        public class OneArgumentExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            public void TestCommand_PreviewExecuted(ExecutedRoutedEventArgs e) => PreviewExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> PreviewExecutedHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            public void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            public void AnotherTestCommand_Executed(ExecutedRoutedEventArgs e) => AnotherExecutedHandler?.Invoke(e);
            private Action<ExecutedRoutedEventArgs>? AnotherExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            public void AnotherTestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                AnotherCanExecuteHandler?.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs>? AnotherCanExecuteHandler { get; }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler.Invoke;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            public void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            public void TestCommand_PreviewExecuted(ExecutedRoutedEventArgs e) => PreviewExecutedHandler?.Invoke(e);
            private Action<ExecutedRoutedEventArgs>? PreviewExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            public void TestCommand_PreviewCanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                PreviewCanExecuteHandler?.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs>? PreviewCanExecuteHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler.Invoke;
            }
        }

        public class ExecutedOnlyHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class ExecutedAndPreviewExecutedHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            public void TestCommand_PreviewExecuted(object? sender, ExecutedRoutedEventArgs e) => PreviewExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler PreviewExecutedHandler { get; }

            public ExecutedAndPreviewExecutedHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class ExecutedAndCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            public void TestCommand_CanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.Executed))]
            public void AnotherTestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => AnotherExecutedHandler?.Invoke(sender, e);
            private ExecutedRoutedEventHandler? AnotherExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(AnotherTestCommand), Event = nameof(CommandBinding.CanExecute))]
            public void AnotherTestCommand_CanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                AnotherCanExecuteHandler?.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler? AnotherCanExecuteHandler { get; }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler.Invoke;
            }
        }

        public class ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.Executed))]
            public void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.CanExecute))]
            public void TestCommand_CanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewExecuted))]
            public void TestCommand_PreviewExecuted(object? sender, ExecutedRoutedEventArgs e) => PreviewExecutedHandler?.Invoke(sender, e);
            private ExecutedRoutedEventHandler? PreviewExecutedHandler { get; }

            [CommandHandler(CommandName = nameof(TestCommand), Event = nameof(CommandBinding.PreviewCanExecute))]
            public void TestCommand_PreviewCanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                PreviewCanExecuteHandler?.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler? PreviewCanExecuteHandler { get; }

            public ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler, CanExecuteRoutedEventHandler previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler.Invoke;
            }
        }
    }

    public class AttributedToMethodUsingNamingConvention
    {
        public class NoArgumentHandlerController : ITestWpfController
        {
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            public void ChildElement_Loaded() => handler();
            private readonly Action handler;

            public NoArgumentHandlerController(Action assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class OneArgumentHandlerController : ITestWpfController
        {
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            public void ChildElement_Loaded(RoutedEventArgs e) => handler(e);
            private readonly Action<RoutedEventArgs> handler;

            public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class RoutedEventHandlerController : ITestWpfController
        {
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            public void ChildElement_Loaded(object? sender, RoutedEventArgs e) => handler(sender, e);
            private readonly RoutedEventHandler handler;

            public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class NoArgumentExecutedOnlyHandlerController
        {
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            public NoArgumentExecutedOnlyHandlerController(Action executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedHandlerController
        {
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            private void TestCommand_PreviewExecuted() => PreviewExecutedHandler();
            private Action PreviewExecutedHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedHandlerController(Action executedAssertionHandler, Action previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndCanExecuteHandlerController
        {
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            private void TestCommand_CanExecute() => CanExecuteHandler();
            private Action CanExecuteHandler { get; }

            private void AnotherTestCommand_Executed() => AnotherExecutedHandler?.Invoke();
            private Action? AnotherExecutedHandler { get; }

            private void AnotherTestCommand_CanExecute() => AnotherCanExecuteHandler?.Invoke();
            private Action? AnotherCanExecuteHandler { get; }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action anotherExecutedAssertionHandler, Action anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            private void TestCommand_Executed() => ExecutedHandler();
            private Action ExecutedHandler { get; }

            private void TestCommand_CanExecute() => CanExecuteHandler();
            private Action CanExecuteHandler { get; }

            private void TestCommand_PreviewExecuted() => PreviewExecutedHandler?.Invoke();
            private Action? PreviewExecutedHandler { get; }

            private void TestCommand_PreviewCanExecute() => PreviewCanExecuteHandler?.Invoke();
            private Action? PreviewCanExecuteHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action previewExecutedAssertionHandler, Action previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler;
            }
        }

        public class OneArgumentExecutedOnlyHandlerController
        {
            private void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedHandlerController
        {
            private void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            private void TestCommand_PreviewExecuted(ExecutedRoutedEventArgs e) => PreviewExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> PreviewExecutedHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndCanExecuteHandlerController
        {
            private void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            private void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            private void AnotherTestCommand_Executed(ExecutedRoutedEventArgs e) => AnotherExecutedHandler?.Invoke(e);
            private Action<ExecutedRoutedEventArgs>? AnotherExecutedHandler { get; }

            private void AnotherTestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                AnotherCanExecuteHandler?.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs>? AnotherCanExecuteHandler { get; }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler.Invoke;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            private void TestCommand_Executed(ExecutedRoutedEventArgs e) => ExecutedHandler(e);
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            private void TestCommand_CanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            private void TestCommand_PreviewExecuted(ExecutedRoutedEventArgs e) => PreviewExecutedHandler?.Invoke(e);
            private Action<ExecutedRoutedEventArgs>? PreviewExecutedHandler { get; }

            private void TestCommand_PreviewCanExecute(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                PreviewCanExecuteHandler?.Invoke(e);
            }
            private Action<CanExecuteRoutedEventArgs>? PreviewCanExecuteHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler.Invoke;
            }
        }

        public class ExecutedOnlyHandlerController
        {
            private void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class ExecutedAndPreviewExecutedHandlerController
        {
            private void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            private void TestCommand_PreviewExecuted(object? sender, ExecutedRoutedEventArgs e) => PreviewExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler PreviewExecutedHandler { get; }

            public ExecutedAndPreviewExecutedHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class ExecutedAndCanExecuteHandlerController
        {
            private void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            private void TestCommand_CanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            private void AnotherTestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => AnotherExecutedHandler?.Invoke(sender, e);
            private ExecutedRoutedEventHandler? AnotherExecutedHandler { get; }

            private void AnotherTestCommand_CanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                AnotherCanExecuteHandler?.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler? AnotherCanExecuteHandler { get; }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler.Invoke;
            }
        }

        public class ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            private void TestCommand_Executed(object? sender, ExecutedRoutedEventArgs e) => ExecutedHandler(sender, e);
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            private void TestCommand_CanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            private void TestCommand_PreviewExecuted(object? sender, ExecutedRoutedEventArgs e) => PreviewExecutedHandler?.Invoke(sender, e);
            private ExecutedRoutedEventHandler? PreviewExecutedHandler { get; }

            private void TestCommand_PreviewCanExecute(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                PreviewCanExecuteHandler?.Invoke(sender, e);
            }
            private CanExecuteRoutedEventHandler? PreviewCanExecuteHandler { get; }

            public ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler, CanExecuteRoutedEventHandler previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler.Invoke;
            }
        }
    }

    public class AttributedToAsyncMethodUsingNamingConvention
    {
        public class NoArgumentHandlerController : ITestWpfController
        {
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            public Task ChildElement_LoadedAsync()
            {
                handler();
                return Task.CompletedTask;
            }
            private readonly Action handler;

            public NoArgumentHandlerController(Action assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class OneArgumentHandlerController : ITestWpfController
        {
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            public Task ChildElement_LoadedAsync(RoutedEventArgs e)
            {
                handler(e);
                return Task.CompletedTask;
            }
            private readonly Action<RoutedEventArgs> handler;

            public OneArgumentHandlerController(Action<RoutedEventArgs> assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class RoutedEventHandlerController : ITestWpfController
        {
            public void SetDataContext(object? dataContext) => DataContext = dataContext;
            public object? DataContext { get; private set; }

            [Element(Name = "element")]
            public void SetElement(FrameworkElement? element) => Element = element;
            public FrameworkElement? Element { get; private set; }

            [Element]
            public void SetChildElement(FrameworkElement? childElement) => ChildElement = childElement;
            public FrameworkElement? ChildElement { get; private set; }

            public Task ChildElement_LoadedAsync(object? sender, RoutedEventArgs e)
            {
                handler(sender, e);
                return Task.CompletedTask;
            }
            private readonly RoutedEventHandler handler;

            public RoutedEventHandlerController(RoutedEventHandler assertionHandler)
            {
                handler = assertionHandler;
            }
        }

        public class NoArgumentExecutedOnlyHandlerController
        {
            private Task TestCommand_ExecutedAsync()
            {
                ExecutedHandler();
                return Task.CompletedTask;
            }
            private Action ExecutedHandler { get; }

            public NoArgumentExecutedOnlyHandlerController(Action executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedHandlerController
        {
            private Task TestCommand_ExecutedAsync()
            {
                ExecutedHandler();
                return Task.CompletedTask;
            }
            private Action ExecutedHandler { get; }

            private Task TestCommand_PreviewExecutedAsync()
            {
                PreviewExecutedHandler();
                return Task.CompletedTask;
            }
            private Action PreviewExecutedHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedHandlerController(Action executedAssertionHandler, Action previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndCanExecuteHandlerController
        {
            private Task TestCommand_ExecutedAsync()
            {
                ExecutedHandler();
                return Task.CompletedTask;
            }
            private Action ExecutedHandler { get; }

            private Task TestCommand_CanExecuteAsync()
            {
                CanExecuteHandler();
                return Task.CompletedTask;
            }
            private Action CanExecuteHandler { get; }

            private Task AnotherTestCommand_ExecutedAsync()
            {
                AnotherExecutedHandler?.Invoke();
                return Task.CompletedTask;
            }
            private Action? AnotherExecutedHandler { get; }

            private Task AnotherTestCommand_CanExecuteAsync()
            {
                AnotherCanExecuteHandler?.Invoke();
                return Task.CompletedTask;
            }
            private Action? AnotherCanExecuteHandler { get; }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public NoArgumentExecutedAndCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action anotherExecutedAssertionHandler, Action anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler;
            }
        }

        public class NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            private Task TestCommand_ExecutedAsync()
            {
                ExecutedHandler();
                return Task.CompletedTask;
            }
            private Action ExecutedHandler { get; }

            private Task TestCommand_CanExecuteAsync()
            {
                CanExecuteHandler();
                return Task.CompletedTask;
            }
            private Action CanExecuteHandler { get; }

            private Task TestCommand_PreviewExecutedAsync()
            {
                PreviewExecutedHandler?.Invoke();
                return Task.CompletedTask;
            }
            private Action? PreviewExecutedHandler { get; }

            private Task TestCommand_PreviewCanExecuteAsync()
            {
                PreviewCanExecuteHandler?.Invoke();
                return Task.CompletedTask;
            }
            private Action? PreviewCanExecuteHandler { get; }

            public NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action executedAssertionHandler, Action canExecuteAssertionHandler, Action previewExecutedAssertionHandler, Action previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler;
            }
        }

        public class OneArgumentExecutedOnlyHandlerController
        {
            private Task TestCommand_ExecutedAsync(ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            public OneArgumentExecutedOnlyHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedHandlerController
        {
            private Task TestCommand_ExecutedAsync(ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            private Task TestCommand_PreviewExecutedAsync(ExecutedRoutedEventArgs e)
            {
                PreviewExecutedHandler(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs> PreviewExecutedHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class OneArgumentExecutedAndCanExecuteHandlerController
        {
            private Task TestCommand_ExecutedAsync(ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            private Task TestCommand_CanExecuteAsync(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(e);
                return Task.CompletedTask;
            }
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            private Task AnotherTestCommand_ExecutedAsync(ExecutedRoutedEventArgs e)
            {
                AnotherExecutedHandler?.Invoke(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs>? AnotherExecutedHandler { get; }

            private Task AnotherTestCommand_CanExecuteAsync(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                AnotherCanExecuteHandler?.Invoke(e);
                return Task.CompletedTask;
            }
            private Action<CanExecuteRoutedEventArgs>? AnotherCanExecuteHandler { get; }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public OneArgumentExecutedAndCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> anotherExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler.Invoke;
            }
        }

        public class OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            private Task TestCommand_ExecutedAsync(ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs> ExecutedHandler { get; }

            private Task TestCommand_CanExecuteAsync(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(e);
                return Task.CompletedTask;
            }
            private Action<CanExecuteRoutedEventArgs> CanExecuteHandler { get; }

            private Task TestCommand_PreviewExecutedAsync(ExecutedRoutedEventArgs e)
            {
                PreviewExecutedHandler?.Invoke(e);
                return Task.CompletedTask;
            }
            private Action<ExecutedRoutedEventArgs>? PreviewExecutedHandler { get; }

            private Task TestCommand_PreviewCanExecuteAsync(CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                PreviewCanExecuteHandler?.Invoke(e);
                return Task.CompletedTask;
            }
            private Action<CanExecuteRoutedEventArgs>? PreviewCanExecuteHandler { get; }

            public OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(Action<ExecutedRoutedEventArgs> executedAssertionHandler, Action<CanExecuteRoutedEventArgs> canExecuteAssertionHandler, Action<ExecutedRoutedEventArgs> previewExecutedAssertionHandler, Action<CanExecuteRoutedEventArgs> previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler.Invoke;
            }
        }

        public class ExecutedOnlyHandlerController
        {
            private Task TestCommand_ExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            public ExecutedOnlyHandlerController(ExecutedRoutedEventHandler executedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
            }
        }

        public class ExecutedAndPreviewExecutedHandlerController
        {
            private Task TestCommand_ExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            private Task TestCommand_PreviewExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                PreviewExecutedHandler(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler PreviewExecutedHandler { get; }

            public ExecutedAndPreviewExecutedHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler;
            }
        }

        public class ExecutedAndCanExecuteHandlerController
        {
            private Task TestCommand_ExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            private Task TestCommand_CanExecuteAsync(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(sender, e);
                return Task.CompletedTask;
            }
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            private Task AnotherTestCommand_ExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                AnotherExecutedHandler?.Invoke(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler? AnotherExecutedHandler { get; }

            private Task AnotherTestCommand_CanExecuteAsync(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                AnotherCanExecuteHandler?.Invoke(sender, e);
                return Task.CompletedTask;
            }
            private CanExecuteRoutedEventHandler? AnotherCanExecuteHandler { get; }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
            }

            public ExecutedAndCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler anotherExecutedAssertionHandler, CanExecuteRoutedEventHandler anotherCanExecuteAssertionHandler) : this(executedAssertionHandler, canExecuteAssertionHandler)
            {
                AnotherExecutedHandler = anotherExecutedAssertionHandler.Invoke;
                AnotherCanExecuteHandler = anotherCanExecuteAssertionHandler.Invoke;
            }
        }

        public class ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController
        {
            private Task TestCommand_ExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                ExecutedHandler(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler ExecutedHandler { get; }

            private Task TestCommand_CanExecuteAsync(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                CanExecuteHandler.Invoke(sender, e);
                return Task.CompletedTask;
            }
            private CanExecuteRoutedEventHandler CanExecuteHandler { get; }

            private Task TestCommand_PreviewExecutedAsync(object? sender, ExecutedRoutedEventArgs e)
            {
                PreviewExecutedHandler?.Invoke(sender, e);
                return Task.CompletedTask;
            }
            private ExecutedRoutedEventHandler? PreviewExecutedHandler { get; }

            private Task TestCommand_PreviewCanExecuteAsync(object? sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = true;
                PreviewCanExecuteHandler?.Invoke(sender, e);
                return Task.CompletedTask;
            }
            private CanExecuteRoutedEventHandler? PreviewCanExecuteHandler { get; }

            public ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedRoutedEventHandler executedAssertionHandler, CanExecuteRoutedEventHandler canExecuteAssertionHandler, ExecutedRoutedEventHandler previewExecutedAssertionHandler, CanExecuteRoutedEventHandler previewCanExecuteAssertionHandler)
            {
                ExecutedHandler = executedAssertionHandler;
                CanExecuteHandler = canExecuteAssertionHandler;
                PreviewExecutedHandler = previewExecutedAssertionHandler.Invoke;
                PreviewCanExecuteHandler = previewCanExecuteAssertionHandler.Invoke;
            }
        }
    }
}