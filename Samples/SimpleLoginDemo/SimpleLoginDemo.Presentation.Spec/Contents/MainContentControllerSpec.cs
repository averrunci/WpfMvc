// Copyright (C) 2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using Carna;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents
{
    [Specification("MainContentController Spec")]
    class MainContentControllerSpec : FixtureSteppable
    {
        MainContentController Controller { get; }

        MainContent Content { get; } = new();
        IContentNavigator Navigator { get; } = Substitute.For<IContentNavigator>();

        object NextContent { get; } = new();

        public MainContentControllerSpec()
        {
            Controller = new MainContentController(Navigator);

            WpfController.SetDataContext(Content, Controller);
        }

        [Example("Sets an initial content")]
        void Ex01()
        {
            When("the MainContent is loaded", () =>
                WpfController.EventHandlersOf(Controller)
                    .GetBy(null)
                    .Raise(FrameworkElement.LoadedEvent.Name)
            );
            Then("the content should be navigated to the LoginContent", () =>
            {
                Navigator.Received(1).NavigateTo(Arg.Any<LoginContent>());
            });
        }

        [Example("Sets a new content when the current content is navigated")]
        void Ex02()
        {
            When("the content is navigated to the next content", () =>
                Navigator.Navigated += Raise.EventWith(Navigator, new ContentNavigatedEventArgs(ContentNavigationMode.New, NextContent, Content))
            );
            Then("the content to navigate should be set to the content of the MainContent", () => Content.Content.Value == NextContent);
        }
    }
}
