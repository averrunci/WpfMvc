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
    bool ExecutedEventAttributedArgumentsHandled { get; set; }
    bool CanExecuteEventHandled { get; set; }
    bool CanExecuteEventAttributedArgumentsHandled { get; set; }
    bool PreviewExecutedEventHandled { get; set; }
    bool PreviewExecutedEventAttributedArgumentsHandled { get; set; }
    bool PreviewCanExecuteEventHandled { get; set; }
    bool PreviewCanExecuteEventAttributedArgumentsHandled { get; set; }

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

        When("the PreviewExecuted event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfController { PreviewExecutedAssertionHandler = () => PreviewExecutedEventHandled = true })
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .RaisePreviewExecuted(new object())
        );
        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);

        When("the PreviewCanExecute event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfController { PreviewCanExecuteAssertionHandler = () => PreviewCanExecuteEventHandled = true })
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .RaisePreviewCanExecute(new object())
        );
        Then("the PreviewCanExecute event should be handled", () => CanExecuteEventHandled);
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

        When("the PreviewExecuted event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfControllerAsync { PreviewExecutedAssertionHandler = () => PreviewExecutedEventHandled = true })
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .RaisePreviewExecutedAsync(new object())
        );
        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);

        When("the PreviewCanExecute event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.TestWpfControllerAsync { PreviewCanExecuteAssertionHandler = () => PreviewCanExecuteEventHandled = true })
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .RaisePreviewCanExecuteAsync(new object())
        );
        Then("the PreviewCanExecute event should be handled", () => PreviewCanExecuteEventHandled);
    }

    [Example("Retrieves command handlers that have attributed parameters and executes them when an element is not attached")]
    void Ex05()
    {
        var dependency1 = new TestWpfControllers.Dependency1();
        var dependency2 = new TestWpfControllers.Dependency2();
        var dependency3 = new TestWpfControllers.Dependency3();
        var element = new TestElement { Name = "element" };
        var dataContext = new TestDataContexts.TestDataContext();
        When("the Executed event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersController
                    {
                        ExecutedAssertionHandler = () => ExecutedEventHandled = true ,
                        ExecutedAttributedArgumentsHandler = (d1, d2, d3, e, d) => ExecutedEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
                    }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaiseExecuted(new object())
        );
        Then("the Executed event should be handled", () => ExecutedEventHandled);
        Then("the attributed arguments should be injected", () => ExecutedEventAttributedArgumentsHandled);

        When("the CanExecute event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersController
                    {
                        CanExecuteAssertionHandler = () => CanExecuteEventHandled = true,
                        CanExecuteAttributedArgumentsHandler = (d1, d2, d3, e, d) => CanExecuteEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaiseCanExecute(new object())
        );
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled);
        Then("the attributed arguments should be injected", () => CanExecuteEventAttributedArgumentsHandled);

        When("the PreviewExecuted event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersController
                    {
                        PreviewExecutedAssertionHandler = () => PreviewExecutedEventHandled = true,
                        PreviewExecutedAttributedArgumentsHandler = (d1, d2, d3, e, d) => PreviewExecutedEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaisePreviewExecuted(new object())
        );
        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the attributed arguments should be injected", () => PreviewExecutedEventAttributedArgumentsHandled);

        When("the PreviewCanExecute event is raised using the CommandHandlerBase", () =>
            WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersController
                    {
                        PreviewCanExecuteAssertionHandler = () => PreviewCanExecuteEventHandled = true,
                        PreviewCanExecuteAttributedArgumentsHandler = (d1, d2, d3, e, d) => PreviewCanExecuteEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaisePreviewCanExecute(new object())
        );
        Then("the PreviewCanExecute event should be handled", () => PreviewCanExecuteEventHandled);
        Then("the attributed arguments should be injected", () => PreviewCanExecuteEventAttributedArgumentsHandled);
    }

    [Example("Retrieves command handlers that have attached parameters and executes asynchronously them when an element is not attached")]
    void Ex06()
    {
        var dependency1 = new TestWpfControllers.Dependency1();
        var dependency2 = new TestWpfControllers.Dependency2();
        var dependency3 = new TestWpfControllers.Dependency3();
        var element = new TestElement { Name = "Element" };
        var dataContext = new TestDataContexts.TestDataContext();
        When("the Executed event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersControllerAsync
                    {
                        ExecutedAssertionHandler = () => ExecutedEventHandled = true,
                        ExecutedAttributedArgumentsHandler = (d1, d2, d3, e, d) => ExecutedEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaiseExecutedAsync(new object())
        );
        Then("the Executed event should be handled", () => ExecutedEventHandled);
        Then("the attributed arguments should be injected", () => ExecutedEventAttributedArgumentsHandled);

        When("the CanExecute event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersControllerAsync
                    {
                        CanExecuteAssertionHandler = () => CanExecuteEventHandled = true,
                        CanExecuteAttributedArgumentsHandler = (d1, d2, d3, e, d) => CanExecuteEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaiseCanExecuteAsync(new object())
        );
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled);
        Then("the attributed arguments should be injected", () => CanExecuteEventAttributedArgumentsHandled);

        When("the PreviewExecuted event is raised using the CommandHandlerBase", async () =>
           await WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersControllerAsync
                   {
                       PreviewExecutedAssertionHandler = () => PreviewExecutedEventHandled = true,
                       PreviewExecutedAttributedArgumentsHandler = (d1, d2, d3, e, d) => PreviewExecutedEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
           }
               )
               .GetBy("TestCommand")
               .With(TestWpfControllers.TestCommand)
               .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
               .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
               .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
               .ResolveFromElement(element.Name, element)
               .ResolveFromDataContext(dataContext)
               .RaisePreviewExecutedAsync(new object())
       );
        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the attributed arguments should be injected", () => PreviewExecutedEventAttributedArgumentsHandled);

        When("the PreviewCanExecute event is raised using the CommandHandlerBase", async () =>
            await WpfController.CommandHandlersOf(new TestWpfControllers.CommandHandlerWithAttributedParametersControllerAsync
                    {
                        PreviewCanExecuteAssertionHandler = () => PreviewCanExecuteEventHandled = true,
                        PreviewCanExecuteAttributedArgumentsHandler = (d1, d2, d3, e, d) => PreviewCanExecuteEventAttributedArgumentsHandled = d1 == dependency1 && d2 == dependency2 && d3 == dependency3 && e.Name == element.Name && d == dataContext
            }
                )
                .GetBy("TestCommand")
                .With(TestWpfControllers.TestCommand)
                .ResolveFromDI<TestWpfControllers.IDependency1>(() => dependency1)
                .ResolveFromDI<TestWpfControllers.IDependency2>(() => dependency2)
                .ResolveFromDI<TestWpfControllers.IDependency3>(() => dependency3)
                .ResolveFromElement(element.Name, element)
                .ResolveFromDataContext(dataContext)
                .RaisePreviewCanExecuteAsync(new object())
        );
        Then("the PreviewCanExecute event should be handled", () => PreviewCanExecuteEventHandled);
        Then("the attributed arguments should be injected", () => PreviewCanExecuteEventAttributedArgumentsHandled);
    }
}