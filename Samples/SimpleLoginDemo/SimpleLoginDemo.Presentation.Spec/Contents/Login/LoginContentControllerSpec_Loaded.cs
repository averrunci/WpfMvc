// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Controls;
using Carna;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    [Context("When the root element is loaded", RequiresSta = true)]
    class LoginContentControllerSpec_Loaded : FixtureSteppable
    {
        PasswordBox PasswordBox { get; } = new PasswordBox { Name = "PasswordBox"};
        LoginContent Content { get; } = new LoginContent();
        LoginContentController Controller { get; } = new LoginContentController(null);

        [Background("a controller to which the PasswordBox and the login content are set")]
        public LoginContentControllerSpec_Loaded()
        {
            WpfController.SetDataContext(Content, Controller);
            WpfController.SetElement(PasswordBox, Controller, true);
        }

        [Example("Adds the PasswordChanged event handler that sets the password of the PasswordBox to the LoginContent")]
        void Ex01()
        {
            When("the Loaded event is raised", () =>
                WpfController.EventHandlersOf(Controller)
                    .GetBy(null)
                    .Raise(FrameworkElement.LoadedEvent.Name)
            );
            When("the password is set to the PasswordBox", () => PasswordBox.Password = "password");
            Then("the password of the login content should be set", () => Content.Password.Value == PasswordBox.Password);
        }
    }
}
