// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Sets elements")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_SetElement : FixtureSteppable
    {
        FrameworkElement ChildElement { get; set; }
        TestElement Element { get; set; }

        [Example("When the elements are attributed to fields")]
        void Ex01()
        {
            TestWpfControllers.AttributedToField.NoArgumentHandlerController controller = null;

            Given("a child element", () => ChildElement = new FrameworkElement { Name = "childElement" });
            Given("an element that has the child element", () => Element = new TestElement { Name = "element", Content = ChildElement });
            Given("a controller", () => controller = new TestWpfControllers.AttributedToField.NoArgumentHandlerController(null));
            When("the child element is set to the controller using the WpfController", () => WpfController.SetElement(ChildElement, controller, true));
            When("the element is set to the controller using the WpfController", () => WpfController.SetElement(Element, controller, true));
            Then("the element should be set to the controller", () => controller.Element == Element);
            Then("the child element should be set to the controller", () => controller.ChildElement == ChildElement);
        }

        [Example("When the elements are attributed to properties")]
        void Ex02()
        {
            TestWpfControllers.AttributedToProperty.NoArgumentHandlerController controller = null;

            Given("a child element", () => ChildElement = new FrameworkElement { Name = "ChildElement" });
            Given("an element that has the child element", () => Element = new TestElement { Name = "element", Content = ChildElement });
            Given("a controller", () => controller = new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(null));
            When("the child element is set to the controller using the WpfController", () => WpfController.SetElement(ChildElement, controller, true));
            When("the element is set to the controller using the WpfController", () => WpfController.SetElement(Element, controller, true));
            Then("the element should be set to the controller", () => controller.Element == Element);
            Then("the child element should be set to the controller", () => controller.ChildElement == ChildElement);
        }

        [Example("When the elements are attributed to methods")]
        void Ex03()
        {
            TestWpfControllers.AttributedToMethod.NoArgumentHandlerController controller = null;

            Given("a child element", () => ChildElement = new FrameworkElement { Name = "ChildElement" });
            Given("an element that has the child element", () => Element = new TestElement { Name = "element", Content = ChildElement });
            Given("a controller", () => controller = new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(null));
            When("the child element is set to the controller using the WpfController", () => WpfController.SetElement(ChildElement, controller, true));
            When("the element is set to the controller using the WpfController", () => WpfController.SetElement(Element, controller, true));
            Then("the element should be set to the controller", () => controller.Element == Element);
            Then("the child element should be set to the controller", () => controller.ChildElement == ChildElement);
        }
    }
}
