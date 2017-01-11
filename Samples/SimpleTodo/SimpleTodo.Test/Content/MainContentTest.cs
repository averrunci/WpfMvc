// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Linq;

using NUnit.Framework;

using Fievus.Windows.Samples.SimpleTodo.Contents;

namespace Fievus.Windows.Samples.SimpleTodo.Test.Content.MainContentTest
{
    [TestFixture]
    public class Initial
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();
        }

        [Test]
        public void ItemsShouldBeEmpty()
        {
            Assert.That(MainContent.TodoItems, Is.Empty);
        }

        [Test]
        public void TodoContentShouldBeEmpty()
        {
            Assert.That(MainContent.TodoContent.Value, Is.Empty);
        }

        [Test]
        public void AllCompletedShouldBeFalse()
        {
            Assert.That(MainContent.AllCompleted.Value, Is.False);
        }

        [Test]
        public void AllCompletedShouldBeInvisible()
        {
            Assert.That(MainContent.CanCompleteAllTodoItems.Value, Is.False);
        }

        [Test]
        public void ItemsLeftMessageShouldHaveZeroItemsLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("0 items left"));
        }

        [Test]
        public void TodoItemDisplayStateShouldBeAll()
        {
            Assert.That(MainContent.TodoItemDisplayState.Value, Is.EqualTo(TodoItemState.All));
        }
    }

    [TestFixture]
    public class AddOneItem
    {
        private MainContent MainContent { get; set; }

        private const string todoContent = "Todo Item";

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();
            MainContent.TodoContent.Value = todoContent;
            MainContent.AddCurrentTodoContent();
        }

        [Test]
        public void ItemsShouldHaveOneItem()
        {
            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(1));
            Assert.That(MainContent.TodoItems[0].Content.Value, Is.EqualTo(todoContent));
            Assert.That(MainContent.TodoItems[0].State.Value, Is.EqualTo(TodoItemState.Active));
        }

        [Test]
        public void TodoContentShouldBeEmpty()
        {
            Assert.That(MainContent.TodoContent.Value, Is.Empty);
        }

        [Test]
        public void AllCompletedShouldBeVisible()
        {
            Assert.That(MainContent.CanCompleteAllTodoItems.Value, Is.True);
        }

        [Test]
        public void ItemsLeftMessageShouldHaveOneItemLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("1 item left"));
        }
    }

    [TestFixture]
    public class AddTwoItems
    {
        private MainContent MainContent { get; set; }

        private const string todoContent1 = "Todo Item 1";
        private const string todoContent2 = "Todo Item 2";

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = todoContent1;
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = todoContent2;
            MainContent.AddCurrentTodoContent();
        }

        [Test]
        public void ItemsShouldHaveTowItems()
        {
            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(2));
            Assert.That(MainContent.TodoItems[0].Content.Value, Is.EqualTo(todoContent1));
            Assert.That(MainContent.TodoItems[0].State.Value, Is.EqualTo(TodoItemState.Active));
            Assert.That(MainContent.TodoItems[1].Content.Value, Is.EqualTo(todoContent2));
            Assert.That(MainContent.TodoItems[1].State.Value, Is.EqualTo(TodoItemState.Active));
        }

        [Test]
        public void TodoContentShouldBeEmpty()
        {
            Assert.That(MainContent.TodoContent.Value, Is.Empty);
        }

        [Test]
        public void AllCompletedShouldBeVisible()
        {
            Assert.That(MainContent.CanCompleteAllTodoItems.Value, Is.True);
        }

        [Test]
        public void ItemsLeftMessageShouldHaveTwoItemsLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("2 items left"));
        }
    }

    [TestFixture]
    public class RemoveItem
    {
        private MainContent MainContent { get; set; }

        private const string todoContent1 = "Todo Item 1";
        private const string todoContent2 = "Todo Item 2";
        private const string todoContent3 = "Todo Item 3";

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = todoContent1;
            MainContent.AddCurrentTodoContent();
            MainContent.TodoContent.Value = todoContent2;
            MainContent.AddCurrentTodoContent();
            MainContent.TodoContent.Value = todoContent3;
            MainContent.AddCurrentTodoContent();

            MainContent.TodoItems[1].Remove();
        }

        [Test]
        public void ItemShouldBeRemoved()
        {
            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(2));
            Assert.That(MainContent.TodoItems[0].Content.Value, Is.EqualTo(todoContent1));
            Assert.That(MainContent.TodoItems[0].State.Value, Is.EqualTo(TodoItemState.Active));
            Assert.That(MainContent.TodoItems[1].Content.Value, Is.EqualTo(todoContent3));
            Assert.That(MainContent.TodoItems[1].State.Value, Is.EqualTo(TodoItemState.Active));
        }

        [Test]
        public void AllCompletedShouldBeVisible()
        {
            Assert.That(MainContent.CanCompleteAllTodoItems.Value, Is.True);
        }

        [Test]
        public void ItemsLeftMessageShouldBeUpdated()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("2 items left"));
        }
    }

    [TestFixture]
    public class RemoveLastItem
    {
        private MainContent MainContent { get; set; }

        private const string todoContent = "Todo Item";

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = todoContent;
            MainContent.AddCurrentTodoContent();

            MainContent.TodoItems[0].Remove();
        }

        [Test]
        public void ItemShouldBeEmpty()
        {
            Assert.That(MainContent.TodoItems, Is.Empty);
        }

        [Test]
        public void AllCompletedShouldBeInvisible()
        {
            Assert.That(MainContent.CanCompleteAllTodoItems.Value, Is.False);
        }

        [Test]
        public void ItemsLeftMessageShouldBeZeroItemsLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("0 items left"));
        }
    }

    [TestFixture]
    public class CompleteItem
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Todo Item";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoItems[0].Complete();
        }

        [Test]
        public void ItemsLeftMessageShouldHaveZeroItemsLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("0 items left"));
        }
    }

    [TestFixture]
    public class RevertCompletedItem
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Todo Item";
            MainContent.AddCurrentTodoContent();
            MainContent.TodoItems[0].Complete();

            MainContent.TodoItems[0].Revert();
        }

        [Test]
        public void ItemsLeftMessageShouldHaveOneItemLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("1 item left"));
        }
    }

    [TestFixture]
    public class SwitchFilter
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Active Item 1";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Completed Item 1";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Active Item 2";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Completed Item 1";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Active Item 3";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[3].Complete();
        }

        [Test]
        public void ShouldShowAllItemsWhenAllIsSelected()
        {
            MainContent.TodoItemDisplayState.Value = TodoItemState.All;

            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(5));
        }

        [Test]
        public void ShouldShowOnlyActiveItemsWhenActiveIsSelected()
        {
            MainContent.TodoItemDisplayState.Value = TodoItemState.Active;

            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(3));
        }

        [Test]
        public void ShouldShowOnlyCompletedItemsWhenCompletedIsSelected()
        {
            MainContent.TodoItemDisplayState.Value = TodoItemState.Completed;

            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(2));
        }
    }

    [TestFixture]
    public class ItemsListUpdatedCorrectlyOnCompletionChanged
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Todo Item";
            MainContent.AddCurrentTodoContent();
        }

        [Test]
        public void ShouldApplyFilterWhenTodoItemIsCompleted()
        {
            MainContent.TodoItemDisplayState.Value = TodoItemState.Active;

            MainContent.TodoItems[0].Complete();

            Assert.That(MainContent.TodoItems, Is.Empty);
        }

        [Test]
        public void ShouldApplyFilterWhenTodoItemIsReverted()
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItemDisplayState.Value = TodoItemState.Completed;

            MainContent.TodoItems[0].Revert();

            Assert.That(MainContent.TodoItems, Is.Empty);
        }

        [Test]
        public void ShouldApplyFilterWhenTodoItemIsAdded()
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItemDisplayState.Value = TodoItemState.Completed;

            MainContent.TodoContent.Value = "New Todo Item";
            MainContent.AddCurrentTodoContent();

            Assert.That(MainContent.TodoItems, Has.Count.EqualTo(1));
        }
    }

    [TestFixture]
    public class CompleteAllItems
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Todo Item 1";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Todo Item 2";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Todo Item 3";
            MainContent.AddCurrentTodoContent();

            MainContent.AllCompleted.Value = true;
        }

        [Test]
        public void AllItemsAreCompleted()
        {
            Assert.That(MainContent.TodoItems.All(i => i.State.Value == TodoItemState.Completed), Is.True);
        }

        [Test]
        public void ItemsLeftMessageShouldHaveZeroItemsLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("0 items left"));
        }
    }

    [TestFixture]
    public class RevertAllItems
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Todo Item 1";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Todo Item 2";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Todo Item 3";
            MainContent.AddCurrentTodoContent();

            MainContent.AllCompleted.Value = true;
            MainContent.AllCompleted.Value = false;
        }

        [Test]
        public void AllItemsAreActive()
        {
            Assert.That(MainContent.TodoItems.All(i => i.State.Value == TodoItemState.Active), Is.True);
        }

        [Test]
        public void ItemsLeftMessageShouldHaveThreeItemsLeft()
        {
            Assert.That(MainContent.ItemsLeftMessage.Value, Is.EqualTo("3 items left"));
        }
    }

    [TestFixture]
    public class AllCompletedBehavior
    {
        private MainContent MainContent { get; set; }

        [SetUp]
        public void SetUp()
        {
            MainContent = new MainContent();

            MainContent.TodoContent.Value = "Todo Item 1";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Todo Item 2";
            MainContent.AddCurrentTodoContent();

            MainContent.TodoContent.Value = "Todo Item 3";
            MainContent.AddCurrentTodoContent();
        }

        [Test]
        public void AllCompletedShouldBeTrueWhenLastItemIsCompleted()
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[2].Complete();

            Assert.That(MainContent.AllCompleted.Value, Is.True);
        }

        [Test]
        public void AllCompletedShouldBeNullWhenItemIsRevertedFromAllItemsCompleted()
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[2].Complete();

            Assert.That(MainContent.AllCompleted.Value, Is.True);

            MainContent.TodoItems[0].Revert();

            Assert.That(MainContent.AllCompleted.Value, Is.Null);
        }

        [Test]
        public void AllCompletedShouldBeFalseWhenAllItemsIsActive()
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[2].Complete();

            Assert.That(MainContent.AllCompleted.Value, Is.True);

            MainContent.TodoItems[0].Revert();
            MainContent.TodoItems[1].Revert();
            MainContent.TodoItems[2].Revert();

            Assert.That(MainContent.AllCompleted.Value, Is.False);
        }

        [Test]
        public void AllCompletedShouldBeNullWhenNewItemIsAddedFromAllItemsCompleted()
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[2].Complete();

            Assert.That(MainContent.AllCompleted.Value, Is.True);

            MainContent.TodoContent.Value = "Todo Item 4";
            MainContent.AddCurrentTodoContent();

            Assert.That(MainContent.AllCompleted.Value, Is.Null);
        }
    }
}
