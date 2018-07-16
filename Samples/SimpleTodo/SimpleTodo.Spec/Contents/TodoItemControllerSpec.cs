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
using FluentAssertions;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleTodo.Contents
{
    [Specification("TodoItemController Spec")]
    class TodoItemControllerSpec : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string InitialContent = "Todo Item";
        const string ModifiedContent = "Todo Item Modified";

        const string TodoItemKey = "TodoItem";
        const string ControllerKey = "Controller";

        TodoItemController Controller { get; } = new TodoItemController();
        TodoItem TodoItem { get; } = new TodoItem(InitialContent);
        EventHandler RemoveRequestedHandler { get; } = Substitute.For<EventHandler>();

        public TodoItemControllerSpec()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("Switches a content to an edit mode when the element is double clicked")]
        void Ex01()
        {
            Given("a controller that has a to-do item", () => WpfRunner.Run((application, context) =>
            {
                var todoItem = new TodoItem(InitialContent);
                var controller = new TodoItemController();
                WpfController.SetDataContext(todoItem, controller);

                context.Set(TodoItemKey, todoItem);
                context.Set(ControllerKey, controller);
            }));
            When("the element is double clicked", () => WpfRunner.Run((application, context) =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            }));
            Then("the to-do item should be editing", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Editing.Value.Should().BeTrue()
            ));
            Then("the edit content of the to-do item should be the initial content", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).EditContent.Value.Should().Be(InitialContent)
            ));
        }

        [Example("Completes and edit when the Enter key is pressed")]
        void Ex02()
        {
            Given("a controller that has a to-do item", () => WpfRunner.Run((application, context) =>
            {
                var todoItem = new TodoItem(InitialContent);
                var controller = new TodoItemController();
                WpfController.SetDataContext(todoItem, controller);

                context.Set(TodoItemKey, todoItem);
                context.Set(ControllerKey, controller);
            }));
            When("the element is double clicked", () => WpfRunner.Run((application, context) =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            }));
            When("the content is modified", () => WpfRunner.Run((application, context) => context.Get<TodoItem>(TodoItemKey).EditContent.Value = ModifiedContent));
            When("the Enter key is pressed", () => WpfRunner.Run((application, context) =>
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Enter))
                    .Raise(UIElement.KeyDownEvent.Name)
            ));
            Then("the to-do item should not be editing", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Editing.Value.Should().BeFalse()
            ));
            Then("the content of the to-do item should be the modified content", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Content.Value.Should().Be(ModifiedContent)
            ));
        }

        [Example("Completes and edit when the focus is lost")]
        void Ex03()
        {
            Given("a controller that has a to-do item", () => WpfRunner.Run((application, context) =>
            {
                var todoItem = new TodoItem(InitialContent);
                var controller = new TodoItemController();
                WpfController.SetDataContext(todoItem, controller);

                context.Set(TodoItemKey, todoItem);
                context.Set(ControllerKey, controller);
            }));
            When("the element is double clicked", () => WpfRunner.Run((application, context) =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            }));
            When("the content is modified", () => WpfRunner.Run((application, context) => context.Get<TodoItem>(TodoItemKey).EditContent.Value = ModifiedContent));
            When("the focus is lost ", () => WpfRunner.Run((application, context) =>
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBox")
                    .Raise(UIElement.LostFocusEvent.Name)
            ));
            Then("the to-do item should not be editing", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Editing.Value.Should().BeFalse()
            ));
            Then("the content of the to-do item should be the modified content", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Content.Value.Should().Be(ModifiedContent)
            ));
        }

        [Example("Cancels and edit when the Esc key is pressed")]
        void Ex04()
        {
            Given("a controller that has a to-do item", () => WpfRunner.Run((application, context) =>
            {
                var todoItem = new TodoItem(InitialContent);
                var controller = new TodoItemController();
                WpfController.SetDataContext(todoItem, controller);

                context.Set(TodoItemKey, todoItem);
                context.Set(ControllerKey, controller);
            }));
            When("the element is double clicked", () => WpfRunner.Run((application, context) =>
            {
                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty(nameof(MouseButtonEventArgs.ClickCount))?.SetValue(e, 2);
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);
            }));
            When("the content is modified", () => WpfRunner.Run((application, context) => context.Get<TodoItem>(TodoItemKey).EditContent.Value = ModifiedContent));
            When("the Esc key is pressed", () => WpfRunner.Run((application, context) =>
                WpfController.EventHandlersOf(context.Get<TodoItemController>(ControllerKey))
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Escape))
                    .Raise(UIElement.KeyDownEvent.Name)
            ));
            Then("the to-do item should not be editing", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Editing.Value.Should().BeFalse()
            ));
            Then("the content of the to-do item should be the initial content", () => WpfRunner.Run((application, context) =>
                context.Get<TodoItem>(TodoItemKey).Content.Value.Should().Be(InitialContent)
            ));
        }

        [Example("Removes a to-do item when the DeleteTodoItem command is executed")]
        void Ex05()
        {
            Given("a controller that has a to-do item", () =>
            {
                TodoItem.RemoveRequested += RemoveRequestedHandler;
                WpfController.SetDataContext(TodoItem, Controller);
            });
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
