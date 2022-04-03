// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Controls;
using Carna;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[Context("When the root element is loaded", RequiresSta = true)]
class LoginContentControllerSpec_Loaded : FixtureSteppable
{
    PasswordBox PasswordBox { get; } = new() { Name = "PasswordBox"};
    LoginContent Content { get; } = new();
    LoginContentController Controller { get; } = new();

    [Background("a controller to which the PasswordBox and the login content are set")]
    public LoginContentControllerSpec_Loaded()
    {
        WpfController.SetDataContext(Content, Controller);
    }

    [Example("Adds the PasswordChanged event handler that sets the password of the PasswordBox to the LoginContent")]
    void Ex01()
    {
        When("the PasswordBox is set", () => WpfController.SetElement(PasswordBox, Controller, true));
        When("the password is set to the PasswordBox", () => PasswordBox.Password = "password");
        Then("the password of the login content should be set", () => Content.Password.Value == PasswordBox.Password);
    }
}