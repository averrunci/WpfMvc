// Copyright (C) 2018-2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections;
using System.Windows;
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Event handler, data context, and element injection")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection : FixtureSteppable
    {
        object DataContext { get; set; }
        FrameworkElement ChildElement { get; set; }
        TestElement Element { get; set; }

        static bool EventHandled { get; set; }
        static Action NoArgumentAssertionHandler { get; } = () => EventHandled = true;
        static Action<object> OneArgumentAssertionHandler { get; } = e => EventHandled = true;
        static RoutedEventHandler RoutedEventAssertionHandler { get; } = (s, e) => EventHandled = true;

        [Example("Adds event handlers")]
        [Sample(Source = typeof(WpfControllerSampleDataSource))]
        void Ex01(TestWpfControllers.ITestWpfController controller, string childElementName)
        {
            EventHandled = false;
            Given("a data context", () => DataContext = new object());
            Given("a child element", () => ChildElement = new FrameworkElement { Name = childElementName });
            Given("an element hta has the child element", () => Element = new TestElement { Name = "element", Content = ChildElement, DataContext = DataContext });

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

        [Example("Sets elements")]
        [Sample(Source = typeof(WpfControllerSampleDataSource))]
        void Ex02(TestWpfControllers.ITestWpfController controller, string childElementName)
        {
            Given("a child element", () => ChildElement = new FrameworkElement { Name = childElementName });
            Given("an element that has the child element", () => Element = new TestElement { Name = "element", Content = ChildElement });
            When("the child element is set to the controller using the WpfController", () => WpfController.SetElement(ChildElement, controller, true));
            When("the element is set to the controller using the WpfController", () => WpfController.SetElement(Element, controller, true));
            Then("the element should be set to the controller", () => controller.Element == Element);
            Then("the child element should be set to the controller", () => controller.ChildElement == ChildElement);
        }

        [Example("Sets a data context")]
        [Sample(Source = typeof(WpfControllerSampleDataSource))]
        void Ex03(TestWpfControllers.ITestWpfController controller)
        {
            Given("a data context", () => DataContext = new object());
            Given("a controller", () => controller = new TestWpfControllers.AttributedToField.NoArgumentHandlerController(null));
            When("the data context is set to the controller using the WpfController", () => WpfController.SetDataContext(DataContext, controller));
            Then("the data context should be set to the controller", () => controller.DataContext == DataContext);
        }

        class WpfControllerSampleDataSource : ISampleDataSource
        {
            IEnumerable ISampleDataSource.GetData()
            {
                yield return new { Description = "When the contents are attributed to fields and the event handler has no argument", Controller = new TestWpfControllers.AttributedToField.NoArgumentHandlerController(NoArgumentAssertionHandler), ChildElementName = "childElement" };
                yield return new { Description = "When the contents are attributed to fields and the event handler has one argument", Controller = new TestWpfControllers.AttributedToField.OneArgumentHandlerController(OneArgumentAssertionHandler), ChildElementName = "childElement" };
                yield return new { Description = "When the contents are attributed to fields and the event handler has the RoutedEventHandler", Controller = new TestWpfControllers.AttributedToField.RoutedEventHandlerController(RoutedEventAssertionHandler), ChildElementName = "childElement" };
                yield return new { Description = "When the contents are attributed to properties and the event handler has no argument", Controller = new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(NoArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to properties and the event handler has one argument", Controller = new TestWpfControllers.AttributedToProperty.OneArgumentHandlerController(OneArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to properties and the event handler has the RoutedEventHandler", Controller = new TestWpfControllers.AttributedToProperty.RoutedEventHandlerController(RoutedEventAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to methods and the event handler has no argument", Controller = new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(NoArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to methods and the event handler has one argument", Controller = new TestWpfControllers.AttributedToMethod.OneArgumentHandlerController(OneArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to methods and the event handler has the RoutedEventHandler", Controller = new TestWpfControllers.AttributedToMethod.RoutedEventHandlerController(RoutedEventAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to methods and the event handler has no argument using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentHandlerController(NoArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to methods and the event handler has one argument using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentHandlerController(OneArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to methods and the event handler has the RoutedEventHandler using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.RoutedEventHandlerController(RoutedEventAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to async methods and the event handler has no argument using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.NoArgumentHandlerController(NoArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to async methods and the event handler has one argument using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.OneArgumentHandlerController(OneArgumentAssertionHandler), ChildElementName = "ChildElement" };
                yield return new { Description = "When the contents are attributed to async methods and the event handler has the RoutedEventHandler using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.RoutedEventHandlerController(RoutedEventAssertionHandler), ChildElementName = "ChildElement" };
            }
        }
    }
}
