// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Input;
using Carna;
using Charites.Windows.Mvc;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleTodo.Contents
{
    [Specification("TodoItemController Spec", RequiresSta = true)]
    class TodoItemControllerSpec : FixtureSteppable
    {
        const string InitialContent = "Todo Item";
        const string ModifiedContent = "Todo Item Modified";

        TodoItemController Controller { get; } = new TodoItemController();
        TodoItem TodoItem { get; } = new TodoItem(InitialContent);
        EventHandler RemoveRequestedHandler { get; } = Substitute.For<EventHandler>();

        [Background("a controller that has a to-do item")]
        public TodoItemControllerSpec()
        {
            TodoItem.RemoveRequested += RemoveRequestedHandler;
            WpfController.SetDataContext(TodoItem, Controller);
        }

        [Example("Switches a content to an edit mode when the element is double clicked")]
        void Ex01()
        {
            When("the element is double clicked", () =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            });
            Then("the to-do item should be editing", () => TodoItem.Editing.Value);
            Then("the edit content of the to-do item should be the initial content", () => TodoItem.EditContent.Value == InitialContent);
        }

        [Example("Completes and edit when the Enter key is pressed")]
        void Ex02()
        {
            When("the element is double clicked", () =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            });
            When("the content is modified", () => TodoItem.EditContent.Value = ModifiedContent);
            When("the Enter key is pressed", () =>
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Enter))
                    .Raise(UIElement.KeyDownEvent.Name)
            );
            Then("the to-do item should not be editing", () => !TodoItem.Editing.Value);
            Then("the content of the to-do item should be the modified content", () => TodoItem.Content.Value == ModifiedContent);
        }

        [Example("Completes and edit when the focus is lost")]
        void Ex03()
        {
            When("the element is double clicked", () =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            });
            When("the content is modified", () => TodoItem.EditContent.Value = ModifiedContent);
            When("the focus is lost ", () =>
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBox")
                    .Raise(UIElement.LostFocusEvent.Name)
            );
            Then("the to-do item should not be editing", () => !TodoItem.Editing.Value);
            Then("the content of the to-do item should be the modified content", () => TodoItem.Content.Value == ModifiedContent);
        }

        [Example("Cancels and edit when the Esc key is pressed")]
        void Ex04()
        {
            When("the element is double clicked", () =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            });
            When("the content is modified", () => TodoItem.EditContent.Value = ModifiedContent);
            When("the Esc key is pressed", () =>
                WpfController.EventHandlersOf(Controller)
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Escape))
                    .Raise(UIElement.KeyDownEvent.Name)
            );
            Then("the to-do item should not be editing", () => !TodoItem.Editing.Value);
            Then("the content of the to-do item should be the initial content", () => TodoItem.Content.Value == InitialContent);
        }

        [Example("Removes a to-do item when the DeleteTodoItem command is executed")]
        void Ex05()
        {
            When("the DeleteTodoItem command is executed", () =>
                WpfController.CommandHandlersOf(Controller)
                    .GetBy(SimpleTodoCommands.DeleteTodoItem.Name)
                    .With(SimpleTodoCommands.DeleteTodoItem)
                    .RaiseExecuted(TodoItem)
            );
            Then("it should be requested to remove the to-do item", () =>
                RemoveRequestedHandler.Received(1).Invoke(TodoItem, EventArgs.Empty)
            );
        }
    }
}
