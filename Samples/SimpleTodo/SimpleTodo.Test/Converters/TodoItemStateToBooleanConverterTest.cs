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
    public class TodoItemStateToBooleanConverterTest
    {
        private TodoItemStateToBooleanConverter Converter { get; set; }

        [SetUp]
        public void SetUp()
        {
            Converter = new TodoItemStateToBooleanConverter();
        }

        [Test]
        public void ShouldConvertToTrueWhenValueIsEqualToCompleted()
        {
            var value = TodoItemState.Completed;

            Assert.That(Converter.Convert(value, value.GetType(), null, null), Is.True);
        }

        [Test]
        public void ShouldConvertToFalseWhenValueIsEqualToActive()
        {
            var value = TodoItemState.Active;

            Assert.That(Converter.Convert(value, value.GetType(), null, null), Is.False);
        }

        [Test]
        public void ShouldConvertBackToCompletedWhenValueIsTrue()
        {
            var value = true;

            Assert.That(Converter.ConvertBack(value, value.GetType(), null, null), Is.EqualTo(TodoItemState.Completed));
        }

        [Test]
        public void ShouldConvertBackToActiveWhenValueIsFalse()
        {
            var value = false;

            Assert.That(Converter.ConvertBack(value, value.GetType(), null, null), Is.EqualTo(TodoItemState.Active));
        }

        [Test]
        public void ShouldThrowExceptionWhenConvertingWithValueThatIsNotTypeOfTodoItemState()
        {
            var value = new object();

            Assert.Throws<ArgumentException>(() => Converter.Convert(value, value.GetType(), null, null));
        }

        [Test]
        public void ShouldThrowExceptionWhenConvertingBackWithValueThatIsNotTypeOfBoolean()
        {
            var value = new object();

            Assert.Throws<ArgumentException>(() => Converter.ConvertBack(value, value.GetType(), null, null));
        }
    }
}
