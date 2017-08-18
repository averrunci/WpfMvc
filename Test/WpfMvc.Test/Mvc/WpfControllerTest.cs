// Copyright (C) 2016-2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Runners;

namespace Fievus.Windows.Mvc.WpfControllerTest
{
    namespace RoutedEventHandlerAndDataContextAndElementInjection
    {
        [TestFixture]
        public class AttributedToField
        {
            [Test]
            public void AddsEventHandlerThatDoesNotHaveArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "childElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<Action>();
                    var controller = new TestWpfControllers.AttributedToField.NoArgumentHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    childElement.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement });

                    assertionHandler.AssertWasCalled(h => h.Invoke());
                }).Shutdown();
            }

            [Test]
            public void AddsEventHandlerThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "childElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<Action<RoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToField.OneArgumentHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement };
                    childElement.RaiseEvent(args);

                    assertionHandler.AssertWasCalled(h => h.Invoke(args));
                }).Shutdown();
            }

            [Test]
            public void AddsEventHandlerThaIsRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "childElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<RoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToField.RoutedEventHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement };
                    childElement.RaiseEvent(args);

                    assertionHandler.AssertWasCalled(h => h.Invoke(childElement, args));
                }).Shutdown();
            }

            [Test]
            public void SetsElementIndependently()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "childElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var controller = new TestWpfControllers.AttributedToField.NoArgumentHandlerController(null);

                    WpfController.SetElement(element, controller, true);
                    WpfController.SetElement(childElement, controller, true);

                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));
                }).Shutdown();
            }
        }

        [TestFixture]
        public class AttributedToProperty
        {
            [Test]
            public void AddsEventHandlerThatDoesNotHaveArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<Action>();
                    var controller = new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    childElement.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement });

                    assertionHandler.AssertWasCalled(h => h.Invoke());
                }).Shutdown();
            }

            [Test]
            public void AddsEventHandlerThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<Action<RoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToProperty.OneArgumentHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement };
                    childElement.RaiseEvent(args);

                    assertionHandler.AssertWasCalled(h => h.Invoke(args));
                }).Shutdown();
            }

            [Test]
            public void AddsEventHandlerThaIsRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<RoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToProperty.RoutedEventHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement };
                    childElement.RaiseEvent(args);

                    assertionHandler.AssertWasCalled(h => h.Invoke(childElement, args));
                }).Shutdown();
            }

            [Test]
            public void SetsElementIndependently()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var controller = new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(null);

                    WpfController.SetElement(element, controller, true);
                    WpfController.SetElement(childElement, controller, true);

                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));
                }).Shutdown();
            }
        }

        [TestFixture]
        public class AttributedToMethod
        {
            [Test]
            public void AddsEventHandlerThatDoesNotHaveArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<Action>();
                    var controller = new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    childElement.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement });

                    assertionHandler.AssertWasCalled(h => h.Invoke());
                }).Shutdown();
            }

            [Test]
            public void AddsEventHandlerThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<Action<RoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToMethod.OneArgumentHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement };
                    childElement.RaiseEvent(args);

                    assertionHandler.AssertWasCalled(h => h.Invoke(args));
                }).Shutdown();
            }

            [Test]
            public void AddsEventHandlerThaIsRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var assertionHandler = MockRepository.GenerateMock<RoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToMethod.RoutedEventHandlerController(assertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.Null);
                    Assert.That(controller.ChildElement, Is.Null);

                    element.RaiseInitialized();

                    Assert.That(controller.Context, Is.EqualTo(context));
                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));

                    var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = childElement };
                    childElement.RaiseEvent(args);

                    assertionHandler.AssertWasCalled(h => h.Invoke(childElement, args));
                }).Shutdown();
            }

            [Test]
            public void SetsElementIndependently()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var childElement = new FrameworkElement { Name = "ChildElement" };
                    var element = new TestElement { Name = "element", Content = childElement, DataContext = context };

                    var controller = new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(null);

                    WpfController.SetElement(element, controller, true);
                    WpfController.SetElement(childElement, controller, true);

                    Assert.That(controller.Element, Is.EqualTo(element));
                    Assert.That(controller.ChildElement, Is.EqualTo(childElement));
                }).Shutdown();
            }
        }
    }

    namespace CommandHandlerInjection
    {
        [TestFixture]
        public class AttributedToField
        {
            [Test]
            public void AddsCommandHandlerOfExecutedEventThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<Action<ExecutedRoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedOnlyHandlerController(executedAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventAndCanExecuteEventThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<Action<ExecutedRoutedEventArgs>>();
                    var canExecuteAssertionHandler = MockRepository.GenerateMock<Action<CanExecuteRoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(executedAssertionHandler, canExecuteAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventThaIsExecutedRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToField.ExecutedOnlyHandlerController(executedAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventAndCanExecuteEventThaIsExecutedRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                    var canExecuteAssertionHandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(executedAssertionHandler, canExecuteAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).CanExecute(parameter, element);
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }
        }

        [TestFixture]
        public class AttributedToProperty
        {
            [Test]
            public void AddsCommandHandlerOfExecutedEventThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<Action<ExecutedRoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedOnlyHandlerController(executedAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventAndCanExecuteEventThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<Action<ExecutedRoutedEventArgs>>();
                    var canExecuteAssertionHandler = MockRepository.GenerateMock<Action<CanExecuteRoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedAndCanExecuteHandlerController(executedAssertionHandler, canExecuteAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventThaIsExecutedRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToProperty.ExecutedOnlyHandlerController(executedAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventAndCanExecuteEventThaIsExecutedRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                    var canExecuteAssertionHandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToProperty.ExecutedAndCanExecuteHandlerController(executedAssertionHandler, canExecuteAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).CanExecute(parameter, element);
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }
        }

        [TestFixture]
        public class AttributedToMethod
        {
            [Test]
            public void AddsCommandHandlerOfExecutedEventThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<Action<ExecutedRoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedOnlyHandlerController(executedAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventAndCanExecuteEventThatHasOneArgument()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<Action<ExecutedRoutedEventArgs>>();
                    var canExecuteAssertionHandler = MockRepository.GenerateMock<Action<CanExecuteRoutedEventArgs>>();
                    var controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedAndCanExecuteHandlerController(executedAssertionHandler, canExecuteAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventThaIsExecutedRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToMethod.ExecutedOnlyHandlerController(executedAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }

            [Test]
            public void AddsCommandHandlerOfExecutedEventAndCanExecuteEventThaIsExecutedRoutedEventHandler()
            {
                WpfApplicationRunner.Start<Application>().Run(application =>
                {
                    var context = new object();
                    var testButton = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand };
                    var element = new TestElement { Name = "element", Content = testButton, DataContext = context };

                    var executedAssertionHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                    var canExecuteAssertionHandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();
                    var controller = new TestWpfControllers.AttributedToMethod.ExecutedAndCanExecuteHandlerController(executedAssertionHandler, canExecuteAssertionHandler);
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    var parameter = new object();
                    (testButton.Command as RoutedCommand).CanExecute(parameter, element);
                    (testButton.Command as RoutedCommand).Execute(parameter, element);

                    canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                    executedAssertionHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                }).Shutdown();
            }
        }
    }

    [TestFixture]
    public class AttachingAndDetachingWpfController
    {
        [Test]
        public void InjectsInstanceWithWpfControllerInjector()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var injector = MockRepository.GenerateMock<IWpfControllerInjector>();
                var controller = new object();
                var element = new FrameworkElement();

                WpfController.Injector = injector;
                WpfController.GetControllers(element).Add(controller);

                injector.AssertWasCalled(i => i.Inject(controller));
            }).Shutdown();
        }

        [Test]
        public void SetsChangedDataContextWhenDatContextOfElementIsChanged()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var context = new object();
                var element = new FrameworkElement { DataContext = context };
                var controller = new TestWpfControllers.TestWpfController();

                WpfController.GetControllers(element).Add(controller);

                Assert.That(controller.Context, Is.EqualTo(context));

                var anotherContext = new object();
                element.DataContext = anotherContext;

                Assert.That(controller.Context, Is.EqualTo(anotherContext));
            });
        }

        [Test]
        public void UnregistersRoutedEventHandlerAndSetNullToElementWhenUnloadedEventIsRaised()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new TestElement { Name = "Element" };
                var controller = new TestWpfControllers.TestWpfController();

                WpfController.GetControllers(element).Add(controller);
                element.RaiseInitialized();

                var assertionHandler = MockRepository.GenerateMock<Action>();
                controller.AssertionHandler = assertionHandler;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler.AssertWasCalled(h => h.Invoke());
                Assert.That(controller.Element, Is.EqualTo(element));

                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = element });

                assertionHandler = MockRepository.GenerateMock<Action>();
                controller.AssertionHandler = assertionHandler;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler.AssertWasNotCalled(h => h.Invoke());
                Assert.That(controller.Element, Is.Null);
            });
        }

        [Test]
        public void UnregistersRoutedEventHandlerWhenWpfControllerIsDetached()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var context = new object();
                var element = new TestElement { Name = "Element", DataContext = context };
                var controller = new TestWpfControllers.TestWpfController();

                WpfController.GetControllers(element).Add(controller);

                element.RaiseInitialized();

                Assert.That(controller.Context, Is.EqualTo(context));
                Assert.That(controller.Element, Is.EqualTo(element));

                var assertionHandler = MockRepository.GenerateMock<Action>();
                controller.AssertionHandler = assertionHandler;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler.AssertWasCalled(h => h.Invoke());

                WpfController.GetControllers(element).Remove(controller);

                Assert.That(controller.Context, Is.Null);
                Assert.That(controller.Element, Is.Null);

                assertionHandler = MockRepository.GenerateMock<Action>();
                controller.AssertionHandler = assertionHandler;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler.AssertWasNotCalled(h => h.Invoke());

                element.RaiseInitialized();

                Assert.That(controller.Context, Is.Null);
                Assert.That(controller.Element, Is.Null);

                assertionHandler = MockRepository.GenerateMock<Action>();
                controller.AssertionHandler = assertionHandler;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler.AssertWasNotCalled(h => h.Invoke());
            });
        }

        [Test]
        public void AttachesMultiWpfControllers()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var context = new object();
                var element = new TestElement { Name = "Element", DataContext = context };

                var controller1 = new TestWpfControllers.TestWpfController();
                var controller2 = new TestWpfControllers.TestWpfController();
                var controller3 = new TestWpfControllers.TestWpfController();
                WpfController.GetControllers(element).Add(controller1);
                WpfController.GetControllers(element).Add(controller2);
                WpfController.GetControllers(element).Add(controller3);

                Assert.That(controller1.Context, Is.EqualTo(context));
                Assert.That(controller2.Context, Is.EqualTo(context));
                Assert.That(controller3.Context, Is.EqualTo(context));

                element.RaiseInitialized();

                Assert.That(controller1.Element, Is.EqualTo(element));
                Assert.That(controller2.Element, Is.EqualTo(element));
                Assert.That(controller3.Element, Is.EqualTo(element));

                var assertionHandler1 = MockRepository.GenerateMock<Action>();
                var assertionHandler2 = MockRepository.GenerateMock<Action>();
                var assertionHandler3 = MockRepository.GenerateMock<Action>();
                controller1.AssertionHandler = assertionHandler1;
                controller2.AssertionHandler = assertionHandler2;
                controller3.AssertionHandler = assertionHandler3;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler1.AssertWasCalled(h => h.Invoke());
                assertionHandler2.AssertWasCalled(h => h.Invoke());
                assertionHandler3.AssertWasCalled(h => h.Invoke());

                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = element });

                Assert.That(controller1.Element, Is.Null);
                Assert.That(controller2.Element, Is.Null);
                Assert.That(controller3.Element, Is.Null);

                assertionHandler1 = MockRepository.GenerateMock<Action>();
                assertionHandler2 = MockRepository.GenerateMock<Action>();
                assertionHandler3 = MockRepository.GenerateMock<Action>();
                controller1.AssertionHandler = assertionHandler1;
                controller2.AssertionHandler = assertionHandler2;
                controller3.AssertionHandler = assertionHandler3;
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
                assertionHandler1.AssertWasNotCalled(h => h.Invoke());
                assertionHandler2.AssertWasNotCalled(h => h.Invoke());
                assertionHandler3.AssertWasNotCalled(h => h.Invoke());
            });
        }

        [Test]
        public void RetrievesRoutedEventHandlersAndExecutesThemWhenElementIsNotAttached()
        {
            var controller = new TestWpfControllers.TestWpfController();
            var assertionHandler = MockRepository.GenerateMock<Action>();
            controller.AssertionHandler = assertionHandler;

            WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("Element")
                .Raise("Loaded");

            assertionHandler.AssertWasCalled(h => h.Invoke());
        }

        [Test]
        public async Task RetrievesRoutedEventHandlersAndExecutesThemThatAreAsyncHandlerWhenElementIsNotAttached()
        {
            var controller = new TestWpfControllers.TestWpfControllerAsync();
            var assertionHandler = MockRepository.GenerateMock<Action>();
            controller.AssertionHandler = assertionHandler;

            await WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("Element")
                .RaiseAsync("Loaded");

            assertionHandler.AssertWasCalled(h => h.Invoke());
        }

        [Test]
        public void RetrievesCommandHandlersAndExecutesThemWhenElementIsNotAttached()
        {
            var controller = new TestWpfControllers.TestWpfController();
            var executedAssertionHandler = MockRepository.GenerateMock<Action>();
            var canExecuteAssertionHandler = MockRepository.GenerateMock<Action>();
            controller.ExecutedAssertionHandler = executedAssertionHandler;
            controller.CanExecuteAssertionHandler = canExecuteAssertionHandler;

            var command = TestWpfControllers.TestCommand;
            var parameter = new object();
            WpfController.RetrieveCommandHandlers(controller)
                .GetBy("TestCommand")
                .With(command)
                .RaiseExecuted(parameter);

            executedAssertionHandler.AssertWasCalled(h => h.Invoke());

            WpfController.RetrieveCommandHandlers(controller)
                .GetBy("TestCommand")
                .With(command)
                .RaiseCanExecute(parameter);

            canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke());
        }

        [Test]
        public async Task RetrievesCommandHandlersAndExecutesThemThatAreAsyncHnalderWhenElementIsNotAttached()
        {
            var controller = new TestWpfControllers.TestWpfControllerAsync();
            var executedAssertionHandler = MockRepository.GenerateMock<Action>();
            var canExecuteAssertionHandler = MockRepository.GenerateMock<Action>();
            controller.ExecutedAssertionHandler = executedAssertionHandler;
            controller.CanExecuteAssertionHandler = canExecuteAssertionHandler;

            var command = TestWpfControllers.TestCommand;
            var parameter = new object();
            await WpfController.RetrieveCommandHandlers(controller)
                .GetBy("TestCommand")
                .With(command)
                .RaiseExecutedAsync(parameter);

            executedAssertionHandler.AssertWasCalled(h => h.Invoke());

            await WpfController.RetrieveCommandHandlers(controller)
                .GetBy("TestCommand")
                .With(command)
                .RaiseCanExecuteAsync(parameter);

            canExecuteAssertionHandler.AssertWasCalled(h => h.Invoke());
        }
    }

    [TestFixture]
    public class WpfControllerExtension
    {
        [Test]
        public void AttachesExtensionWhenInitializedEventIsRaised()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var extension = MockRepository.GenerateMock<IWpfControllerExtension>();
                WpfController.Add(extension);

                try
                {
                    var element = new TestElement { Name = "element" };

                    var controller = new TestWpfControllers.TestWpfController();
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();

                    extension.AssertWasCalled(e => e.Attach(element, controller));
                }
                finally
                {
                    WpfController.Remove(extension);
                }
            }).Shutdown();
        }

        [Test]
        public void DetachesExtensionWhenUnloadEventIsRaised()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var extension = MockRepository.GenerateMock<IWpfControllerExtension>();
                WpfController.Add(extension);

                try
                {
                    var element = new TestElement { Name = "element" };

                    var controller = new TestWpfControllers.TestWpfController();
                    WpfController.GetControllers(element).Add(controller);

                    element.RaiseInitialized();
                    element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = element });

                    extension.AssertWasCalled(e => e.Detach(element, controller));
                }
                finally
                {
                    WpfController.Remove(extension);
                }
            }).Shutdown();
        }

        [Test]
        public void RetrievesExtensionWithSpecifiedExtensionType()
        {
            var extension = new TestExtension();
            WpfController.Add(extension);

            try
            {
                var controller = new TestWpfControllers.TestWpfController();

                var retrievedExtension = WpfController.Retrieve<TestExtension, object>(controller);

                Assert.That(retrievedExtension, Is.EqualTo(controller));
            }
            finally
            {
                WpfController.Remove(extension);
            }
        }

        private class TestExtension : IWpfControllerExtension
        {
            void IWpfControllerExtension.Attach(FrameworkElement element, object controller)
            {
            }

            void IWpfControllerExtension.Detach(FrameworkElement element, object controller)
            {
            }

            object IWpfControllerExtension.Retrieve(object controller)
            {
                return controller;
            }
        }
    }

    [TestFixture]
    public class WpfControllerCreation
    {
        [Test]
        public void CreatesControllerTheTypeOfWhichIsTheValueOfControllerTypeProperty()
        {
            var controllerType = typeof(TestWpfControllers.TestWpfController);

            var wpfController = new WpfController { ControllerType = controllerType };
            var controller = wpfController.Create();

            Assert.That(controller, Is.TypeOf(controllerType));
        }

        [Test]
        public void CreatesControllerWithSpecifiedIWpfControllerFactory()
        {
            var controllerType = typeof(TestWpfControllers.TestWpfController);
            var expectedController = new TestWpfControllers.TestWpfController();

            WpfController.Factory = MockRepository.GenerateMock<IWpfControllerFactory>();
            WpfController.Factory.Expect(f => f.Create(controllerType)).Return(expectedController);

            var wpfController = new WpfController { ControllerType = controllerType };
            var controller = wpfController.Create();

            Assert.That(controller, Is.EqualTo(expectedController));
        }

        [Test]
        public void GetsNullWhenValueOfControllerTypePropertyIsNull()
        {
            var wpfController = new WpfController { ControllerType = null };
            var controller = wpfController.Create();

            Assert.That(controller, Is.Null);
        }

        [Test]
        public void ThrowsExceptionWhenIWpfControllerThatIsNullIsSpecified()
        {
            Assert.Throws<ArgumentNullException>(() => WpfController.Factory = null);
        }
    }
}
