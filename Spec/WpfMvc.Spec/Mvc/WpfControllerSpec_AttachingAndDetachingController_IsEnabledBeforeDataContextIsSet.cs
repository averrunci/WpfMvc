// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Linq;
using System.Windows;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("Attaches a controller when the IsEnabled property of the WpfController is set to true before the data context of the element is set")]
    class WpfControllerSpec_AttachingAndDetachingController_IsEnabledBeforeDataContextIsSet : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string ElementKey = "Element";

        public WpfControllerSpec_AttachingAndDetachingController_IsEnabledBeforeDataContextIsSet()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("When the key is the name of the data context type")]
        void Ex01()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.AttachingTestDataContext()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.TestDataContextController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the name of the data context base type")]
        void Ex02()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.DerivedBaseAttachingTestDataContext()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.BaseTestDataContextController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the full name of the data context type")]
        void Ex03()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.AttachingTestDataContextFullName()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.TestDataContextFullNameController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the full name of the data context base type")]
        void Ex04()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.DerivedBaseAttachingTestDataContextFullName()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.BaseTestDataContextFullNameController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the name of the data context type that is generic")]
        void Ex05()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.GenericAttachingTestDataContext<string>()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.GenericTestDataContextController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the full name of the data context type that is generic")]
        void Ex06()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.GenericAttachingTestDataContextFullName<string>()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.GenericTestDataContextFullNameController), typeof(TestWpfControllers.GenericTestDataContextFullNameWithoutParametersController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the name of the interface implemented by the data context")]
        void Ex07()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.InterfaceImplementedAttachingTestDataContext()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.InterfaceImplementedTestDataContextController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("When the key is the full name of the interface implemented by the data context")]
        void Ex08()
        {
            Given("an element that does not contain the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("a data context of the element is set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new TestDataContexts.InterfaceImplementedAttachingTestDataContextFullName()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.InterfaceImplementedTestDataContextFullNameController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }
    }
}
