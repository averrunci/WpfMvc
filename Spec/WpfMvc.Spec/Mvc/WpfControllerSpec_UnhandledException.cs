// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Unhandled exception")]
    class WpfControllerSpec_UnhandledException : FixtureSteppable, IDisposable
    {
        TestWpfControllers.ExceptionTestWpfController Controller { get; } = new TestWpfControllers.ExceptionTestWpfController();
        TestElement Element { get; } = new TestElement();

        bool ExceptionHandled { get; set; }
        Exception UnhandledException { get; set; }

        public WpfControllerSpec_UnhandledException()
        {
            WpfController.UnhandledException += OnWpfControllerUnhandledException;
        }

        public void Dispose()
        {
            WpfController.UnhandledException -= OnWpfControllerUnhandledException;
        }

        void OnWpfControllerUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            UnhandledException = e.Exception;
            e.Handled = ExceptionHandled;
        }

        [Example("Handles an unhandled exception as it is handled")]
        void Ex01()
        {
            ExceptionHandled = true;

            When("the controller is added", () => WpfController.GetControllers(Element).Add(Controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));

            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the Changed event of the element is raised", () => Element.RaiseChanged());
            Then("the unhandled exception should be handled", () => UnhandledException != null);
        }

        [Example("Handles an unhandled exception as it is not handled")]
        void Ex02()
        {
            ExceptionHandled = false;

            When("the controller is added", () => WpfController.GetControllers(Element).Add(Controller));
            When("the controller is attached to the element", () => WpfController.GetControllers(Element).AttachTo(Element));

            When("the Initialized event is raised", () => Element.RaiseInitialized());

            When("the Changed event of the element is raised", () => Element.RaiseChanged());
            Then<TargetInvocationException>("the exception should be thrown");
            Then("the unhandled exception should be handled", () => UnhandledException != null);
        }
    }
}
