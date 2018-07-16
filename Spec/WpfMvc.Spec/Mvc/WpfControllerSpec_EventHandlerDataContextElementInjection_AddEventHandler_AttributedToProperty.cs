// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("The event handlers are attributed to properties")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToProperty : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string DataContextKey = "DataContext";
        const string ChildElementKey = "ChildElement";
        const string ElementKey = "Element";
        const string ControllerKey = "Controller";
        const string EventHandledKey = "EventHandled";

        public WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToProperty()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("When an event handler has no argument")]
        void Ex01()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a child element", () => WpfRunner.Run((application, context) => context.Set(ChildElementKey, new FrameworkElement { Name = "ChildElement" })));
            Given("an element that has the child element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<FrameworkElement>(ChildElementKey), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has event handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleEvent() => context.Set(EventHandledKey, true);
                context.Set(ControllerKey, new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(HandleEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));

            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));

            When("the Loaded event of the child element is raised", () => WpfRunner.Run((application, context) => context.Get<FrameworkElement>(ChildElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = context.Get<FrameworkElement>(ChildElementKey) })));

            Then("the Loaded event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(EventHandledKey)));
        }

        [Example("When an event handler has one argument")]
        void Ex02()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a child element", () => WpfRunner.Run((application, context) => context.Set(ChildElementKey, new FrameworkElement { Name = "ChildElement" })));
            Given("an element that has the child element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<FrameworkElement>(ChildElementKey), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has event handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleEvent(RoutedEventArgs e) => context.Set(EventHandledKey, Equals(e.Source, context.Get<FrameworkElement>(ChildElementKey)));
                context.Set(ControllerKey, new TestWpfControllers.AttributedToProperty.OneArgumentHandlerController(HandleEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));

            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.OneArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));

            When("the Loaded event of the child element is raised", () => WpfRunner.Run((application, context) => context.Get<FrameworkElement>(ChildElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = context.Get<FrameworkElement>(ChildElementKey) })));

            Then("the Loaded event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(EventHandledKey)));
        }

        [Example("When an event handler is the RoutedEventHandler")]
        void Ex03()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a child element", () => WpfRunner.Run((application, context) => context.Set(ChildElementKey, new FrameworkElement { Name = "ChildElement" })));
            Given("an element that has the child element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<FrameworkElement>(ChildElementKey), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has event handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleEvent(object s, RoutedEventArgs e) => context.Set(EventHandledKey, Equals(s, context.Get<FrameworkElement>(ChildElementKey)) && Equals(e.Source, context.Get<FrameworkElement>(ChildElementKey)));
                context.Set(ControllerKey, new TestWpfControllers.AttributedToProperty.RoutedEventHandlerController(HandleEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));

            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.RoutedEventHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));

            When("the Loaded event of the child element is raised", () => WpfRunner.Run((application, context) => context.Get<FrameworkElement>(ChildElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = context.Get<FrameworkElement>(ChildElementKey) })));

            Then("the Loaded event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(EventHandledKey)));
        }
    }
}
