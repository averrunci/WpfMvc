// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;

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
    }
}
