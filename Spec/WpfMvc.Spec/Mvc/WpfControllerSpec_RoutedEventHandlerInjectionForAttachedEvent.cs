// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Carna;

namespace Charites.Windows.Mvc;

[Context("RoutedEventHandler injection for an attached event")]
class WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent : FixtureSteppable
{
    Button Button { get; } = new();
    TestElement Element { get; }

    bool ClickEventHandled { get; set; }

    public WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent()
    {
        Element = new TestElement
        {
            Name = "Element",
            Content = new ContentControl { Content = Button },
            DataContext = new TestDataContexts.TestDataContext()
        };
    }

    [Example("Adds an event handler for an attached event")]
    void Ex01()
    {
        When("the WpfController is enabled for the element", () => WpfController.SetIsEnabled(Element, true));
        When("the Initialized event is raised", () => Element.RaiseInitialized());
        When("the Click event of the button is raised", () =>
        {
            Element.GetController<TestWpfControllers.TestWpfController>().ButtonClickAssertionHandler += () => ClickEventHandled = true;
            Button.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent) { Source = Button });
        });
        Then("the Click event should be handled", () => ClickEventHandled);
    }
}