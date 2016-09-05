// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Input;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Runners;

namespace Fievus.Windows.Mvc
{
    [TestFixture]
    public class CommandHandlerBaseTest
    {
        [Test]
        public void ExecutesCommandHandlerOfExecutedEventWithSpecifiedCommandName()
        {
            var handler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
            var notCalledHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();

            var commandHandlerBase = new CommandHandlerBase();
            commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, null, handler);
            commandHandlerBase.Add("AnotherTestCommand", new RoutedCommand("AnotherTestCommand", typeof(CommandHandlerBaseTest)), null, notCalledHandler);

            var sender = new object();
            var parameter = new object();
            commandHandlerBase.GetBy(TestWpfControllers.TestCommand.Name)
                .From(sender)
                .RaiseExecuted(parameter);

            handler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(sender), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
            notCalledHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<ExecutedRoutedEventArgs>.Is.Anything));
        }

        [Test]
        public void ExecutesCommandHandlerOfCanExecuteEventWithSpecifiedCommandName()
        {
            var handler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();
            var notCalledHandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();

            var commandHandlerBase = new CommandHandlerBase();
            commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, null, handler);
            commandHandlerBase.Add("AnotherTestCommand", new RoutedCommand("AnotherTestCommand", typeof(CommandHandlerBaseTest)), null, notCalledHandler);

            var sender = new object();
            var parameter = new object();
            var args = commandHandlerBase.GetBy(TestWpfControllers.TestCommand.Name)
                .From(sender)
                .RaiseCanExecute(parameter);

            Assert.That(args, Has.Count.EqualTo(1));
            handler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(sender), Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
            notCalledHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<CanExecuteRoutedEventArgs>.Is.Anything));
        }

        [Test]
        public void RegistersCommandHandlerOfExecutedEventToSpecifiedElement()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var handler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                var notCalledHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();

                var commandHandlerBase = new CommandHandlerBase();
                commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, element, handler);
                commandHandlerBase.Add("AnotherTestCommand", new RoutedCommand("AnotherTestCommand", typeof(CommandHandlerBaseTest)), element, notCalledHandler);

                commandHandlerBase.RegisterCommandHandler();

                var parameter = new object();
                TestWpfControllers.TestCommand.Execute(parameter, element);

                handler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                notCalledHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<ExecutedRoutedEventArgs>.Is.Anything));
            }).Shutdown();
        }

        [Test]
        public void RegistersCommandHandlerOfCanExecuteEventToSpecifiedElement()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var handler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();
                var notCalledHandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();

                var commandHandlerBase = new CommandHandlerBase();
                commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, element, handler);
                commandHandlerBase.Add("AnotherTestCommand", new RoutedCommand("AnotherTestCommand", typeof(CommandHandlerBaseTest)), element, notCalledHandler);

                commandHandlerBase.RegisterCommandHandler();

                var parameter = new object();
                TestWpfControllers.TestCommand.CanExecute(parameter, element);

                handler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<CanExecuteRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                notCalledHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<CanExecuteRoutedEventArgs>.Is.Anything));
            }).Shutdown();
        }

        [Test]
        public void RegistersCommandHandlerOfExecutedAndCanExecuteEventToSpecifiedElement()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var executedHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                CanExecuteRoutedEventHandler canExecuteHandler = (s, e) => e.CanExecute = true;
                var notCalledExecutedHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                var notCalledCanExecuteHandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();

                var commandHandlerBase = new CommandHandlerBase();
                commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, element, executedHandler);
                commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, element, canExecuteHandler);
                commandHandlerBase.Add("AnotherTestCommand", new RoutedCommand("AnotherTestCommand", typeof(CommandHandlerBaseTest)), element, notCalledExecutedHandler);
                commandHandlerBase.Add("AnotherTestCommand", new RoutedCommand("AnotherTestCommand", typeof(CommandHandlerBaseTest)), element, notCalledCanExecuteHandler);

                commandHandlerBase.RegisterCommandHandler();

                var parameter = new object();
                TestWpfControllers.TestCommand.Execute(parameter, element);

                executedHandler.AssertWasCalled(h => h.Invoke(Arg<object>.Is.Equal(element), Arg<ExecutedRoutedEventArgs>.Matches(e => e.Parameter == parameter)));
                notCalledExecutedHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<ExecutedRoutedEventArgs>.Is.Anything));
                notCalledCanExecuteHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<CanExecuteRoutedEventArgs>.Is.Anything));
            }).Shutdown();
        }

        [Test]
        public void UnregistersCommandHandlerFromSpecifiedElement()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var element = new FrameworkElement { Name = "TestElement" };
                var executedHandler = MockRepository.GenerateMock<ExecutedRoutedEventHandler>();
                var canExecutehandler = MockRepository.GenerateMock<CanExecuteRoutedEventHandler>();

                var commandHandlerBase = new CommandHandlerBase();
                commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, element, executedHandler);
                commandHandlerBase.Add(TestWpfControllers.TestCommand.Name, TestWpfControllers.TestCommand, element, canExecutehandler);
                commandHandlerBase.RegisterCommandHandler();
                commandHandlerBase.UnregisterCommandHandler();

                var parameter = new object();
                TestWpfControllers.TestCommand.Execute(parameter, element);
                TestWpfControllers.TestCommand.CanExecute(parameter, element);

                executedHandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<ExecutedRoutedEventArgs>.Is.Anything));
                canExecutehandler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<CanExecuteRoutedEventArgs>.Is.Anything));
            }).Shutdown();
        }
    }
}
