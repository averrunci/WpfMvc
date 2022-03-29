// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using Carna;

namespace Charites.Windows.Mvc;

[Context("Attaching and detaching a controller")]
class WpfControllerSpec_AttachingAndDetachingController : FixtureSteppable
{
    [Context]
    WpfControllerSpec_AttachingAndDetachingController_IsEnabled IsEnabled => default!;

    [Context]
    WpfControllerSpec_AttachingAndDetachingController_IsEnabledBeforeDataContextIsSet IsEnabledBeforeDataContextIsSet => default!;

    TestElement Element { get; set; } = default!;
    TestWpfControllers.TestWpfController Controller { get; set; } = default!;

    bool LoadedEventHandled { get; set; }
    bool ChangedEventHandled { get; set; }
    private TestWpfControllers.TestWpfControllerBase[] Controllers { get; set; } = default!;
    bool[] LoadedEventsHandled { get; set; } = default!;
    bool[] ChangedEventsHandled { get; set; } = default!;

    [Example("Attaches a controller when the Key property of the WpfController is set")]
    void Ex01()
    {
        Given("an element that contains the data context", () => Element = new TestElement { DataContext = new TestDataContexts.KeyAttachingTestDataContext() });
        When("the key of the element is set using the WpfController", () => WpfController.SetKey(Element, "TestElement"));
        Then("the WpfController should be enabled for the element", () => WpfController.GetIsEnabled(Element));
        Then("the controller should be attached to the element", () =>
            WpfController.GetControllers(Element).Select(controller => controller.GetType()).SequenceEqual(new[] { typeof(TestWpfControllers.KeyTestDataContextController) }) &&
            WpfController.GetControllers(Element).OfType<TestWpfControllers.TestController>().All(controller => controller.DataContext == Element.DataContext)
        );
    }

    [Example("Sets the changed data context when the WpfController is enabled and the data context of an element is changed")]
    void Ex02()
    {
        Given("an element that contains the data context", () => Element = new TestElement { DataContext = new TestDataContexts.AttachingTestDataContext() });
        When("the WpfController is enabled for the element", () => WpfController.SetIsEnabled(Element, true));
        Then("the data context of the controller should be set", () => Element.GetController<TestWpfControllers.TestController>().DataContext == Element.DataContext);
        When("another data context is set for the element", () => Element.DataContext = new object());
        Then("the data context of the controller should be set", () => Element.GetController<TestWpfControllers.TestController>().DataContext == Element.DataContext);
    }

