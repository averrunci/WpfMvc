// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Globalization;
using System.Windows.Data;

using Fievus.Windows.Samples.SimpleTodo.Contents;

namespace Fievus.Windows.Samples.SimpleTodo.Converters
{
    [ValueConversion(typeof(TodoItemState), typeof(bool))]
    public class TodoItemDisplayStateToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is TodoItemState)) { throw new ArgumentException(nameof(value)); }
            if (!(parameter is TodoItemState)) { throw new ArgumentException(nameof(parameter)); }

            return (TodoItemState)value == (TodoItemState)parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool)) { throw new ArgumentException(nameof(value)); }
            if (!(parameter is TodoItemState)) { throw new ArgumentException(nameof(parameter)); }

            return (bool)value ? (TodoItemState)parameter : TodoItemState.All;
        }
    }
}
