﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Globalization;
using System.Windows.Data;
using Charites.Windows.Samples.SimpleTodo.Contents;

namespace Charites.Windows.Samples.SimpleTodo.Converters;

[ValueConversion(typeof(TodoItemState), typeof(bool))]
public class TodoItemStateToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not TodoItemState state) throw new ArgumentException(nameof(value));

        return state == TodoItemState.Completed;
    }

    public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
    {
        if (value is not bool targetValue) throw new ArgumentException(nameof(value));

        return targetValue ? TodoItemState.Completed: TodoItemState.Active;
    }
}