    [Example("Removes event handlers and sets null to elements and a data context when the Unload event of the root element is raised")]
    void Ex03()
    {
        Given("an element that contains the data context", () => Element = new TestElement { Name = "Element", DataContext = new TestDataContexts.TestDataContext() });

        When("the WpfController is enabled for the element", () =>
        {
            WpfController.SetIsEnabled(Element, true);
            Controller = Element.GetController<TestWpfControllers.TestWpfController>();
        });
        Then("the data context of the controller should be set", () => Controller.DataContext == Element.DataContext);

        When("the Initialized event is raised", () => Element.RaiseInitialized());
        Then("the element of the controller should be set", () => Controller.Element == Element);

        When("the Loaded event is raised", () =>
        {
            Controller.LoadedAssertionHandler = () => LoadedEventHandled = true;
            Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = Element });
        });
        Then("the Loaded event should be handled", () => LoadedEventHandled);

        When("the Changed event is raised", () =>
        {
            Controller.ChangedAssertionHandler = () => ChangedEventHandled = true;
            Element.RaiseChanged();
        });
        Then("the Changed event should be handled", () => ChangedEventHandled);

        When("the Unloaded event is raised", () => Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = Element }));
        Then("the data context of the controller should be null", () => Controller.DataContext == null);
        Then("the element of the controller should be null", () => Controller.Element == null);

        When("the Loaded event is raised", () =>
        {
            LoadedEventHandled = false;
            Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = Element });
        });
        Then("the Loaded event should not be handled", () => !LoadedEventHandled);

        When("the Changed event is raised", () =>
        {
            ChangedEventHandled = false;
            Element.RaiseChanged();
        });
        Then("the Changed event should not be handled", () => !ChangedEventHandled);
    }

    [Example("Removes event handlers and sets null to elements and a data context when the WpfController is disabled")]
    void Ex04()
    {
        Given("an element that contains the data context", () => Element = new TestElement { Name = "Element", DataContext = new TestDataContexts.TestDataContext() });

        When("the WpfController is enabled for the element", () =>
        {
            WpfController.SetIsEnabled(Element, true);
            Controller = Element.GetController<TestWpfControllers.TestWpfController>();
        });
        Then("the data context of the controller should be set", () => Controller.DataContext == Element.DataContext);

        When("the Initialized event is raised", () => Element.RaiseInitialized());
        Then("the element of the controller should be set", () => Controller.Element == Element);

        When("the Loaded event is raised", () =>
        {
            Controller.LoadedAssertionHandler = () => LoadedEventHandled = true;
            Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = Element });
        });
        Then("the Loaded event should be handled", () => LoadedEventHandled);

        When("the Changed event is raised", () =>
        {
            Controller.ChangedAssertionHandler = () => ChangedEventHandled = true;
            Element.RaiseChanged();
        });
        Then("the Changed event should be handled", () => ChangedEventHandled);

        When("the WpfController is disabled for the element", () => WpfController.SetIsEnabled(Element, false));
        Then("the controller should be detached", () => !WpfController.GetControllers(Element).Any());
        Then("the data context of the controller should be null", () => Controller.DataContext == null);
        Then("the element of the controller should be null", () => Controller.Element == null);

        When("the Loaded event is raised", () =>
        {
            LoadedEventHandled = false;
            Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = Element });
        });
        Then("the Loaded event should not be handled", () => !LoadedEventHandled);

        When("the Changed event is raised", () =>
        {
            ChangedEventHandled = false;
            Element.RaiseChanged();
        });
        Then("the Changed event should not be handled", () => !ChangedEventHandled);

        When("the Initialized event is raised", () => Element.RaiseInitialized());
        Then("the controller should not be attached", () => !WpfController.GetControllers(Element).Any());
    }

    [Example("Attaches multi controllers")]
    void Ex05()
    {
        Given("an element that contains the data context", () => Element = new TestElement { Name = "Element", DataContext = new TestDataContexts.MultiTestDataContext() });

        When("the WpfController is enabled for the element", () =>
        {
            WpfController.SetIsEnabled(Element, true);
            Controllers = new TestWpfControllers.TestWpfControllerBase[] { Element.GetController<TestWpfControllers.MultiTestWpfControllerA>(), Element.GetController<TestWpfControllers.MultiTestWpfControllerB>(), Element.GetController<TestWpfControllers.MultiTestWpfControllerC>() };
            LoadedEventsHandled = new[] { false, false, false };
            ChangedEventsHandled = new[] { false, false, false };
        });
        Then("the data context of the controller should be set", () => Controllers.All(controller => Equals(controller.DataContext, Element.DataContext)));

        When("the Initialized event is raised", () => Element.RaiseInitialized());
        Then("the element of the controller should be set", () => Controllers.All(controller => Equals(controller.Element, Element)));

        When("the Loaded event is raised", () =>
        {
            for (var index = 0; index < Controllers.Length; ++index)
            {
                var eventHandledIndex = index;
                Controllers[index].LoadedAssertionHandler = () => LoadedEventsHandled[eventHandledIndex] = true;
            }

            Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = Element });
        });
        Then("the Loaded event should be handled", () => LoadedEventsHandled.All(handled => handled));

        When("the Changed event is raised", () =>
        {
            for (var index = 0; index < Controllers.Length; ++index)
            {
                var eventHandledIndex = index;
                Controllers[index].ChangedAssertionHandler = () => ChangedEventsHandled[eventHandledIndex] = true;
            }

            Element.RaiseChanged();
        });
        Then("the Changed event should be handled", () => ChangedEventsHandled.All(handled => handled));

        When("the Unloaded event is raised", () => Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = Element }));
        Then("the data context of the controller should be null", () => Controllers.All(controller => controller.DataContext == null));
        Then("the element of the controller should be null", () => Controllers.All(controller => controller.Element == null));

        When("the Loaded event is raised", () =>
        {
            for (var index = 0; index < LoadedEventsHandled.Length; ++index)
            {
                LoadedEventsHandled[index] = false;
            }

            Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.LoadedEvent) { Source = Element });
        });
        Then("the Loaded event should not be handled", () => LoadedEventsHandled.All(handled => !handled));

        When("the Changed event is raised", () =>
        {
            for (var index = 0; index < ChangedEventsHandled.Length; ++index)
            {
                ChangedEventsHandled[index] = false;
            }

            Element.RaiseChanged();
        });
        Then("the Changed event should not be handled", () => ChangedEventsHandled.All(handled => !handled));
    }
}