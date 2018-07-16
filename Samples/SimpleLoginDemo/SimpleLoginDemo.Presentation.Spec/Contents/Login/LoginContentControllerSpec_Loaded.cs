// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;
using Carna;
using Charites.Windows.Mvc;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    [Context("When the root element is loaded")]
    class LoginContentControllerSpec_Loaded : FixtureSteppable , IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        const string PasswordBoxKey = "PasswordBox";
        const string LoginContentKey = "LoginContent";
        const string ControllerKey = "Controller";

        public LoginContentControllerSpec_Loaded()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
        }

        public void Dispose()
        {
            WpfRunner.Shutdown();
        }

        [Example("Adds the PasswordChanged event handler that sets the password of the PasswordBox to the LoginContent")]
        void Ex01()
        {
            Given("a PasswordBox", () => WpfRunner.Run((application, context) => context.Set(PasswordBoxKey, new PasswordBox { Name = "PasswordBox" })));
            Given("a login content", () => WpfRunner.Run((application, context) => context.Set(LoginContentKey, new LoginContent())));
            Given("a controller to which the PasswordBox and the login content are set", () => WpfRunner.Run((application, context) =>
            {
                var controller = new LoginContentController(null);
                context.Set(ControllerKey, controller);
                WpfController.SetDataContext(context.Get<LoginContent>(LoginContentKey), controller);
                WpfController.SetElement(context.Get<PasswordBox>(PasswordBoxKey), controller, true);
            }));

            When("the Loaded event is raised", () => WpfRunner.Run((application, context) =>
                WpfController.EventHandlersOf(context.Get<LoginContentController>(ControllerKey))
                    .GetBy(null)
                    .Raise(FrameworkElement.LoadedEvent.Name)
            ));
            When("the password is set to the PasswordBox", () => WpfRunner.Run((application, context) => context.Get<PasswordBox>(PasswordBoxKey).Password = "password"));
            Then("the password of the login content should be set", () => WpfRunner.Run((application, context) =>
                context.Get<LoginContent>(LoginContentKey).Password.Value.Should().Be(context.Get<PasswordBox>(PasswordBoxKey).Password)
            ));
        }
    }
}
