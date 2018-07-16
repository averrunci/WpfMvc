// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("The command handlers are attributed to fields")]
    class WpfControllerSpec_CommandHandlerInjection_AttributedToField : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string DataContextKey = "DataContext";
        const string TestButton1Key = "TestButton1";
        const string TestButton2Key = "TestButton2";
        const string ElementKey = "Element";
        const string ControllerKey = "Controller";
        const string ParameterKey = "Parameter";
        const string ExecutedEventHandled1Key = "ExecutedEventHandled1";
        const string CanExecuteEventHandled1Key = "CanExecuteEventHandled1";
        const string ExecutedEventHandled2Key = "ExecutedEventHandled2";
        const string CanExecuteEventHandled2Key = "CanExecuteEventHandled2";

        public WpfControllerSpec_CommandHandlerInjection_AttributedToField()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("When a command handler of the Executed event has one argument")]
        void Ex01()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton1Key, new Button { Name = "testButton", Command = TestWpfControllers.TestCommand})));
            Given("an element that has the test button", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<Button>(TestButton1Key), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has command handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleExecutedEvent(ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled1Key, Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                context.Set(ControllerKey, new TestWpfControllers.AttributedToField.OneArgumentExecutedOnlyHandlerController(HandleExecutedEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToField.OneArgumentExecutedOnlyHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the command is executed with a parameter", () => WpfRunner.Run((application, context) =>
            {
                var parameter = new object();
                context.Set(ParameterKey, parameter);
                (context.Get<Button>(TestButton1Key).Command as RoutedCommand)?.Execute(parameter, context.Get<TestElement>(ElementKey));
            }));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled1Key).Should().BeTrue()));
        }

        [Example("When command handlers of the Executed and CanExecute event have one argument")]
        void Ex02()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton1Key, new Button { Name = "testButton", Command = TestWpfControllers.TestCommand })));
            Given("an element that has the test button", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<Button>(TestButton1Key), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has command handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleExecutedEvent(ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled1Key, Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                void HandleCanExecuteEvent(CanExecuteRoutedEventArgs e) => context.Set(CanExecuteEventHandled1Key, Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)) && e.CanExecute);
                context.Set(ControllerKey, new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(HandleExecutedEvent, HandleCanExecuteEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the command is executed with a parameter", () => WpfRunner.Run((application, context) =>
            {
                var parameter = new object();
                context.Set(ParameterKey, parameter);
                (context.Get<Button>(TestButton1Key).Command as RoutedCommand)?.Execute(parameter, context.Get<TestElement>(ElementKey));
            }));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled1Key).Should().BeTrue()));
            Then("the CanExecuted event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(CanExecuteEventHandled1Key).Should().BeTrue()));
        }

        [Example("When some command handlers of the Executed and CanExecute event have one argument")]
        void Ex03()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton1Key, new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand })));
            Given("another test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton2Key, new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand })));
            Given("an element that has the test buttons",() => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = new Grid { Children = { context.Get<Button>(TestButton1Key), context.Get<Button>(TestButton2Key) } }, DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has command handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleExecutedEvent1(ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled1Key, Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                void HandleCanExecuteEvent1(CanExecuteRoutedEventArgs e) => context.Set(CanExecuteEventHandled1Key, Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)) && e.CanExecute);
                void HandleExecutedEvent2(ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled2Key, Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                void HandleCanExecuteEvent2(CanExecuteRoutedEventArgs e) => context.Set(CanExecuteEventHandled2Key, Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)) && e.CanExecute);
                context.Set(ControllerKey, new TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController(HandleExecutedEvent1, HandleCanExecuteEvent1, HandleExecutedEvent2, HandleCanExecuteEvent2));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToField.OneArgumentExecutedAndCanExecuteHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the command is executed with a parameter", () => WpfRunner.Run((application, context) =>
            {
                var parameter = new object();
                context.Set(ParameterKey, parameter);
                (context.Get<Button>(TestButton1Key).Command as RoutedCommand)?.Execute(parameter, context.Get<TestElement>(ElementKey));
            }));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled1Key).Should().BeTrue()));
            Then("the CanExecuted event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(CanExecuteEventHandled1Key).Should().BeTrue()));

            When("another command is executed with a parameter", () => WpfRunner.Run((application, context) => (context.Get<Button>(TestButton2Key).Command as RoutedCommand)?.Execute(context.Get<object>(ParameterKey), context.Get<TestElement>(ElementKey))));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled2Key).Should().BeTrue()));
            Then("the CanExecuted event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(CanExecuteEventHandled2Key).Should().BeTrue()));
        }

        [Example("When a command handler of the Executed event is the ExecutedRoutedEventHandler")]
        void Ex04()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton1Key, new Button { Name = "testButton", Command = TestWpfControllers.TestCommand })));
            Given("an element that has the test button", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<Button>(TestButton1Key), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has command handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleExecutedEvent(object sender, ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled1Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                context.Set(ControllerKey, new TestWpfControllers.AttributedToField.ExecutedOnlyHandlerController(HandleExecutedEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToField.ExecutedOnlyHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the command is executed with a parameter", () => WpfRunner.Run((application, context) =>
            {
                var parameter = new object();
                context.Set(ParameterKey, parameter);
                (context.Get<Button>(TestButton1Key).Command as RoutedCommand)?.Execute(parameter, context.Get<TestElement>(ElementKey));
            }));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled1Key).Should().BeTrue()));
        }

        [Example("When command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
        void Ex05()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton1Key, new Button { Name = "testButton", Command = TestWpfControllers.TestCommand })));
            Given("an element that has the test button", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = context.Get<Button>(TestButton1Key), DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has command handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleExecutedEvent(object sender, ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled1Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                void HandleCanExecuteEvent(object sender, CanExecuteRoutedEventArgs e) => context.Set(CanExecuteEventHandled1Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)) && e.CanExecute);
                context.Set(ControllerKey, new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(HandleExecutedEvent, HandleCanExecuteEvent));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the command is executed with a parameter", () => WpfRunner.Run((application, context) =>
            {
                var parameter = new object();
                context.Set(ParameterKey, parameter);
                (context.Get<Button>(TestButton1Key).Command as RoutedCommand)?.Execute(parameter, context.Get<TestElement>(ElementKey));
            }));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled1Key).Should().BeTrue()));
            Then("the CanExecuted event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(CanExecuteEventHandled1Key).Should().BeTrue()));
        }

        [Example("When some command handlers of the Executed and CanExecute event are the ExecutedRoutedEventHandler and the CanExecuteRoutedEventHandler")]
        void Ex06()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton1Key, new Button { Name = "testButton1", Command = TestWpfControllers.TestCommand })));
            Given("another test button that has a command", () => WpfRunner.Run((application, context) => context.Set(TestButton2Key, new Button { Name = "testButton2", Command = TestWpfControllers.AnotherTestCommand })));
            Given("an element that has the test buttons", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { Name = "element", Content = new Grid { Children = { context.Get<Button>(TestButton1Key), context.Get<Button>(TestButton2Key) } }, DataContext = context.Get<object>(DataContextKey) })));
            Given("a controller that has command handlers", () => WpfRunner.Run((application, context) =>
            {
                void HandleExecutedEvent1(object sender, ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled1Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                void HandleCanExecuteEvent1(object sender, CanExecuteRoutedEventArgs e) => context.Set(CanExecuteEventHandled1Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.TestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)) && e.CanExecute);
                void HandleExecutedEvent2(object sender, ExecutedRoutedEventArgs e) => context.Set(ExecutedEventHandled2Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)));
                void HandleCanExecuteEvent2(object sender, CanExecuteRoutedEventArgs e) => context.Set(CanExecuteEventHandled2Key, Equals(sender, context.Get<TestElement>(ElementKey)) && Equals(e.Command, TestWpfControllers.AnotherTestCommand) && Equals(e.Parameter, context.Get<object>(ParameterKey)) && e.CanExecute);
                context.Set(ControllerKey, new TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController(HandleExecutedEvent1, HandleCanExecuteEvent1, HandleExecutedEvent2, HandleCanExecuteEvent2));
            }));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.AttributedToField.ExecutedAndCanExecuteHandlerController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));
            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the command is executed with a parameter", () => WpfRunner.Run((application, context) =>
            {
                var parameter = new object();
                context.Set(ParameterKey, parameter);
                (context.Get<Button>(TestButton1Key).Command as RoutedCommand)?.Execute(parameter, context.Get<TestElement>(ElementKey));
            }));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled1Key).Should().BeTrue()));
            Then("the CanExecuted event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(CanExecuteEventHandled1Key).Should().BeTrue()));

            When("another command is executed with a parameter", () => WpfRunner.Run((application, context) => (context.Get<Button>(TestButton2Key).Command as RoutedCommand)?.Execute(context.Get<object>(ParameterKey), context.Get<TestElement>(ElementKey))));

            Then("the Executed event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(ExecutedEventHandled2Key).Should().BeTrue()));
            Then("the CanExecuted event should be handled", () => WpfRunner.Run((application, context) => context.Get<bool>(CanExecuteEventHandled2Key).Should().BeTrue()));
        }
    }
}
