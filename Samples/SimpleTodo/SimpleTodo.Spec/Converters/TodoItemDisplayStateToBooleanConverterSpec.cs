// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System;
using System.Collections;
using Carna;
using Charites.Windows.Samples.SimpleTodo.Contents;

namespace Charites.Windows.Samples.SimpleTodo.Converters
{
    [Specification("TodoItemDisplayStateToBooleanConverter Spec")]
    class TodoItemDisplayStateToBooleanConverterSpec : FixtureSteppable
    {
        TodoItemDisplayStateToBooleanConverter Converter { get; } = new TodoItemDisplayStateToBooleanConverter();

        [Example("Converts a display state of a TodoItem with a parameter to a boolean value")]
        [Sample(TodoItemState.All, TodoItemState.All, true, Description = "When a display state of a TodoItem is equal to a parameter")]
        [Sample(TodoItemState.All, TodoItemState.Active, Description = "When a display state of a TodoItem is not equal to a parameter")]
        void Ex01(TodoItemState value, TodoItemState parameter, bool expected)
        {
            Expect($"the converted value should be {expected}", () => Equals(Converter.Convert(value, value.GetType(), parameter, null), expected));
        }

        [Example("Converts a boolean value with a parameter back to a display state of a TodoItem")]
        [Sample(true, TodoItemState.Completed, TodoItemState.Completed, Description = "When a value is true and a parameter is 'Completed'")]
        [Sample(true, TodoItemState.Active, TodoItemState.Active, Description = "When a value is true and a parameter is 'Active'")]
        [Sample(true, TodoItemState.All, TodoItemState.All, Description = "When a value is true and a parameter is 'All'")]
        [Sample(false, TodoItemState.Completed, TodoItemState.All, Description = "When a value is false and a parameter is 'Completed'")]
        [Sample(false, TodoItemState.Active, TodoItemState.All, Description = "When a value is false and a parameter is 'Active'")]
        [Sample(false, TodoItemState.All, TodoItemState.All, Description = "When a value is false and a parameter is 'All'")]
        void Ex02(bool value, TodoItemState parameter, TodoItemState expected)
        {
            Expect($"the converted value should be {expected}", () => Equals(Converter.ConvertBack(value, value.GetType(), parameter, null), expected));
        }

        [Example("Converts a value whose type is invalid")]
        [Sample(Source = typeof(InvalidConvertingDataSource))]
        void Ex03(object value, object parameter)
        {
            When("a value whose type is invalid is converted", () => Converter.ConvertBack(value, value.GetType(), parameter, null));
            Then<ArgumentException>($"{typeof(ArgumentException)} should be thrown");
        }

        [Example("Converts back a value whose type is invalid")]
        [Sample(Source = typeof(InvalidConvertingBackDataSource))]
        void Ex04(object value, object parameter)
        {
            When("a value whose type is invalid is converted back", () => Converter.ConvertBack(value, value.GetType(), parameter, null));
            Then<ArgumentException>($"{typeof(ArgumentException)} should be thrown");
        }

        class InvalidConvertingDataSource : ISampleDataSource
        {
            IEnumerable ISampleDataSource.GetData()
            {
                yield return new { Description = "When a value type is invalid", Value = new object(), Parameter = TodoItemState.Active };
                yield return new { Description = "When a parameter type is invalid", Value = TodoItemState.Active, Parameter = new object() };
            }
        }

        class InvalidConvertingBackDataSource : ISampleDataSource
        {
            IEnumerable ISampleDataSource.GetData()
            {
                yield return new { Description = "When a value type is invalid", Value = new object(), Parameter = TodoItemState.Active };
                yield return new { Description = "When a parameter type is invalid", Value = true, Parameter = new object() };
            }
        }
    }
}
