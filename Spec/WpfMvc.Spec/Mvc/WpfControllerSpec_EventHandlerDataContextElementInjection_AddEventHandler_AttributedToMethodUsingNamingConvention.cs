﻿// Copyright (C) 2019-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("The event handlers are attributed to methods using a naming convention")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToMethodUsingNamingConvention : FixtureSteppable
    {
        object DataContext { get; } = new object();
        FrameworkElement ChildElement { get; } = new FrameworkElement { Name = "ChildElement" };
        TestElement Element { get; }

        bool EventHandled { get; set; }

        [Background("a data context, a child element, and an element that has the child element")]
        public WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToMethodUsingNamingConvention()
        {
            Element = new TestElement { Name = "element", Content = ChildElement, DataContext = DataContext };
        }

        [Example("When an event handler has no argument")]
        void Ex01()
        {
            TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentHandlerController controller = null;

            Given("a controller that has event handlers", () =>
            {
                void HandleEvent() => EventHandled = true;
                controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentHandlerController(HandleEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));

            Then("the data context of the controller should be set", () => controller.DataContext == DataContext);
            Then("the element of the controller should be set", () => controller.Element == Element);
            Then("the child element of the controller should be set", () => controller.ChildElement == ChildElement);

            When("the Initialized event is raised", () => Element.RaiseInitialized());

            Then("the data context of the controller should be set", () => controller.DataContext == DataContext);
            Then("the element of the controller should be set", () => controller.Element == Element);
            Then("the child element of the controller should be set", () => controller.ChildElement == ChildElement);

            When("the Loaded event of the child element is raised", () => ChildElement.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = ChildElement }));

            Then("the Loaded event should be handled", () => EventHandled);
        }

        [Example("When an event handler has one argument")]
        void Ex02()
        {
            TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentHandlerController controller = null;

            Given("a controller that has event handlers", () =>
            {
                void HandleEvent(RoutedEventArgs e) => EventHandled = Equals(e.Source, ChildElement);
                controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentHandlerController(HandleEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));

            Then("the data context of the controller should be set", () => controller.DataContext == DataContext);
            Then("the element of the controller should be set", () => controller.Element == Element);
            Then("the child element of the controller should be set", () => controller.ChildElement == ChildElement);

            When("the Initialized event is raised", () => Element.RaiseInitialized());

            Then("the data context of the controller should be set", () => controller.DataContext == DataContext);
            Then("the element of the controller should be set", () => controller.Element == Element);
            Then("the child element of the controller should be set", () => controller.ChildElement == ChildElement);

            When("the Loaded event of the child element is raised", () => ChildElement.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = ChildElement }));

            Then("the Loaded event should be handled", () => EventHandled);
        }

        [Example("When an event handler is the RoutedEventHandler")]
        void Ex03()
        {
            TestWpfControllers.AttributedToMethodUsingNamingConvention.RoutedEventHandlerController controller = null;

            Given("a controller that has event handlers", () =>
            {
                void HandleEvent(object s, RoutedEventArgs e) => EventHandled = Equals(s, ChildElement) && Equals(e.Source, ChildElement);
                controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.RoutedEventHandlerController(HandleEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));

            Then("the data context of the controller should be set", () => controller.DataContext == DataContext);
            Then("the element of the controller should be set", () => controller.Element == Element);
            Then("the child element of the controller should be set", () => controller.ChildElement == ChildElement);

            When("the Initialized event is raised", () => Element.RaiseInitialized());

            Then("the data context of the controller should be set", () => controller.DataContext == DataContext);
            Then("the element of the controller should be set", () => controller.Element == Element);
            Then("the child element of the controller should be set", () => controller.ChildElement == ChildElement);

            When("the Loaded event of the child element is raised", () => ChildElement.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = ChildElement }));

            Then("the Loaded event should be handled", () => EventHandled);
        }
    }
}
