// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("RoutedEventHandler injection for an attached event")]
    class WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string ButtonKey = "Button";
        const string ElementKey = "Element";
        const string ClickEventHandledKey = "ClickEventHandled";

        public WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("Adds event handler for an attached event")]
        void Ex01()
        {
            Given("a button attached as an attached event", () => WpfRunner.Run((application, context) => context.Set(ButtonKey, new Button())));
            Given("an element that contains the button", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "Element", Content = new ContentControl { Content = context.Get<Button>(ButtonKey) }, DataContext = new TestDataContexts.TestDataContext() })));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) => WpfController.SetIsEnabled(context.Get<TestElement>(ElementKey), true)));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));
            When("the Click event of the button is raised", () => WpfRunner.Run((application, context) =>
            {
                context.Get<TestElement>(ElementKey).GetController<TestWpfControllers.TestWpfController>().ButtonClickAssertionHandler += () => context.Set(ClickEventHandledKey, true);
                context.Get<Button>(ButtonKey).RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent) { Source = context.Get<Button>(ButtonKey) });
            }));
            Then("the Click event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ClickEventHandledKey).Should().BeTrue()));
        }
    }
}
