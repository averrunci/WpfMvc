// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using Charites.Windows.Samples.SimpleTodo.Contents;

namespace Charites.Windows.Samples.SimpleTodo.Converters;

[Specification("TodoItemStateToBooleanConverter Spec")]
class TodoItemStateToBooleanConverterSpec : FixtureSteppable
{
    TodoItemStateToBooleanConverter Converter { get; } = new();

    [Example("Converts a TodoItemState to a boolean value")]
    [Sample(TodoItemState.Completed, true, Description = "When a TodoItemState is Completed")]
    [Sample(TodoItemState.Active, false, Description = "When a TodoItemState is Active")]
    [Sample(TodoItemState.All, false, Description = "When a TodoItemState is All")]
    void Ex01(TodoItemState value, bool expected)
    {
        Expect($"the converted value should be {expected}", () => Equals(Converter.Convert(value, value.GetType(), null, null), expected));
    }

    [Example("Converts a boolean value back to a TodoItemState")]
    [Sample(true, TodoItemState.Completed, Description = "When a value is true")]
    [Sample(false, TodoItemState.Active, Description = "When a value is false")]
    void Ex02(bool value, TodoItemState expected)
    {
        Expect($"the converted value should be {expected}", () => Equals(Converter.ConvertBack(value, value.GetType(), null, null), expected));
    }

    [Example("Converts a value whose type is not TodoItemState")]
    void Ex03()
    {
        var value = new object();
        When("a value whose type is not TodoItemState is converted", () => Converter.Convert(value, value.GetType(), null, null));
        Then<ArgumentException>($"{typeof(ArgumentException)} should be thrown");
    }

    [Example("Converts back a value whose type is not boolean")]
    void Ex04()
    {
        var value = new object();
        When("a value whose type is not boolean is converted back", () => Converter.ConvertBack(value, value.GetType(), null, null));
        Then<ArgumentException>($"{typeof(ArgumentException)} should be thrown");
    }
}