// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Controls;
using System.Windows.Input;
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("The command handlers are attributed to fields")]
    class WpfControllerSpec_CommandHandlerInjection_AttributedToField : FixtureSteppable
    {
        object DataContext { get; set; }
        Button TestButton1 { get; set; }
        Button TestButton2 { get; set; }
        TestElement Element { get; set; }
        object Parameter { get; set; }

        bool ExecutedEventHandled1 { get; set; }
        bool CanExecuteEventHandled1 { get; set; }
        bool ExecutedEventHandled2 { get; set; }
        bool CanExecuteEventHandled2 { get; set; }

        [Example("When a command handler of the Executed event has one argument")]
        void Ex01()
        {
            TestWpfControllers.AttributedToField.OneArgumentExecutedOnlyHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
            Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });
            Given("a controller that has command handlers", () =>
            {
                void HandleExecutedEvent(ExecutedRoutedEventArgs e) => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
                controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedOnlyHandlerController(HandleExecutedEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter = new object(), Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled1);
        }

        [Example("When command handlers of the Executed and CanExecute event have one argument")]
        void Ex02()
        {
            TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
            Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });
            Given("a controller that has command handlers", () =>
            {
                void HandleExecutedEvent(ExecutedRoutedEventArgs e) => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
                void HandleCanExecuteEvent(CanExecuteRoutedEventArgs e) => CanExecuteEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
                controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(HandleExecutedEvent, HandleCanExecuteEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter = new object(), Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled1);
            Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);
        }

        [Example("When some command handlers of the Executed and CanExecute event have one argument")]
        void Ex03()
        {
            TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
            Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
            Given("an element that has the test buttons",() => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });
            Given("a controller that has command handlers", () =>
            {
                void HandleExecutedEvent1(ExecutedRoutedEventArgs e) => ExecutedEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
                void HandleCanExecuteEvent1(CanExecuteRoutedEventArgs e) => CanExecuteEventHandled1 = Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
                void HandleExecutedEvent2(ExecutedRoutedEventArgs e) => ExecutedEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter);
                void HandleCanExecuteEvent2(CanExecuteRoutedEventArgs e) => CanExecuteEventHandled2 = Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
                controller = new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(HandleExecutedEvent1, HandleCanExecuteEvent1, HandleExecutedEvent2, HandleCanExecuteEvent2);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter = new object(), Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled1);
            Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);

            When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled2);
            Then("the CanExecuted event should be handled", () => CanExecuteEventHandled2);
        }

        [Example("When a command handler of the Executed event is the ExecutedRoutedEventHandler")]
        void Ex04()
        {
            TestWpfControllers.AttributedToField.ExecutedOnlyHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
            Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });
            Given("a controller that has command handlers", () =>
            {
                void HandleExecutedEvent(object sender, ExecutedRoutedEventArgs e) => ExecutedEventHandled1 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
                controller = new TestWpfControllers.AttributedToField.ExecutedOnlyHandlerController(HandleExecutedEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter = new object(), Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled1);
        }

        [Example("When command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
        void Ex05()
        {
            TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton", Command = TestWpfControllers.TestCommand });
            Given("an element that has the test button", () => Element = new TestElement { Name = "element", Content = TestButton1, DataContext = DataContext });
            Given("a controller that has command handlers", () =>
            {
                void HandleExecutedEvent(object sender, ExecutedRoutedEventArgs e) => ExecutedEventHandled1 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
                void HandleCanExecuteEvent(object sender, CanExecuteRoutedEventArgs e) => CanExecuteEventHandled1 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
                controller = new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(HandleExecutedEvent, HandleCanExecuteEvent);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter = new object(), Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled1);
            Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);
        }

        [Example("When some command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
        void Ex06()
        {
            TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a test button that has a command", () => TestButton1 = new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand });
            Given("another test button that has a command", () => TestButton2 = new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand });
            Given("an element that has the test buttons", () => Element = new TestElement { Name = "element", Content = new Grid { Children = { TestButton1, TestButton2 } }, DataContext = DataContext });
            Given("a controller that has command handlers", () =>
            {
                void HandleExecutedEvent1(object sender, ExecutedRoutedEventArgs e) => ExecutedEventHandled1 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter);
                void HandleCanExecuteEvent1(object sender, CanExecuteRoutedEventArgs e) => CanExecuteEventHandled1 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
                void HandleExecutedEvent2(object sender, ExecutedRoutedEventArgs e) => ExecutedEventHandled2 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter);
                void HandleCanExecuteEvent2(object sender, CanExecuteRoutedEventArgs e) => CanExecuteEventHandled2 = Equals(sender, Element) && Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, Parameter) && e.CanExecute;
                controller = new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(HandleExecutedEvent1, HandleCanExecuteEvent1, HandleExecutedEvent2, HandleCanExecuteEvent2);
            });

            When("the controller is added", () => WpfController.GetControllers(Element).Add(controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));
            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the command is executed with a parameter", () => (TestButton1.Command as RoutedCommand)?.Execute(Parameter = new object(), Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled1);
            Then("the CanExecuted event should be handled", () => CanExecuteEventHandled1);

            When("another command is executed with a parameter", () => (TestButton2.Command as RoutedCommand)?.Execute(Parameter, Element));

            Then("the Executed event should be handled", () => ExecutedEventHandled2);
            Then("the CanExecuted event should be handled", () => CanExecuteEventHandled2);
        }
    }
}
