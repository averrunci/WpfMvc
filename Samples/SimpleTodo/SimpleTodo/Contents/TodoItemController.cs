// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleTodo.Contents;

[View(Key = nameof(TodoItem))]
public class TodoItemController
{
    private void TodoContentTextBox_IsVisibleChanged(object? sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not TextBox textBox) return;
        if (!(bool)e.NewValue) return;

        textBox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            textBox.Focus();
            textBox.SelectAll();
        }));
    }

    private void TodoContentTextBlock_MouseLeftButtonDown(MouseButtonEventArgs e, [FromDataContext] TodoItem todoItem)
    {
        if (e.ClickCount is not 2) return;

        todoItem.StartEdit();
    }

    private void TodoContentTextBox_KeyDown(KeyEventArgs e, [FromDataContext] TodoItem todoItem)
    {
        switch (e.Key)
        {
            case Key.Enter:
                todoItem.CompleteEdit();
                break;
            case Key.Escape:
                todoItem.CancelEdit();
                break;
        }
    }

    private void TodoContentTextBox_LostFocus([FromDataContext] TodoItem todoItem)
    {
        if (!todoItem.Editing.Value) return;

        todoItem.CompleteEdit();
    }

    private void DeleteTodoItem_Executed([FromDataContext] TodoItem todoItem)
    {
        todoItem.Remove();
    }
}