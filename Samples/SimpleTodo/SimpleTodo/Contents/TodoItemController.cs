// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Fievus.Windows.Mvc;

namespace Fievus.Windows.Samples.SimpleTodo.Contents
{
    public class TodoItemController
    {
        [DataContext]
        public TodoItem TodoItem { get; set; }

        [Element]
        private TextBox TodoContentTextBox { get; set; }

        [RoutedEventHandler(RoutedEvent = "Loaded")]
        private void OnLoaded()
        {
            TodoContentTextBox.IsVisibleChanged += OnTodoContentTextBoxIsVisibleChanged;
        }

        private void OnTodoContentTextBoxIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var todoContentTextBox = sender as TextBox;
            if (todoContentTextBox == null) { return; }
            if (!((bool)e.NewValue)) { return; }

            todoContentTextBox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                todoContentTextBox.Focus();
                todoContentTextBox.SelectAll();
            }));
        }

        [RoutedEventHandler(ElementName = "TodoContentTextBlock", RoutedEvent = "MouseLeftButtonDown")]
        private void OnTodoContentTextBlockMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) { return; }

            TodoItem.StartEdit();
        }

        [RoutedEventHandler(ElementName = "TodoContentTextBox", RoutedEvent = "KeyDown")]
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

        [RoutedEventHandler(ElementName = "TodoContentTextBox", RoutedEvent = "LostFocus")]
        private void OnTodoContentTextBoxLostFocus()
        {
            if (!TodoItem.Editing.Value) { return; }

            TodoItem.CompleteEdit();
        }

        [CommandHandler(CommandName = "DeleteTodoItem")]
        private void DeleteTodoItem(ExecutedRoutedEventArgs e)
        {
            TodoItem.Remove();
        }
    }
}
