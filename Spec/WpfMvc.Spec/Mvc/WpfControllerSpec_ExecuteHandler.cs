// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Executes handlers")]
    class WpfControllerSpec_ExecuteHandler : FixtureSteppable
    {
        bool LoadedEventHandled { get; set; }
        bool ExecutedEventHandled { get; set; }
        bool CanExecuteEventHandled { get; set; }

        [Example("Retrieves event handlers and executes them when an element is not attached")]
        void Ex01()
        {
            When("the Loaded event is raised using the EventHandlerBase", () =>
                WpfController.EventHandlersOf(new TestWpfControllers.TestWpfController { LoadedAssertionHandler = () => LoadedEventHandled = true })
                    .GetBy("Element")
                    .Raise("Loaded")
            );
            Then("the Loaded event should be handled", () => LoadedEventHandled);
        }

        [Example("Retrieves event handlers and executes them asynchronously when an element is not attached")]
        void Ex02()
        {
            When("the Loaded event is raised using the EventHandlerBase", async () =>
                await WpfController.EventHandlersOf(new TestWpfControllers.TestWpfControllerAsync { LoadedAssertionHandler = () => LoadedEventHandled = true })
                    .GetBy("Element")
                    .RaiseAsync("Loaded")
            );
            Then("the Loaded event should be handled", () => LoadedEventHandled);
        }

        [Example("Retrieves command handlers and executes them when an element is not attached")]
        void Ex03()
        {
            When("the Executed event is raised using the CommandHandlerBase", () =>
                WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfController { ExecutedAssertionHandler = () => ExecutedEventHandled = true })
                    .GetBy("TestCommand")
                    .With(TestWpfControllers.TestCommand)
                    .RaiseExecuted(new object())
            );
            Then("the Executed event should be handled", () => ExecutedEventHandled);

            When("the CanExecute event is raised using the CommandHandlerBase", () =>
                WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfController { CanExecuteAssertionHandler = () => CanExecuteEventHandled = true })
                    .GetBy("TestCommand")
                    .With(TestWpfControllers.TestCommand)
                    .RaiseCanExecute(new object())
            );
            Then("the Executed event should be handled", () => ExecutedEventHandled);
        }

        [Example("Retrieves command handlers and executes asynchronously them when an element is not attached")]
        void Ex04()
        {
            When("the Executed event is raised using the CommandHandlerBase", async () =>
                await WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfController { ExecutedAssertionHandler = () => ExecutedEventHandled = true })
                    .GetBy("TestCommand")
                    .With(TestWpfControllers.TestCommand)
                    .RaiseExecutedAsync(new object())
            );
            Then("the Executed event should be handled", () => ExecutedEventHandled);

            When("the CanExecute event is raised using the CommandHandlerBase", async () =>
                await WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfController { CanExecuteAssertionHandler = () => CanExecuteEventHandled = true })
                    .GetBy("TestCommand")
                    .With(TestWpfControllers.TestCommand)
                    .RaiseCanExecuteAsync(new object())
            );
            Then("the Executed event should be handled", () => ExecutedEventHandled);
        }
    }
}
