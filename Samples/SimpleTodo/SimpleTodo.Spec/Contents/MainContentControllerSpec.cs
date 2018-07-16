// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System;
using System.Windows;
using System.Windows.Input;
using Carna;
using Charites.Windows.Mvc;
using Charites.Windows.Runners;
using Charites.Windows.Samples.SimpleTodo.Contents;
using FluentAssertions;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleTodo.Test.Content
{
    [Specification("MainContentController Spec")]
    class MainContentControllerSpec : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string ControllerKey = "Controller";
        const string MainContentKey = "MainContent";

        public MainContentControllerSpec()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("A to-do item is added when the Enter key is pressed")]
        void Ex01()
        {
            Given("a controller that has a to-do item", () => WpfRunner.Run((application, context) =>
            {
                var mainContent = new MainContent();
                var controller = new MainContentController();
                WpfController.SetDataContext(mainContent, controller);

                context.Set(ControllerKey, controller);
                context.Set(MainContentKey, mainContent);
            }));
            When("the content of the to-do is set", () => WpfRunner.Run((application, context) => context.Get<MainContent>(MainContentKey).TodoContent.Value = "Todo Item"));
            When("the Enter key is pressed", () => WpfRunner.Run((application, context) =>
                WpfController.EventHandlersOf(context.Get<MainContentController>(ControllerKey))
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Enter))
                    .Raise(UIElement.KeyDownEvent.Name)
            ));
            Then("a to-do item should be added", () => WpfRunner.Run((application, context) => context.Get<MainContent>(MainContentKey).TodoItems.Count.Should().Be(1)));
        }

        [Example("A to-do item is not added when the Tab key is pressed")]
        void Ex02()
        {
            Given("a controller that has a to-do item", () => WpfRunner.Run((application, context) =>
            {
                var mainContent = new MainContent();
                var controller = new MainContentController();
                WpfController.SetDataContext(mainContent, controller);

                context.Set(ControllerKey, controller);
                context.Set(MainContentKey, mainContent);
            }));
            When("the content of the to-do is set", () => WpfRunner.Run((application, context) => context.Get<MainContent>(MainContentKey).TodoContent.Value = "Todo Item"));
            When("the Tab key is pressed", () => WpfRunner.Run((application, context) =>
                WpfController.EventHandlersOf(context.Get<MainContentController>(ControllerKey))
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Tab))
                    .Raise(UIElement.KeyDownEvent.Name)
            ));
            Then("a to-do item should not be added", () => WpfRunner.Run((application, context) => context.Get<MainContent>(MainContentKey).TodoItems.Should().BeEmpty()));
        }
    }
}
