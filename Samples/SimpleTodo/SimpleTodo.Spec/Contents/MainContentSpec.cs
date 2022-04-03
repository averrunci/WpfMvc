// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Samples.SimpleTodo.Contents;

[Specification("MainContent Spec")]
class MainContentSpec : FixtureSteppable
{
    MainContent MainContent { get; } = new();

    const string TodoContent1 = "Todo Item 1";
    const string TodoContent2 = "Todo Item 2";
    const string TodoContent3 = "Todo Item 3";

    void AddTodoContent(string content)
    {
        MainContent.TodoContent.Value = content;
        MainContent.AddCurrentTodoContent();
    }

    [Example("Initial state")]
    void Ex01()
    {
        Expect("the Items should be empty", () => !MainContent.TodoItems.Any());
        Expect("the TodoContent should be empty", () => MainContent.TodoContent.Value == string.Empty);
        Expect("the AllCompleted should be false", () => MainContent.AllCompleted.Value == false);
        Expect("the AllCompleted should be invisible", () => !MainContent.CanCompleteAllTodoItems.Value);
        Expect("the ItemsLeftMessage should be a string that expresses 0 items left", () => MainContent.ItemsLeftMessage.Value == "0 items left");
        Expect("the TodoItemDisplayState should be All", () => MainContent.TodoItemDisplayState.Value == TodoItemState.All);
    }

    [Example("Add one item")]
    void Ex02()
    {
        When("the content of the to-do is set", () => MainContent.TodoContent.Value = TodoContent1);
        When("to add the current content of the to-do", () => MainContent.AddCurrentTodoContent());
        Then("the items should have one item that has the given content of the to-do and is Active state", () =>
            MainContent.TodoItems.Count == 1 &&
            MainContent.TodoItems[0].Content.Value == TodoContent1 &&
            MainContent.TodoItems[0].State.Value == TodoItemState.Active
        );
        Then("the content of the to-do should be empty", () => MainContent.TodoContent.Value == string.Empty);
        Then("the AllCompleted should be visible", () => MainContent.CanCompleteAllTodoItems.Value);
        Then("the ItemsLeftMessage should be a string that expresses 1 item left", () => MainContent.ItemsLeftMessage.Value == "1 item left");
    }

    [Example("Adds two items")]
    void Ex03()
    {
        When("the content of the to-do is set", () => MainContent.TodoContent.Value = TodoContent1);
        When("to add the current content of the to-do", () => MainContent.AddCurrentTodoContent());
        When("the content of the to-do is set again", () => MainContent.TodoContent.Value = TodoContent2);
        When("to add the current content of the to-do", () => MainContent.AddCurrentTodoContent());
        Then("the items should be two items that have the given content of the to-do and are Active state", () =>
            MainContent.TodoItems.Count == 2 &&
            MainContent.TodoItems[0].Content.Value == TodoContent1 &&
            MainContent.TodoItems[0].State.Value == TodoItemState.Active &&
            MainContent.TodoItems[1].Content.Value == TodoContent2 &&
            MainContent.TodoItems[1].State.Value == TodoItemState.Active
        );
        Then("the content of the to-do should be empty", () => MainContent.TodoContent.Value = string.Empty);
        Then("the AllCompleted should be visible", () => MainContent.CanCompleteAllTodoItems.Value);
        Then("the ItemsLeftMessage should be a string that expresses 2 items left", () => MainContent.ItemsLeftMessage.Value = "2 items left");
    }

    [Example("Removes an item")]
    void Ex04()
    {
        When("to add three to-do items", () =>
        {
            AddTodoContent(TodoContent1);
            AddTodoContent(TodoContent2);
            AddTodoContent(TodoContent3);
        });
        When("to remove the item that is added secondly", () => MainContent.TodoItems[1].Remove());
        Then("the item should be removed", () =>
            MainContent.TodoItems.Count == 2 &&
            MainContent.TodoItems[0].Content.Value == TodoContent1 &&
            MainContent.TodoItems[0].State.Value == TodoItemState.Active &&
            MainContent.TodoItems[1].Content.Value == TodoContent3 &&
            MainContent.TodoItems[1].State.Value == TodoItemState.Active
        );
        Then("the AllCompleted should be visible", () => MainContent.CanCompleteAllTodoItems.Value);
        Then("the ItemsLeftMessage should be a string that expresses 2 items left", () => MainContent.ItemsLeftMessage.Value = "2 items left");
    }

    [Example("Removes the last item")]
    void Ex05()
    {
        When("to add a to-do items", () => AddTodoContent(TodoContent1));
        When("to remove the item", () => MainContent.TodoItems[0].Remove());
        Then("the item should be empty", () => !MainContent.TodoItems.Any());
        Then("the AllCompleted should be invisible", () => !MainContent.CanCompleteAllTodoItems.Value);
        Then("the ItemsLeftMessage should be a string that expresses 0 items left", () => MainContent.ItemsLeftMessage.Value = "0 items left");
    }

