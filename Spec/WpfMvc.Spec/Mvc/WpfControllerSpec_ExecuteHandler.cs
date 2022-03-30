// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc;

[Context("Executes handlers")]
class WpfControllerSpec_ExecuteHandler : FixtureSteppable
{
    bool LoadedEventHandled { get; set; }
    bool ExecutedEventHandled { get; set; }
    bool ExecutedEventDependencyArgumentsHandled { get; set; }
    bool CanExecuteEventHandled { get; set; }
    bool CanExecuteEventDependencyArgumentsHandled { get; set; }

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
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled);
    }

    [Example("Retrieves command handlers and executes asynchronously them when an element is not attached")]
    void Ex04()
    {
        When("the Executed event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfControllerAsync { ExecutedAssertionHandler = () => ExecutedEventHandled = true })
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .RaiseExecutedAsync(new object())
        );
        Then("the Executed event should be handled", () => ExecutedEventHandled);

        When("the CanExecute event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfControllerAsync { CanExecuteAssertionHandler = () => CanExecuteEventHandled = true })
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .RaiseCanExecuteAsync(new object())
        );
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled);
    }

    [Example("Retrieves command handlers that have dependency parameters and executes them when an element is not attached")]
    void Ex05()
    {
        var dependency1 = new TestWpfControllers.Dependency1();
        var dependency2 = new TestWpfControllers.Dependency2();
        var dependency3 = new TestWpfControllers.Dependency3();
        When("the Executed event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithDependencyParametersController
                    {
                        ExecutedAssertionHandler = () => ExecutedEventHandled = true ,
                        ExecutedDependencyArgumentsHandler = (d1, d2, d3) => ExecutedEventDependencyArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3
                    }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .Resolve<TestWpfControllers.IDependency1>(() => dependency1)
                .Resolve<TestWpfControllers.IDependency2>(() => dependency2)
                .Resolve<TestWpfControllers.IDependency3>(() => dependency3)
                .RaiseExecuted(new object())
        );
        Then("the Executed event should be handled", () => ExecutedEventHandled);
        Then("the dependency arguments should be injected", () => ExecutedEventDependencyArgumentsHandled);

        When("the CanExecute event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithDependencyParametersController
                    {
                        CanExecuteAssertionHandler = () => CanExecuteEventHandled = true,
                        CanExecuteDependencyArgumentsHandler = (d1, d2, d3) => CanExecuteEventDependencyArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .Resolve<TestWpfControllers.IDependency1>(() => dependency1)
                .Resolve<TestWpfControllers.IDependency2>(() => dependency2)
                .Resolve<TestWpfControllers.IDependency3>(() => dependency3)
                .RaiseCanExecute(new object())
        );
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled);
        Then("the dependency arguments should be injected", () => CanExecuteEventDependencyArgumentsHandled);
    }

    [Example("Retrieves command handlers that have dependency parameters and executes asynchronously them when an element is not attached")]
    void Ex06()
    {
        var dependency1 = new TestWpfControllers.Dependency1();
        var dependency2 = new TestWpfControllers.Dependency2();
        var dependency3 = new TestWpfControllers.Dependency3();
        When("the Executed event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithDependencyParametersControllerAsync
                    {
                        ExecutedAssertionHandler = () => ExecutedEventHandled = true,
                        ExecutedDependencyArgumentsHandler = (d1, d2, d3) => ExecutedEventDependencyArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3
                    }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .Resolve<TestWpfControllers.IDependency1>(() => dependency1)
                .Resolve<TestWpfControllers.IDependency2>(() => dependency2)
                .Resolve<TestWpfControllers.IDependency3>(() => dependency3)
                .RaiseExecutedAsync(new object())
        );
        Then("the Executed event should be handled", () => ExecutedEventHandled);
        Then("the dependency arguments should be injected", () => ExecutedEventDependencyArgumentsHandled);

        When("the CanExecute event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithDependencyParametersControllerAsync
            {
                        CanExecuteAssertionHandler = () => CanExecuteEventHandled = true,
                        CanExecuteDependencyArgumentsHandler = (d1, d2, d3) => CanExecuteEventDependencyArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3
                    }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .Resolve<TestWpfControllers.IDependency1>(() => dependency1)
                .Resolve<TestWpfControllers.IDependency2>(() => dependency2)
                .Resolve<TestWpfControllers.IDependency3>(() => dependency3)
                .RaiseCanExecuteAsync(new object())
        );
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled);
        Then("the dependency arguments should be injected", () => CanExecuteEventDependencyArgumentsHandled);
    }
}