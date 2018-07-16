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
    [Context("Sets elements")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_SetElement : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string ChildElementKey = "ChildElement";
        const string ElementKey = "Element";
        const string ControllerKey = "Controller";

        public WpfControllerSpec_EventHandlerDataContextElementInjection_SetElement()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("When the elements are attributed to fields")]
        void Ex01()
        {
            Given("a child element", () => WpfRunner.Run((application, context) => context.Set(ChildElementKey, new FrameworkElement { Name = "childElement" })));
            Given("an element that has the child element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<FrameworkElement>(ChildElementKey) })));
            Given("a controller", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.AttributedToField.NoArgumentHandlerController(null))));
            When("the child element is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetElement(context.Get<FrameworkElement>(ChildElementKey), context.Get<TestWpfControllers.AttributedToField.NoArgumentHandlerController>(ControllerKey), true)));
            When("the element is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetElement(context.Get<TestElement>(ElementKey), context.Get<TestWpfControllers.AttributedToField.NoArgumentHandlerController>(ControllerKey), true)));
            Then("the element should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToField.NoArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToField.NoArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));
        }

        [Example("When the elements are attributed to properties")]
        void Ex02()
        {
            Given("a child element", () => WpfRunner.Run((application, context) => context.Set(ChildElementKey, new FrameworkElement { Name = "ChildElement" })));
            Given("an element that has the child element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<FrameworkElement>(ChildElementKey) })));
            Given("a controller", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(null))));
            When("the child element is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetElement(context.Get<FrameworkElement>(ChildElementKey), context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey), true)));
            When("the element is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetElement(context.Get<TestElement>(ElementKey), context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey), true)));
            Then("the element should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));
        }

        [Example("When the elements are attributed to methods")]
        void Ex03()
        {
            Given("a child element", () => WpfRunner.Run((application, context) => context.Set(ChildElementKey, new FrameworkElement { Name = "ChildElement" })));
            Given("an element that has the child element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<FrameworkElement>(ChildElementKey) })));
            Given("a controller", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(null))));
            When("the child element is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetElement(context.Get<FrameworkElement>(ChildElementKey), context.Get<TestWpfControllers.AttributedToMethod.NoArgumentHandlerController>(ControllerKey), true)));
            When("the element is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetElement(context.Get<TestElement>(ElementKey), context.Get<TestWpfControllers.AttributedToMethod.NoArgumentHandlerController>(ControllerKey), true)));
            Then("the element should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToMethod.NoArgumentHandlerController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));
            Then("the child element should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToMethod.NoArgumentHandlerController>(ControllerKey).ChildElement.Should().Be(context.Get<FrameworkElement>(ChildElementKey))));
        }
    }
}
