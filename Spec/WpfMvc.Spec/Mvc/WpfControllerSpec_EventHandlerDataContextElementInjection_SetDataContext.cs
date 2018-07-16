// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("Sets a data context")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_SetDataContext : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string DataContextKey = "DataContext";
        const string ControllerKey = "Controller";

        public WpfControllerSpec_EventHandlerDataContextElementInjection_SetDataContext()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("When the data context are attributed to a field")]
        void Ex01()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a controller", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.AttributedToField.NoArgumentHandlerController(null))));
            When("the data context is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetDataContext(context.Get<object>(DataContextKey), context.Get<TestWpfControllers.AttributedToField.NoArgumentHandlerController>(ControllerKey))));
            Then("the data context should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToField.NoArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
        }

        [Example("When the data context are attributed to a property")]
        void Ex02()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a controller", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(null))));
            When("the data context is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetDataContext(context.Get<object>(DataContextKey), context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey))));
            Then("the data context should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToProperty.NoArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
        }

        [Example("When the data context are attributed to a method")]
        void Ex03()
        {
            Given("a data context", () => WpfRunner.Run((application, context) => context.Set(DataContextKey, new object())));
            Given("a controller", () => WpfRunner.Run((application, context) => context.Set(ControllerKey, new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(null))));
            When("the data context is set to the controller using the WpfController", () => WpfRunner.Run((application, context) => WpfController.SetDataContext(context.Get<object>(DataContextKey), context.Get<TestWpfControllers.AttributedToMethod.NoArgumentHandlerController>(ControllerKey))));
            Then("the data context should be set to the controller", () => WpfRunner.Run((application, context) => context.Get<TestWpfControllers.AttributedToMethod.NoArgumentHandlerController>(ControllerKey).DataContext.Should().Be(context.Get<object>(DataContextKey))));
        }
    }
}
