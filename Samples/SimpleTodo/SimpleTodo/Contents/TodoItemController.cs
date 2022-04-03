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

[View(Key = nameof(Contents.TodoItem))]
public class TodoItemController
{
    [DataContext]
    private TodoItem? TodoItem { get; set; }

    [Element]
    private TextBox? TodoContentTextBox { get; set; }

    [EventHandler(Event = nameof(FrameworkElement.Loaded))]
    private void OnLoaded()
    {
        if (TodoContentTextBox is not null) TodoContentTextBox.IsVisibleChanged += TodoContentTextBox_IsVisibleChanged;
    }

    private void TodoContentTextBox_IsVisibleChanged(object? sender, DependencyPropertyChangedEventArgs e)
    {
        if (sender is not TextBox todoContentTextBox) return;
        if (!((bool)e.NewValue)) return;

        todoContentTextBox.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
        {
            todoContentTextBox.Focus();
            todoContentTextBox.SelectAll();
        }));
    }

    private void TodoContentTextBlock_MouseLeftButtonDown(MouseButtonEventArgs e)
    {
        if (e.ClickCount is not 2) return;

        TodoItem?.StartEdit();
    }

    private void TodoContentTextBox_KeyDown(KeyEventArgs e)
    {
        switch (e.Key)
        {
            case Key.Enter:
                TodoItem?.CompleteEdit();
                break;
            case Key.Escape:
                TodoItem?.CancelEdit();
                break;
        }
    }

    private void TodoContentTextBox_LostFocus()
    {
        if (!(TodoItem?.Editing.Value ?? false)) return;

        TodoItem.CompleteEdit();
    }

    private void DeleteTodoItem_Executed()
    {
        TodoItem?.Remove();
    }
}