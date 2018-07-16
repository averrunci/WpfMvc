// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleTodo.Contents
{
    [View(Key = nameof(Contents.TodoItem))]
    public class TodoItemController
    {
        [DataContext]
        private TodoItem TodoItem { get; set; }

        [Element]
        private TextBox TodoContentTextBox { get; set; }

        [EventHandler(Event = nameof(FrameworkElement.Loaded))]
        private void OnLoaded()
        {
            TodoContentTextBox.IsVisibleChanged += OnTodoContentTextBoxIsVisibleChanged;
        }

        private void OnTodoContentTextBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is TextBox todoContentTextBox)) return;
            if (!((bool)e.NewValue)) return;

            todoContentTextBox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                todoContentTextBox.Focus();
                todoContentTextBox.SelectAll();
            }));
        }

        [EventHandler(ElementName = "TodoContentTextBlock", Event = nameof(UIElement.MouseLeftButtonDown))]
        private void OnTodoContentTextBlockMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;

            TodoItem.StartEdit();
        }

        [EventHandler(ElementName = nameof(TodoContentTextBox), Event = nameof(UIElement.KeyDown))]
        private void OnTodoContentTextBoxKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    TodoItem.CompleteEdit();
                    break;
                case Key.Escape:
                    TodoItem.CancelEdit();
                    break;
            }
        }

        [EventHandler(ElementName = nameof(TodoContentTextBox), Event = nameof(UIElement.LostFocus))]
        private void OnTodoContentTextBoxLostFocus()
        {
            if (!TodoItem.Editing.Value) return;

            TodoItem.CompleteEdit();
        }

        [CommandHandler(CommandName = nameof(SimpleTodoCommands.DeleteTodoItem))]
        private void DeleteTodoItem(ExecutedRoutedEventArgs e)
        {
            TodoItem.Remove();
        }
    }
}
