// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Input;

using Fievus.Windows.Samples.SimpleTodo.Contents;

using NUnit.Framework;

using NSubstitute;

using Fievus.Windows.Mvc;
using Fievus.Windows.Runners;

namespace Fievus.Windows.Samples.SimpleTodo.Test.Content
{
    [TestFixture]
    public class TodoItemControllerTest
    {
        [Test]
        public void ShouldSwitchToEditModeOnLeftButtonDoubleClick()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var controller = new TodoItemController
                {
                    TodoItem = new TodoItem("Todo Item")
                };

                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty("ClickCount").SetValue(e, 2);
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);

                Assert.That(controller.TodoItem.Editing.Value, Is.True);
                Assert.That(controller.TodoItem.EditContent.Value, Is.EqualTo("Todo Item"));
            }).Shutdown();
        }

        [Test]
        public void ShouldCompleteEditOnEnterClick()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var controller = new TodoItemController
                {
                    TodoItem = new TodoItem("Todo Item")
                };

                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty("ClickCount").SetValue(e, 2);
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);

                controller.TodoItem.EditContent.Value = "Todo Item Modified";
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Enter))
                    .Raise(UIElement.KeyDownEvent.Name);

                Assert.That(controller.TodoItem.Editing.Value, Is.False);
                Assert.That(controller.TodoItem.Content.Value, Is.EqualTo("Todo Item Modified"));
            }).Shutdown();
        }

        [Test]
        public void ShouldCompleteEditOnLostFocus()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var controller = new TodoItemController
                {
                    TodoItem = new TodoItem("Todo Item")
                };

                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty("ClickCount").SetValue(e, 2);
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);

                controller.TodoItem.EditContent.Value = "Todo Item Modified";
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBox")
                    .Raise(UIElement.LostFocusEvent.Name);

                Assert.That(controller.TodoItem.Editing.Value, Is.False);
                Assert.That(controller.TodoItem.Content.Value, Is.EqualTo("Todo Item Modified"));
            }).Shutdown();
        }

        [Test]
        public void ShouldCancelEditOnEscClick()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var controller = new TodoItemController
                {
                    TodoItem = new TodoItem("Todo Item")
                };

                var e = new MouseButtonEventArgs(Mouse.PrimaryDevice, 0, MouseButton.Left)
                {
                    RoutedEvent = UIElement.MouseLeftButtonDownEvent
                };
                e.GetType().GetProperty("ClickCount").SetValue(e, 2);
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBlock")
                    .With(e)
                    .Raise(UIElement.MouseLeftButtonDownEvent.Name);

                controller.TodoItem.EditContent.Value = "Todo Item Modified";
                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy("TodoContentTextBox")
                    .With(new KeyEventArgs(Keyboard.PrimaryDevice, Substitute.For<PresentationSource>(), 0, Key.Escape))
                    .Raise(UIElement.KeyDownEvent.Name);

                Assert.That(controller.TodoItem.Editing.Value, Is.False);
                Assert.That(controller.TodoItem.Content.Value, Is.EqualTo("Todo Item"));
            }).Shutdown();
        }

        [Test]
        public void ShouldRemoveTodoItemOnDeleteTodoItemCommand()
        {
            var controller = new TodoItemController
            {
                TodoItem = new TodoItem("Todo Item")
            };

            var handler = Substitute.For<EventHandler>();
            controller.TodoItem.RemoveRequested += handler;

            WpfController.RetrieveCommandHandlers(controller)
                .GetBy(SimpleTodoCommands.DeleteTodoItem.Name)
                .With(SimpleTodoCommands.DeleteTodoItem)
                .RaiseExecuted(controller.TodoItem);

            handler.Received().Invoke(controller.TodoItem, EventArgs.Empty);
        }
    }
}
