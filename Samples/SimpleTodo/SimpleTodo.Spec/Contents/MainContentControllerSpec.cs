// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Input;
using Carna;
using Charites.Windows.Mvc;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleTodo.Contents;

[Specification("MainContentController Spec", RequiresSta = true)]
class MainContentControllerSpec : FixtureSteppable
{
    MainContent MainContent { get; } = new();
    MainContentController Controller { get; } = new();

    [Background("a controller that has a to-do item")]
    public MainContentControllerSpec()
    {
        WpfController.SetDataContext(MainContent, Controller);
    }

    [Example("A to-do item is added when the Enter key is pressed")]
    void Ex01()
    {
        When("the content of the to-do is set", () => MainContent.TodoContent.Value = "Todo Item");
        When("the Enter key is pressed", () =>
            WpfController.EventHandlersOf(Controller)
                .GetBy("TodoContentTextBox")
                .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Enter))
                .Raise(UIElement.KeyDownEvent.Name)
        );
        Then("a to-do item should be added", () => MainContent.TodoItems.Count == 1);
    }

    [Example("A to-do item is not added when the Tab key is pressed")]
    void Ex02()
    {
        When("the content of the to-do is set", () => MainContent.TodoContent.Value = "Todo Item");
        When("the Tab key is pressed", () =>
            WpfController.EventHandlersOf(Controller)
                .GetBy("TodoContentTextBox")
                .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Tab))
                .Raise(UIElement.KeyDownEvent.Name)
        );
        Then("a to-do item should not be added", () => !MainContent.TodoItems.Any());
    }
}