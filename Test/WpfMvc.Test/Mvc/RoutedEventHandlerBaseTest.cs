// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Runners;

namespace Fievus.Windows.Mvc
{
    [TestFixture]
    public class RoutedEventHandlerBaseTest
    {
        [Test]
        public void ExecutesRoutedEventHandlerWithSpecifiedElementNameAndRoutedEventName()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var handler = MockRepository.GenerateMock<RoutedEventHandler>();
                var notCalledHandler = MockRepository.GenerateMock<RoutedEventHandler>();

                var routedEventHandlerBase = new RoutedEventHandlerBase();
                routedEventHandlerBase.Add("TestElement", element, "Loaded", FrameworkElement.LoadedEvent, handler, true);
                routedEventHandlerBase.Add("TestElement", element, "Unloaded", FrameworkElement.UnloadedEvent, notCalledHandler, true);
                routedEventHandlerBase.Add("AnotherElement", new FrameworkElement(), "Loaded", FrameworkElement.LoadedEvent, notCalledHandler, true);

                var sender = new object();
                var args = new RoutedEventArgs();
                routedEventHandlerBase.GetBy("TestElement")
                    .From(sender)
                    .With(args)
                    .Raise("Loaded");

                handler.AssertWasCalled(h => h.Invoke(sender, args));
                notCalledHandler.AssertWasNotCalled(h => h.Invoke(sender, args));
            }).Shutdown();
        }

        [Test]
        public void ExecutesRoutedEventHandlerWhenSpecifiedElementAndRoutedEventIsNull()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var handler = MockRepository.GenerateMock<RoutedEventHandler>();

                var routedEventHandlerBase = new RoutedEventHandlerBase();
                routedEventHandlerBase.Add("TestElement", null, "Loaded", null, handler, true);

                var sender = new object();
                var args = new RoutedEventArgs();
                routedEventHandlerBase.GetBy("TestElement")
                    .From(sender)
                    .With(args)
                    .Raise("Loaded");

                handler.AssertWasCalled(h => h.Invoke(sender, args));
            }).Shutdown();
        }

        [Test]
        public void ExecutesRoutedEventHandlerThatDoesNotHaveArgumentWithSpecifiedElementNameAndRoutedEventName()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var handler = MockRepository.GenerateMock<Action>();

                var routedEventHandlerBase = new RoutedEventHandlerBase();
                routedEventHandlerBase.Add("TestElement", null, "Loaded", null, handler, true);

                var sender = new object();
                var args = new RoutedEventArgs();
                routedEventHandlerBase.GetBy("TestElement")
                    .From(sender)
                    .With(args)
                    .Raise("Loaded");

                handler.AssertWasCalled(h => h.Invoke());
            }).Shutdown();
        }

        [Test]
        public void ExecutesRoutedEventHandlerThatHaveOneArgumentWithSpecifiedElementNameAndRoutedEventName()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var handler = MockRepository.GenerateMock<Action<RoutedEventArgs>>();

                var routedEventHandlerBase = new RoutedEventHandlerBase();
                routedEventHandlerBase.Add("TestElement", null, "Loaded", null, handler, true);

                var sender = new object();
                var args = new RoutedEventArgs();
                routedEventHandlerBase.GetBy("TestElement")
                    .From(sender)
                    .With(args)
                    .Raise("Loaded");

                handler.AssertWasCalled(h => h.Invoke(args));
            }).Shutdown();
        }

        [Test]
        public void RegistersRoutedEventHandlerToSpecifiedElement()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var handler = MockRepository.GenerateMock<RoutedEventHandler>();

                var routedEventHandlerBase = new RoutedEventHandlerBase();
                routedEventHandlerBase.Add("TestElement", element, "Loaded", FrameworkElement.LoadedEvent, handler, true);
                routedEventHandlerBase.RegisterRoutedEventHandler();

                var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element };
                element.RaiseEvent(args);

                handler.AssertWasCalled(h => h.Invoke(element, args));
            }).Shutdown();
        }

        [Test]
        public void UnregistersRoutedEventHandlerFromSpecifiedElement()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var handler = MockRepository.GenerateMock<RoutedEventHandler>();

                var routedEventHandlerBase = new RoutedEventHandlerBase();
                routedEventHandlerBase.Add("TestElement", element, "Loaded", FrameworkElement.LoadedEvent, handler, true);
                routedEventHandlerBase.RegisterRoutedEventHandler();
                routedEventHandlerBase.UnregisterRoutedEventHandler();

                var args = new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element };
                element.RaiseEvent(args);

                handler.AssertWasNotCalled(h => h.Invoke(element, args));
            }).Shutdown();
        }
    }
}
