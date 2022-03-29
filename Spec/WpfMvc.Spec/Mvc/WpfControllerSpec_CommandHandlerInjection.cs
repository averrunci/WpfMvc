// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using Carna;

namespace Charites.Windows.Mvc;

[Context("Command handler injection")]
class WpfControllerSpec_CommandHandlerInjection : FixtureSteppable
{
    object DataContext { get; set; } = default!;
    Button TestButton1 { get; set; } = default!;
    Button TestButton2 { get; set; } = default!;
    TestElement Element { get; set; } = default!;

    static object Parameter { get; } = new();

    static bool ExecutedEventHandled1 { get; set; }
    static bool CanExecuteEventHandled1 { get; set; }
    static bool ExecutedEventHandled2 { get; set; }
    static bool CanExecuteEventHandled2 { get; set; }
    static Action<ExecutedRoutedEventArgs> OneArgumentExecutedAssertionHandler1 { get; } = e => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
    static Action<CanExecuteRoutedEventArgs> OneArgumentCanExecuteAssertionHandler1 { get; } = e => CanExecuteEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static Action<ExecutedRoutedEventArgs> OneArgumentExecutedAssertionHandler2 { get; } = e => ExecutedEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter);
    static Action<CanExecuteRoutedEventArgs> OneArgumentCanExecuteAssertionHandler2 { get; } = e => CanExecuteEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static ExecutedRoutedEventHandler ExecutedAssertionHandler1 { get; } = (_, e) => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
    static CanExecuteRoutedEventHandler CanExecuteAssertionHandler1 { get; } = (_, e) => CanExecuteEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static ExecutedRoutedEventHandler ExecutedAssertionHandler2 { get; } = (_, e) => ExecutedEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter);
    static CanExecuteRoutedEventHandler CanExecuteAssertionHandler2 { get; } = (_, e) => CanExecuteEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;

    [Example("When a command handler of the Executed event has one argument")]
    [Sample(Source = typeof(OneArgumentExecutedOnlySampleDataSource))]
    void Ex01(object controller)
    {
        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed and CanExecute event have one argument")]
    [Sample(Source = typeof(OneArgumentExecutedAndCanExecuteSampleDataSource))]
    void Ex02(object controller)
    {
        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
        Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);
    }

    [Example("When some command handlers of the Executed and CanExecute event have one argument")]
    [Sample(Source = typeof(OneArgumentExecutedAndCanExecuteSomeSampleDataSource))]
    void Ex03(object controller)
    {
        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
        Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
        Given("an element that has the test buttons", () => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
        Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);

        When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled2);
        Then("the CanExecuted event should be handled", () => CanExecuteEventHandled2);
    }

    [Example("When a command handler of the Executed event is the ExecutedRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedOnlySampleDataSource))]
    void Ex04(object controller)
    {
        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedAndCanExecuteSampleDataSource))]
    void Ex05(object controller)
    {
        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
        Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);
    }

    [Example("When some command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedAndCanExecuteSomeSampleDataSource))]
    void Ex06(object controller)
    {
        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
        Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
        Given("an element that has the test buttons", () => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
        Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);

        When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled2);
        Then("the CanExecuted event should be handled", () => CanExecuteEventHandled2);
    }

    class OneArgumentExecutedOnlySampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedOnlyHandlerController(OneArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedOnlyHandlerController(OneArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedOnlyHandlerController(OneArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentExecutedOnlyHandlerController(OneArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.OneArgumentExecutedOnlyHandlerController(OneArgumentExecutedAssertionHandler1) };
        }
    }

    class OneArgumentExecutedAndCanExecuteSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1) };
        }
    }

    class OneArgumentExecutedAndCanExecuteSomeSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentExecutedAssertionHandler2, OneArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentExecutedAssertionHandler2, OneArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentExecutedAssertionHandler2, OneArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentExecutedAssertionHandler2, OneArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.OneArgumentExecutedAndCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentExecutedAssertionHandler2, OneArgumentCanExecuteAssertionHandler2) };
        }
    }

    class ExecutedOnlySampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.ExecutedOnlyHandlerController(ExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.ExecutedOnlyHandlerController(ExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.ExecutedOnlyHandlerController(ExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.ExecutedOnlyHandlerController(ExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.ExecutedOnlyHandlerController(ExecutedAssertionHandler1) };
        }
    }

    class ExecutedAndCanExecuteSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1) };
        }
    }

    class ExecutedAndCanExecuteSomeSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, ExecutedAssertionHandler2, CanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, ExecutedAssertionHandler2, CanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, ExecutedAssertionHandler2, CanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, ExecutedAssertionHandler2, CanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.ExecutedAndCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, ExecutedAssertionHandler2, CanExecuteAssertionHandler2) };
        }
    }
}