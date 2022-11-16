// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Controls;
using Carna;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[Context("When the password is changed")]
class LoginContentControllerSpec_PasswordBox_PasswordChanged : FixtureSteppable
{
    PasswordBox PasswordBox { get; } = new();
    LoginContent Content { get; } = new();
    LoginContentController Controller { get; } = new();

    string Password => "Password";

    public LoginContentControllerSpec_PasswordBox_PasswordChanged()
    {
        WpfController.SetDataContext(Content, Controller);
    }

    [Example("Sets the password of the PasswordBox to the LoginContent")]
    void Ex01()
    {
        When("the password is set to the PasswordBox", () =>
        {
            PasswordBox.Password = Password;
            WpfController.EventHandlersOf(Controller)
                .GetBy("PasswordBox")
                .From(PasswordBox)
                .Raise(nameof(PasswordBox.PasswordChanged));
        });
        Then("the password of the login content should be set", () => Content.Password.Value == Password);
    }
}