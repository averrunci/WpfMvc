// Copyright (C) 2022-2025 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System.Collections.Specialized;
using Charites.Windows.Mvc.Bindings;

namespace Charites.Windows.Samples.SimpleTodo.Contents;

public class MainContent
{
    public ObservableProperty<bool?> AllCompleted { get; } = new(false);
    public ObservableProperty<bool> CanCompleteAllTodoItems { get; } = false.ToObservableProperty();

    public ObservableProperty<string> TodoContent { get; } = string.Empty.ToObservableProperty();

    public SynchronizationObservableCollection<TodoItem> TodoItems { get; } = new();
    private readonly List<TodoItem> todoItems = new();

    public ObservableProperty<string> ItemsLeftMessage { get; } = string.Empty.ToObservableProperty();

    public ObservableProperty<TodoItemState> TodoItemDisplayState { get; } = TodoItemState.All.ToObservableProperty();

    public MainContent()
    {
        AllCompleted.PropertyValueChanged += OnAllCompletedPropertyValueChanged;
        TodoItems.CollectionChanged += OnTodoItemsCollectionChanged;
        TodoItemDisplayState.PropertyValueChanged += OnTodoItemsDisplayStatePropertyValueChanged;

        UpdateItemsLeftMessage();
    }

    public void AddCurrentTodoContent()
    {
        if (string.IsNullOrWhiteSpace(TodoContent.Value)) return;

        var newTodoItem = new TodoItem(TodoContent.Value);
        newTodoItem.RemoveRequested += OnTodoItemRemoveRequested;
        newTodoItem.State.PropertyValueChanged += OnTodoItemStateChanged;
        todoItems.Add(newTodoItem);
        TodoItems.Add(newTodoItem);

        TodoContent.Value = string.Empty;
        ApplyFilter();
    }

    private void OnAllCompletedPropertyValueChanged(object? sender, PropertyValueChangedEventArgs<bool?> e)
    {
        UpdateAllTodoItemsState();
    }

    private void OnTodoItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateAllTodoItemsCompletionState();
        UpdateAllTodoItemsCompletionEnabled();
        UpdateItemsLeftMessage();
    }

    private void OnTodoItemsDisplayStatePropertyValueChanged(object? sender, PropertyValueChangedEventArgs<TodoItemState> e)
    {
        ApplyFilter();
    }

    private void OnTodoItemRemoveRequested(object? sender, EventArgs e)
    {
        if (sender is not TodoItem removedTodoItem) return;

        removedTodoItem.RemoveRequested -= OnTodoItemRemoveRequested;
        removedTodoItem.State.PropertyValueChanged -= OnTodoItemStateChanged;
        todoItems.Remove(removedTodoItem);
        TodoItems.Remove(removedTodoItem);
    }

    private void OnTodoItemStateChanged(object? sender, PropertyValueChangedEventArgs<TodoItemState> e)
    {
        UpdateAllTodoItemsCompletionState();
        UpdateItemsLeftMessage();
        ApplyFilter();
    }

    private void UpdateAllTodoItemsState()
    {
        if (!AllCompleted.Value.HasValue) return;

        foreach (var item in todoItems)
        {
            item.State.PropertyValueChanged -= OnTodoItemStateChanged;
            try
            {
                item.State.Value = AllCompleted.Value.GetValueOrDefault() ? TodoItemState.Completed : TodoItemState.Active;
            }
            finally
            {
                item.State.PropertyValueChanged += OnTodoItemStateChanged;
            }
        }

        UpdateItemsLeftMessage();
        ApplyFilter();
    }

    private void UpdateAllTodoItemsCompletionState()
    {
        AllCompleted.PropertyValueChanged -= OnAllCompletedPropertyValueChanged;
        try
        {
            if (todoItems.All(i => i.State.Value == TodoItemState.Active))
            {
                AllCompleted.Value = false;
            }
            else if (todoItems.All(i => i.State.Value == TodoItemState.Completed))
            {
                AllCompleted.Value = true;
            }
            else
            {
                AllCompleted.Value = null;
            }
        }
        finally
        {
            AllCompleted.PropertyValueChanged += OnAllCompletedPropertyValueChanged;
        }
    }

    private void UpdateAllTodoItemsCompletionEnabled()
    {
        CanCompleteAllTodoItems.Value = TodoItems.Any();
    }

    private void UpdateItemsLeftMessage()
    {
        var activeCount = todoItems.Count(i => i.State.Value == TodoItemState.Active);
        ItemsLeftMessage.Value = $"{activeCount} item{(activeCount == 1 ? string.Empty : "s")} left";
    }

    private void ApplyFilter()
    {
        TodoItems.Clear();
        foreach (var item in TodoItemDisplayState.Value == TodoItemState.All ? todoItems : todoItems.Where(i => i.State.Value == TodoItemDisplayState.Value))
        {
            TodoItems.Add(item);
        }
    }
}