// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Input;
using Charites.Windows.Samples.SimpleTodo.Properties;

namespace Charites.Windows.Samples.SimpleTodo;

public static class SimpleTodoCommands
{
    public static readonly RoutedUICommand DeleteTodoItem = new(Resources.DeleteTodoItemCommandText, nameof(DeleteTodoItem), typeof(SimpleTodoCommands));
}