// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Sets a data context")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_SetDataContext : FixtureSteppable
    {
        object DataContext { get; set; }

        [Example("When the data context are attributed to a field")]
        void Ex01()
        {
            TestWpfControllers.AttributedToField.NoArgumentHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a controller", () => controller = new TestWpfControllers.AttributedToField.NoArgumentHandlerController(null));
            When("the data context is set to the controller using the WpfController", () => WpfController.SetDataContext(DataContext, controller));
            Then("the data context should be set to the controller", () => controller.DataContext == DataContext);
        }

        [Example("When the data context are attributed to a property")]
        void Ex02()
        {
            TestWpfControllers.AttributedToProperty.NoArgumentHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a controller", () => controller = new TestWpfControllers.AttributedToProperty.NoArgumentHandlerController(null));
            When("the data context is set to the controller using the WpfController", () => WpfController.SetDataContext(DataContext, controller));
            Then("the data context should be set to the controller", () => controller.DataContext == DataContext);
        }

        [Example("When the data context are attributed to a method")]
        void Ex03()
        {
            TestWpfControllers.AttributedToMethod.NoArgumentHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a controller", () => controller = new TestWpfControllers.AttributedToMethod.NoArgumentHandlerController(null));
            When("the data context is set to the controller using the WpfController", () => WpfController.SetDataContext(DataContext, controller));
            Then("the data context should be set to the controller", () => controller.DataContext == DataContext);
        }

        [Example("When the data context are attributed to a method using a naming convention")]
        void Ex04()
        {
            TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentHandlerController controller = null;

            Given("a data context", () => DataContext = new object());
            Given("a controller", () => controller = new TestWpfControllers.AttributedToMethodUsingNamingConvention.NoArgumentHandlerController(null));
            When("the data context is set to the controller using the WpfController", () => WpfController.SetDataContext(DataContext, controller));
            Then("the data context should be set to the controller", () => controller.DataContext == DataContext);
        }
    }
}