    [Example("Completes an item")]
    void Ex06()
    {
        When("to add a to-do items", () => AddTodoContent(TodoContent1));
        When("to complete the item", () => MainContent.TodoItems[0].Complete());
        Then("the ItemsLeftMessage should be a string that expresses 0 items left", () => MainContent.ItemsLeftMessage.Value = "0 items left");
    }

    [Example("Reverts an item that is completed")]
    void Ex07()
    {
        When("to add a to-do items", () => AddTodoContent(TodoContent1));
        When("to complete the item", () => MainContent.TodoItems[0].Complete());
        When("to revert the item", () => MainContent.TodoItems[0].Revert());
        Then("the ItemsLeftMessage should be a string that expresses 1 item left", () => MainContent.ItemsLeftMessage.Value = "1 item left");
    }

    [Example("Switches a filter")]
    void Ex08()
    {
        When("to add five items", () =>
        {
            AddTodoContent("Active Item 1");
            AddTodoContent("Completed Item 1");
            AddTodoContent("Active Item 2");
            AddTodoContent("Completed Item 2");
            AddTodoContent("Active Item 3");
        });
        When("to complete two items", () =>
        {
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[3].Complete();
        });
        When("to select All state", () => MainContent.TodoItemDisplayState.Value = TodoItemState.All);
        Then("the number of the to-do items should be 5", () => MainContent.TodoItems.Count == 5);

        When("to select Active state", () => MainContent.TodoItemDisplayState.Value = TodoItemState.Active);
        Then("the number of the to-do items should be 3", () => MainContent.TodoItems.Count == 3);

        When("to select Completed state", () => MainContent.TodoItemDisplayState.Value = TodoItemState.Completed);
        Then("the number of the to-do items should be 2", () => MainContent.TodoItems.Count == 2);
    }

    [Example("Items list is updated correctly when the state of the items is changed")]
    void Ex09()
    {
        Expect("the number of the to-do items should be 0", () => MainContent.TodoItems.Count == 0);
        When("to add a to-do item", () => AddTodoContent(TodoContent1));

        When("to select Active state", () => MainContent.TodoItemDisplayState.Value = TodoItemState.Active);
        Then("the number of the to-do items should be 1", () => MainContent.TodoItems.Count == 1);

        When("to complete the item", () => MainContent.TodoItems[0].Complete());
        Then("the number of the to-do items should be 0", () => MainContent.TodoItems.Count == 0);

        When("to select Completed state", () => MainContent.TodoItemDisplayState.Value = TodoItemState.Completed);
        Then("the number of the to-do items should be 1", () => MainContent.TodoItems.Count == 1);

        When("to revert the item", () => MainContent.TodoItems[0].Revert());
        Then("the number of the to-do items should be 0", () => MainContent.TodoItems.Count == 0);
    }

    [Example("Completes and reverts all items")]
    void Ex10()
    {
        When("to add three items", () =>
        {
            AddTodoContent(TodoContent1);
            AddTodoContent(TodoContent2);
            AddTodoContent(TodoContent3);
        });

        When("the AllCompleted is set to true", () => MainContent.AllCompleted.Value = true);
        Then("all items should be completed", () => MainContent.TodoItems.All(item => item.State.Value == TodoItemState.Completed));
        Then("the ItemsLeftMessage should be a string that expresses 0 items left", () => MainContent.ItemsLeftMessage.Value = "0 items left");

        When("the AllCompleted is set to false", () => MainContent.AllCompleted.Value = false);
        Then("all items should be reverted", () => MainContent.TodoItems.All(item => item.State.Value == TodoItemState.Active));
        Then("the ItemsLeftMessage should be a string that expresses 3 items left", () => MainContent.ItemsLeftMessage.Value = "3 items left");
    }

    [Example("Behavior of the AllCompleted")]
    void Ex11()
    {
        When("to add three items", () =>
        {
            AddTodoContent(TodoContent1);
            AddTodoContent(TodoContent2);
            AddTodoContent(TodoContent3);
        });
        When("to complete all items", () =>
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[2].Complete();
        });
        Then("the AllCompleted should be true", () => MainContent.AllCompleted.Value == true);

        When("to revert a first item", () => MainContent.TodoItems[0].Revert());
        Then("the AllCompleted should be null", () => MainContent.AllCompleted.Value == null);

        When("to revert remained items", () =>
        {
            MainContent.TodoItems[1].Revert();
            MainContent.TodoItems[2].Revert();
        });
        Then("the AllCompleted should be false", () => MainContent.AllCompleted.Value == false);

        When("to complete all items", () =>
        {
            MainContent.TodoItems[0].Complete();
            MainContent.TodoItems[1].Complete();
            MainContent.TodoItems[2].Complete();
        });
        Then("the AllCompleted should be true", () => MainContent.AllCompleted.Value == true);

        When("to add a to-do item", () => AddTodoContent("Todo Item 4"));
        Then("the AllCompleted should be null", () => MainContent.AllCompleted.Value == null);
    }
}