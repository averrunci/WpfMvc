// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Fievus.Windows.Samples.SimpleTodo.Contents;

using NUnit.Framework;

using NSubstitute;

namespace Fievus.Windows.Samples.SimpleTodo.Test.Content
{
    [TestFixture]
    public class TodoItemTest
    {
        private TodoItem TodoItem { get; set; }

        private const string initialContent = "Todo Item";
        private const string modifiedContent = "Todo Item Modified";

        [SetUp]
        public void SetUp()
        {
            TodoItem = new TodoItem(initialContent);
        }

        [Test]
        public void ShouldEditContent()
        {
            Assert.That(TodoItem.Editing.Value, Is.False);

            TodoItem.StartEdit();
            Assert.That(TodoItem.Editing.Value, Is.True);
            Assert.That(TodoItem.EditContent.Value, Is.EqualTo(initialContent));

            TodoItem.EditContent.Value = modifiedContent;
            TodoItem.CompleteEdit();

            Assert.That(TodoItem.Editing.Value, Is.False);
            Assert.That(TodoItem.Content.Value, Is.EqualTo(modifiedContent));
        }

        [Test]
        public void ShouldCancelContentEdit()
        {
            Assert.That(TodoItem.Editing.Value, Is.False);

            TodoItem.StartEdit();
            Assert.That(TodoItem.Editing.Value, Is.True);
            Assert.That(TodoItem.EditContent.Value, Is.EqualTo(initialContent));

            TodoItem.EditContent.Value = modifiedContent;
            TodoItem.CancelEdit();

            Assert.That(TodoItem.Editing.Value, Is.False);
            Assert.That(TodoItem.Content.Value, Is.EqualTo(initialContent));
        }

        [Test]
        public void ShouldRequestToRemove()
        {
            var handler = Substitute.For<EventHandler>();
            TodoItem.RemoveRequested += handler;

            TodoItem.Remove();

            handler.Received().Invoke(TodoItem, EventArgs.Empty);
        }

        [Test]
        public void ShouldCompleteTodoItem()
        {
            TodoItem.Complete();

            Assert.That(TodoItem.State.Value, Is.EqualTo(TodoItemState.Completed));
        }

        [Test]
        public void ShouldRevertCompleted()
        {
            TodoItem.Revert();

            Assert.That(TodoItem.State.Value, Is.EqualTo(TodoItemState.Active));
        }
    }
}
