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
    static bool PreviewExecutedEventHandled { get; set; }
    static bool PreviewCanExecuteEventHandled { get; set; }
    static Action NoArgumentExecutedAssertionHandler1 { get; } = () => ExecutedEventHandled1 = true;
    static Action NoArgumentCanExecuteAssertionHandler1 { get; } = () => CanExecuteEventHandled1 = true;
    static Action NoArgumentExecutedAssertionHandler2 { get; } = () => ExecutedEventHandled2 = true;
    static Action NoArgumentCanExecuteAssertionHandler2 { get; } = () => CanExecuteEventHandled2 = true;
    static Action NoArgumentPreviewExecutedAssertionHandler { get; } = () => PreviewExecutedEventHandled = true;
    static Action NoArgumentPreviewCanExecuteAssertionHandler { get; } = () => PreviewCanExecuteEventHandled = true;
    static Action<ExecutedRoutedEventArgs> OneArgumentExecutedAssertionHandler1 { get; } = e => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
    static Action<CanExecuteRoutedEventArgs> OneArgumentCanExecuteAssertionHandler1 { get; } = e => CanExecuteEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static Action<ExecutedRoutedEventArgs> OneArgumentExecutedAssertionHandler2 { get; } = e => ExecutedEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter);
    static Action<CanExecuteRoutedEventArgs> OneArgumentCanExecuteAssertionHandler2 { get; } = e => CanExecuteEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static Action<ExecutedRoutedEventArgs> OneArgumentPreviewExecutedAssertionHandler { get; } = e => PreviewExecutedEventHandled = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
    static Action<CanExecuteRoutedEventArgs> OneArgumentPreviewCanExecuteAssertionHandler { get; } = e => PreviewCanExecuteEventHandled = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static ExecutedRoutedEventHandler ExecutedAssertionHandler1 { get; } = (_, e) => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
    static CanExecuteRoutedEventHandler CanExecuteAssertionHandler1 { get; } = (_, e) => CanExecuteEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static ExecutedRoutedEventHandler ExecutedAssertionHandler2 { get; } = (_, e) => ExecutedEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter);
    static CanExecuteRoutedEventHandler CanExecuteAssertionHandler2 { get; } = (_, e) => CanExecuteEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
    static ExecutedRoutedEventHandler PreviewExecutedAssertionHandler { get; } = (_, e) => PreviewExecutedEventHandled = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
    static CanExecuteRoutedEventHandler PreviewCanExecuteAssertionHandler { get; } = (_, e) => PreviewCanExecuteEventHandled = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;

    void ClearEventHandled()
    {
        ExecutedEventHandled1 = false;
        CanExecuteEventHandled1 = false;
        ExecutedEventHandled2 = false;
        CanExecuteEventHandled2 = false;
        PreviewExecutedEventHandled = false;
        PreviewCanExecuteEventHandled = false;
    }

    [Example("When a command handler of the Executed event has no argument")]
    [Sample(Source = typeof(NoArgumentExecutedOnlySampleDataSource))]
    void Ex01(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When a command handler of the Executed and PreviewExecuted event has no argument")]
    [Sample(Source = typeof(NoArgumentExecutedAndPreviewExecutedSampleDataSource))]
    void Ex02(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed and CanExecute event have no argument")]
    [Sample(Source = typeof(NoArgumentExecutedAndCanExecuteSampleDataSource))]
    void Ex03(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed, PreviewExecuted, CanExecute, and PreviewCanExecute event have no argument")]
    [Sample(Source = typeof(NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteSampleDataSource))]
    void Ex04(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the PreviewCanExecute event should be handled", () => PreviewCanExecuteEventHandled);
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the PreviewExecuted event should not be handled", () => !PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the PreviewCanExecute event should not be handled", () => !PreviewCanExecuteEventHandled);
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the PreviewExecuted event should not be handled", () => !PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When some command handlers of the Executed and CanExecute event have no argument")]
    [Sample(Source = typeof(NoArgumentExecutedAndCanExecuteSomeSampleDataSource))]
    void Ex05(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
        Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
        Given("an element that has the test buttons", () => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the CanExecute of another command is executed with the parameter", () => (TestButton2.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled2);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled2);

        ClearEventHandled();

        When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled2);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled2);
    }

    [Example("When a command handler of the Executed event has one argument")]
    [Sample(Source = typeof(OneArgumentExecutedOnlySampleDataSource))]
    void Ex06(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When a command handler of the Executed and PreviewExecuted event has one argument")]
    [Sample(Source = typeof(OneArgumentExecutedAndPreviewExecutedSampleDataSource))]
    void Ex07(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed and CanExecute event have one argument")]
    [Sample(Source = typeof(OneArgumentExecutedAndCanExecuteSampleDataSource))]
    void Ex08(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed, PreviewExecuted, CanExecute, and PreviewCanExecute event have one argument")]
    [Sample(Source = typeof(OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteSampleDataSource))]
    void Ex09(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the PreviewCanExecute event should be handled", () => PreviewCanExecuteEventHandled);
        Then("the CanExecute event should not be handled", () => !CanExecuteEventHandled1);
        Then("the PreviewExecuted event should not be handled", () => !PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the PreviewCanExecute event should not be handled", () => !PreviewCanExecuteEventHandled);
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When some command handlers of the Executed and CanExecute event have one argument")]
    [Sample(Source = typeof(OneArgumentExecutedAndCanExecuteSomeSampleDataSource))]
    void Ex10(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
        Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
        Given("an element that has the test buttons", () => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should be handled", () => ExecutedEventHandled1);

        ClearEventHandled();

        When("the CanExecute of another command is executed with the parameter", () => (TestButton2.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled2);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled2);

        ClearEventHandled();

        When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled2);
        Then("the Executed event should be handled", () => ExecutedEventHandled2);
    }

    [Example("When a command handler of the Executed event is the ExecutedRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedOnlySampleDataSource))]
    void Ex11(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When a command handler of the Executed and PreviewExecuted event is the ExecutedRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedAndPreviewExecutedSampleDataSource))]
    void Ex12(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedAndCanExecuteSampleDataSource))]
    void Ex13(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should be handled", () => ExecutedEventHandled1);
    }

    [Example("When command handlers of the Executed, PreviewExecuted, CanExecute, and PreviewCanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteSampleDataSource))]
    void Ex14(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
        Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the PreviewCanExecute event should be handled", () => PreviewCanExecuteEventHandled);
        Then("the CanExecute event should not be handled", () => !CanExecuteEventHandled1);
        Then("the PreviewExecuted event should not be handled", () => !PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the PreviewCanExecute event should not be handled", () => !PreviewCanExecuteEventHandled);
        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the PreviewExecuted event should be handled", () => PreviewExecutedEventHandled);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);
    }

    [Example("When some command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
    [Sample(Source = typeof(ExecutedAndCanExecuteSomeSampleDataSource))]
    void Ex15(object controller)
    {
        ClearEventHandled();

        Given("a data context", () => DataContext = new object());
        Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
        Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
        Given("an element that has the test buttons", () => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });

        When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
        When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
        When("the Initialized event is raised", () => Element.RaiseInitialized());

        When("the CanExecute of the command is executed with the parameter", () => (TestButton1.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled1);

        ClearEventHandled();

        When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled1);
        Then("the Executed event should be handled", () => ExecutedEventHandled1);

        ClearEventHandled();

        When("the CanExecute of another command is executed with the parameter", () => (TestButton2.Command as RoutedCommand)?.CanExecute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled2);
        Then("the Executed event should not be handled", () => !ExecutedEventHandled2);

        ClearEventHandled();

        When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

        Then("the CanExecute event should be handled", () => CanExecuteEventHandled2);
        Then("the Executed event should be handled", () => ExecutedEventHandled2);
    }

    class NoArgumentExecutedOnlySampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.NoArgumentExecutedOnlyHandlerController(NoArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.NoArgumentExecutedOnlyHandlerController(NoArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.NoArgumentExecutedOnlyHandlerController(NoArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentExecutedOnlyHandlerController(NoArgumentExecutedAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.NoArgumentExecutedOnlyHandlerController(NoArgumentExecutedAssertionHandler1) };
        }
    }

    class NoArgumentExecutedAndPreviewExecutedSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.NoArgumentExecutedAndPreviewExecutedHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.NoArgumentExecutedAndPreviewExecutedHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.NoArgumentExecutedAndPreviewExecutedHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentExecutedAndPreviewExecutedHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.NoArgumentExecutedAndPreviewExecutedHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler) };
        }
    }

    class NoArgumentExecutedAndCanExecuteSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1) };
        }
    }

    class NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler, NoArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler, NoArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler, NoArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler, NoArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.NoArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentPreviewExecutedAssertionHandler, NoArgumentPreviewCanExecuteAssertionHandler) };
        }
    }

    class NoArgumentExecutedAndCanExecuteSomeSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentExecutedAssertionHandler2, NoArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentExecutedAssertionHandler2, NoArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentExecutedAssertionHandler2, NoArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentExecutedAssertionHandler2, NoArgumentCanExecuteAssertionHandler2) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.NoArgumentExecutedAndCanExecuteHandlerController(NoArgumentExecutedAssertionHandler1, NoArgumentCanExecuteAssertionHandler1, NoArgumentExecutedAssertionHandler2, NoArgumentCanExecuteAssertionHandler2) };
        }
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

    class OneArgumentExecutedAndPreviewExecutedSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndPreviewExecutedHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedAndPreviewExecutedHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedAndPreviewExecutedHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentExecutedAndPreviewExecutedHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.OneArgumentExecutedAndPreviewExecutedHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler) };
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

    class OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler, OneArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler, OneArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler, OneArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler, OneArgumentPreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.OneArgumentExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(OneArgumentExecutedAssertionHandler1, OneArgumentCanExecuteAssertionHandler1, OneArgumentPreviewExecutedAssertionHandler, OneArgumentPreviewCanExecuteAssertionHandler) };
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

    class ExecutedAndPreviewExecutedSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.ExecutedAndPreviewExecutedHandlerController(ExecutedAssertionHandler1, PreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.ExecutedAndPreviewExecutedHandlerController(ExecutedAssertionHandler1, PreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.ExecutedAndPreviewExecutedHandlerController(ExecutedAssertionHandler1, PreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.ExecutedAndPreviewExecutedHandlerController(ExecutedAssertionHandler1, PreviewExecutedAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.ExecutedAndPreviewExecutedHandlerController(ExecutedAssertionHandler1, PreviewExecutedAssertionHandler) };
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

    class ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteSampleDataSource : ISampleDataSource
    {
        IEnumerable ISampleDataSource.GetData()
        {
            yield return new { Description = "When the command handlers are attributed to fields", Controller = new TestWpfControllers.AttributedToField.ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, PreviewExecutedAssertionHandler, PreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to properties", Controller = new TestWpfControllers.AttributedToProperty.ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, PreviewExecutedAssertionHandler, PreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods", Controller = new TestWpfControllers.AttributedToMethod.ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, PreviewExecutedAssertionHandler, PreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to methods using a naming convention", Controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, PreviewExecutedAssertionHandler, PreviewCanExecuteAssertionHandler) };
            yield return new { Description = "When the command handlers are attributed to async methods using a naming convention", Controller = new TestWpfControllers.AttributedToAsyncMethodUsingNamingConvention.ExecutedAndPreviewExecutedAndCanExecuteAndPreviewCanExecuteHandlerController(ExecutedAssertionHandler1, CanExecuteAssertionHandler1, PreviewExecutedAssertionHandler, PreviewCanExecuteAssertionHandler) };
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