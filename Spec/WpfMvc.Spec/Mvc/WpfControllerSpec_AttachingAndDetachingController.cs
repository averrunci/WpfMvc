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
    [Context("Attaching and detaching a controller")]
    class WpfControllerSpec_AttachingAndDetachingController : FixtureSteppable, IDisposable
    {
        [Context]
        WpfControllerSpec_AttachingAndDetachingController_IsEnabled IsEnabled { get; }

        [Context]
        WpfControllerSpec_AttachingAndDetachingController_IsEnabledBeforeDataContextIsSet IsEnabledBeforeDataContextIsSet { get; }

        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string ElementKey = "Element";
        const string ControllerKey = "Controller";
        const string LoadedEventHandledKey = "LoadedEventHandled";
        const string ChangedEventHandledKey = "ChangedEventHandled";
        const string ControllersKey = "Controllers";
        const string LoadedEventsHandledKey = "LoadedEventsHandled";
        const string ChangedEventsHandledKey = "ChangedEventsHandled";

        public WpfControllerSpec_AttachingAndDetachingController()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("Attaches a controller when the Key property of the WpfController is set")]
        void Ex01()
        {
            Given("an element that contains the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { DataContext = new TestDataContexts.KeyAttachingTestDataContext() })));
            When("the key of the element is set using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetKey(context.Get<TestElement>(ElementKey), "TestElement")));
            Then("the WpfController should be enabled for the element", () => WpfRunner.Run((application, context) => WpfController.GetIsEnabled(context.Get<TestElement>(ElementKey)).Should().BeTrue()));
            Then("the controller should be attached to the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.GetControllers(element).Select(controller => controller.GetType()).Should().BeEquivalentTo(typeof(TestWpfControllers.KeyTestDataContextController));
                WpfController.GetControllers(element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == element.DataContext).Should().BeTrue();
            }));
        }

        [Example("Sets the changed data context when the WpfController is enabled and the data context of an element is changed")]
        void Ex02()
        {
            Given("an element that contains the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { DataContext = new TestDataContexts.AttachingTestDataContext() })));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).GetController<TestWpfControllers.TestController>().DataContext.Should().Be(context.Get<TestElement>(ElementKey).DataContext)));
            When("another data context is set for the element", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).DataContext = new object()));
            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).GetController<TestWpfControllers.TestController>().DataContext.Should().Be(context.Get<TestElement>(ElementKey).DataContext)));
        }

        [Example("Removes event handlers and sets null to elements and a data context when the Unload event of the root element is raised")]
        void Ex03()
        {
            Given("an element that contains the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "Element", DataContext = new TestDataContexts.TestDataContext() })));

            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.SetIsEnabled(element, true);
                context.Set(ControllerKey, element.GetController<TestWpfControllers.TestWpfController>());
            }));
            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).DataContext.Should().Be(context.Get<TestElement>(ElementKey).DataContext)));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Get<TestWpfControllers.TestWpfController>(ControllerKey).LoadedAssertionHandler = () => context.Set(LoadedEventHandledKey, true);
                var element = context.Get<TestElement>(ElementKey);
                context.Get<TestElement>(ElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
            }));
            Then("the Loaded event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(LoadedEventHandledKey).Should().BeTrue()));

            When("the Changed event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Get<TestWpfControllers.TestWpfController>(ControllerKey).ChangedAssertionHandler = () => context.Set(ChangedEventHandledKey, true);
                context.Get<TestElement>(ElementKey).RaiseChanged();
            }));
            Then("the Changed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ChangedEventHandledKey).Should().BeTrue()));

            When("the Unloaded event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = context.Get<TestElement>(ElementKey) })));
            Then("the data context of the controller should be null", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).DataContext.Should().BeNull()));
            Then("the element the controller should be null", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).Element.Should().BeNull()));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Set(LoadedEventHandledKey, false);
                var element = context.Get<TestElement>(ElementKey);
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
            }));
            Then("the Loaded event should not be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(LoadedEventHandledKey).Should().BeFalse()));

            When("the Changed event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Set(ChangedEventHandledKey, false);
                context.Get<TestElement>(ElementKey).RaiseChanged();
            }));
            Then("the Changed event should not be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ChangedEventHandledKey).Should().BeFalse()));
        }

        [Example("Removes event handlers and sets null to elements and a data context when the WpfController is disabled")]
        void Ex04()
        {
            Given("an element that contains the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "Element", DataContext = new TestDataContexts.TestDataContext() })));

            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.SetIsEnabled(element, true);
                context.Set(ControllerKey, element.GetController<TestWpfControllers.TestWpfController>());
            }));
            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).DataContext.Should().Be(context.Get<TestElement>(ElementKey).DataContext)));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).Element.Should().Be(context.Get<TestElement>(ElementKey))));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Get<TestWpfControllers.TestWpfController>(ControllerKey).LoadedAssertionHandler = () => context.Set(LoadedEventHandledKey, true);
                var element = context.Get<TestElement>(ElementKey);
                context.Get<TestElement>(ElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
            }));
            Then("the Loaded event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(LoadedEventHandledKey).Should().BeTrue()));

            When("the Changed event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Get<TestWpfControllers.TestWpfController>(ControllerKey).ChangedAssertionHandler = () => context.Set(ChangedEventHandledKey, true);
                context.Get<TestElement>(ElementKey).RaiseChanged();
            }));
            Then("the Changed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ChangedEventHandledKey).Should().BeTrue()));

            When("the WpfController is disabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), false)));
            Then("the controller should be detached", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Should().BeEmpty()));
            Then("the data context of the controller should be null", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).DataContext.Should().BeNull()));
            Then("the element the controller should be null", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfController>(ControllerKey).Element.Should().BeNull()));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Set(LoadedEventHandledKey, false);
                var element = context.Get<TestElement>(ElementKey);
                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
            }));
            Then("the Loaded event should not be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(LoadedEventHandledKey).Should().BeFalse()));

            When("the Changed event is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Set(ChangedEventHandledKey, false);
                context.Get<TestElement>(ElementKey).RaiseChanged();
            }));
            Then("the Changed event should not be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ChangedEventHandledKey).Should().BeFalse()));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));
            Then("the controller should not be attached", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Should().BeEmpty()));
        }

        [Example("Attaches multi controllers")]
        void Ex05()
        {
            Given("an element that contains the data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "Element", DataContext = new TestDataContexts.MultiTestDataContext() })));

            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.SetIsEnabled(element, true);
                context.Set(ControllersKey, new TestWpfControllers.TestWpfControllerBase[] { element.GetController<TestWpfControllers.MultiTestWpfControllerA>(), element.GetController<TestWpfControllers.MultiTestWpfControllerB>(), element.GetController<TestWpfControllers.MultiTestWpfControllerC>() });
                context.Set(LoadedEventsHandledKey, new[] { false, false, false });
                context.Set(ChangedEventsHandledKey, new[] { false, false, false });
            }));
            Then("the data context of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfControllerBase[]>(ControllersKey).All(controller => Equals(controller.DataContext, context.Get<TestElement>(ElementKey).DataContext)).Should().BeTrue()));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));
            Then("the element of the controller should be set", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfControllerBase[]>(ControllersKey).All(controller => Equals(controller.Element, context.Get<TestElement>(ElementKey))).Should().BeTrue()));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
            {
                var controllers = context.Get<TestWpfControllers.TestWpfControllerBase[]>(ControllersKey);
                var loadedEventsHandled = context.Get<bool[]>(LoadedEventsHandledKey);
                for (var index = 0; index < controllers.Length; ++index)
                {
                    var eventHandledIndex = index;
                    controllers[index].LoadedAssertionHandler = () => loadedEventsHandled[eventHandledIndex] = true;
                }

                var element = context.Get<TestElement>(ElementKey);
                context.Get<TestElement>(ElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
            }));
            Then("the Loaded event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool[]>(LoadedEventsHandledKey).All(handled => handled).Should().BeTrue()));

            When("the Changed event is raised", () => WpfRunner.Run((application, context) =>
            {
                var controllers = context.Get<TestWpfControllers.TestWpfControllerBase[]>(ControllersKey);
                var changedEventsHandled = context.Get<bool[]>(ChangedEventsHandledKey);
                for (var index = 0; index < controllers.Length; ++index)
                {
                    var eventHandledIndex = index;
                    controllers[index].ChangedAssertionHandler = () => changedEventsHandled[eventHandledIndex] = true;
                }

                context.Get<TestElement>(ElementKey).RaiseChanged();
            }));
            Then("the Changed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool[]>(ChangedEventsHandledKey).All(handled => handled).Should().BeTrue()));

            When("the Unloaded event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = context.Get<TestElement>(ElementKey) })));
            Then("the data context of the controller should be null", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfControllerBase[]>(ControllersKey).All(controller => controller.DataContext == null).Should().BeTrue()));
            Then("the element the controller should be null", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.TestWpfControllerBase[]>(ControllersKey).All(controller => controller.Element == null).Should().BeTrue()));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
            {
                var loadedEventsHandled = context.Get<bool[]>(LoadedEventsHandledKey);
                for (var index = 0; index < loadedEventsHandled.Length; ++index)
                {
                    loadedEventsHandled[index] = false;
                }
                var element = context.Get<TestElement>(ElementKey);

                element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = element });
            }));
            Then("the Loaded event should not be handled", () => WpfRunner.Run((application, context) => context.Get<bool[]>(LoadedEventsHandledKey).All(handled => handled).Should().BeFalse()));

            When("the Changed event is raised", () => WpfRunner.Run((application, context) =>
            {
                var changedEventsHandled = context.Get<bool[]>(ChangedEventsHandledKey);
                for (var index = 0; index < changedEventsHandled.Length; ++index)
                {
                    changedEventsHandled[index] = false;
                }

                context.Get<TestElement>(ElementKey).RaiseChanged();
            }));
            Then("the Loaded event should not be handled", () => WpfRunner.Run((application, context) => context.Get<bool[]>(ChangedEventsHandledKey).All(handled => handled).Should().BeFalse()));
        }
    }
}
