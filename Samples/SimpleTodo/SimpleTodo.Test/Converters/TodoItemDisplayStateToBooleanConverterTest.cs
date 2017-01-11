// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Fievus.Windows.Samples.SimpleTodo.Contents;
using Fievus.Windows.Samples.SimpleTodo.Converters;

using NUnit.Framework;

namespace Fievus.Windows.Samples.SimpleTodo.Test.Converters
{
    [TestFixture]
    public class TodoItemDisplayStateToBooleanConverterTest
    {
        private TodoItemDisplayStateToBooleanConverter Converter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Converter = new TodoItemDisplayStateToBooleanConverter();
        }

        [Test]
        public void ShouldConvertToTrueWhenValueIsEqualToParameter()
        {
            var value = TodoItemState.All;
            var parameter = TodoItemState.All;

            Assert.That(Converter.Convert(value, value.GetType(), parameter, null), Is.True);
        }

        [Test]
        public void ShouldConvertToFalseWhenValueIsEqualToParameter()
        {
            var value = TodoItemState.All;
            var parameter = TodoItemState.Active;

            Assert.That(Converter.Convert(value, value.GetType(), parameter, null), Is.False);
        }

        [Test]
        public void ShouldConvertBackToSpecifiedParameterWhenValueIsTrue()
        {
            var value = true;
            var parameter = TodoItemState.Completed;

            Assert.That(Converter.ConvertBack(value, value.GetType(), parameter, null), Is.EqualTo(TodoItemState.Completed));
        }

        [Test]
        public void ShouldConvertBackToAllStateWhenValueIsFalse()
        {
            var value = false;
            var parameter = TodoItemState.Active;

            Assert.That(Converter.ConvertBack(value, value.GetType(), parameter, null), Is.EqualTo(TodoItemState.All));
        }

        [Test]
        public void ShouldThrowExceptionWhenConvertingWithValueThatIsNotTypeOfTodoItemState()
        {
            var value = new object();
            var parameter = TodoItemState.All;

            Assert.Throws<ArgumentException>(() => Converter.Convert(value, value.GetType(), parameter, null));
        }

        [Test]
        public void ShouldThrowExceptionWhenConvertingWithParameterThatIsNotTypeOfTodoItemState()
        {
            var value = TodoItemState.All;
            var parameter = new object();

            Assert.Throws<ArgumentException>(() => Converter.Convert(value, value.GetType(), parameter, null));
        }

        [Test]
        public void ShouldThrowExceptionWhenConvertingBackWithValueThatIsNotTypeOfBoolean()
        {
            var value = new object();
            var parameter = TodoItemState.All;

            Assert.Throws<ArgumentException>(() => Converter.ConvertBack(value, value.GetType(), parameter, null));
        }

        [Test]
        public void ShouldThrowExceptionWhenConvertingBackWithParameterThatIsNotTypeOfBoolean()
        {
            var value = true;
            var parameter = new object();

            Assert.Throws<ArgumentException>(() => Converter.ConvertBack(value, value.GetType(), parameter, null));
        }
    }
}
