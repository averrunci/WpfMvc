// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Input;

using Fievus.Windows.Samples.SimpleTodo.Properties;

namespace Fievus.Windows.Samples.SimpleTodo
{
    public static class SimpleTodoCommands
    {
        public static readonly RoutedUICommand DeleteTodoItem = new RoutedUICommand(Resources.DeleteTodoItemCommandText, "DeleteTodoItem", typeof(SimpleTodoCommands));
    }
}
