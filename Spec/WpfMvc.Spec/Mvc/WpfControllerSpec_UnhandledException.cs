// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using System.Windows;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("Unhandled exception")]
    class WpfControllerSpec_UnhandledException : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string UnhandledExceptionHandlerKey = "UnhandledExceptionHanlder";
        const string UnhandledExceptionKey = "UnhandledException";
        const string ControllerKey = "Controller";
        const string ElementKey = "Element";

        public WpfControllerSpec_UnhandledException()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Run((application, context) =>
            {
                var unhandledExceptionHandler = context.Get<UnhandledExceptionEventHandler>(UnhandledExceptionHandlerKey);
                if (unhandledExceptionHandler != null) WpfController.UnhandledException -= unhandledExceptionHandler;
            });

            WpfRunner.Shutdown();
        }

        [Example("Handles an unhandled exception as it is handled")]
        void Ex01()
        {
            WpfRunner.Run((application, context) =>
            {
                void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
                {
                    context.Set(UnhandledExceptionKey, e.Exception);
                    e.Handled = true;
                }

                WpfController.UnhandledException += HandleUnhandledException;
                context.Set(UnhandledExceptionHandlerKey, (UnhandledExceptionEventHandler)HandleUnhandledException);
            });

            Given("a controller that has an event handler that throws an exception", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.ExceptionTestWpfController())));
            Given("an element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.ExceptionTestWpfController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the Changed event of the element is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseChanged()));
            Then("the unhandled exception should be handled", () => WpfRunner.Run((application, context) => context.Get<Exception>(UnhandledExceptionKey).Should().NotBeNull()));
        }

        [Example("Handles an unhandled exception as it is not handled")]
        void Ex02()
        {
            WpfRunner.Run((application, context) =>
            {
                void HandleUnhandledException(object sender, UnhandledExceptionEventArgs e)
                {
                    context.Set(UnhandledExceptionKey, e.Exception);
                    e.Handled = false;
                }

                WpfController.UnhandledException += HandleUnhandledException;
                context.Set(UnhandledExceptionHandlerKey, (UnhandledExceptionEventHandler)HandleUnhandledException);
            });

            Given("a controller that has an event handler that throws an exception", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.ExceptionTestWpfController())));
            Given("an element", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement())));

            When("the controller is added", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).Add(context.Get<TestWpfControllers.ExceptionTestWpfController>(ControllerKey))));
            When("the controller is attached to the element", () => WpfRunner.Run((application, context) => WpfController.GetControllers(context.Get<TestElement>(ElementKey)).AttachTo(context.Get<TestElement>(ElementKey))));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));

            When("the Changed event of the element is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseChanged()));
            Then<TargetInvocationException>("the exception should be thrown");
            Then("the unhandled exception should be handled", () => WpfRunner.Run((application, context) => context.Get<Exception>(UnhandledExceptionKey).Should().NotBeNull()));
        }
    }
}
