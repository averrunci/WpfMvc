// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Input;
using Carna;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[Context("Login command can execute")]
class LoginContentControllerSpec_LoginCommandCanExecute : FixtureSteppable
{
    LoginContentController Controller { get; } = new();

    LoginContent LoginContent { get; } = new();

    CanExecuteRoutedEventArgs Result { get; set; } = default!;

    public LoginContentControllerSpec_LoginCommandCanExecute()
    {
        WpfController.SetDataContext(LoginContent, Controller);
    }

    [Example("Enables / disables the login button")]
    [Sample(null, "password", false, Description = "When the user id is null")]
    [Sample("", "password", false, Description = "When the user id is empty")]
    [Sample("user", null, false, Description = "When the password is null")]
    [Sample("user", "", false, Description = "When the password is empty")]
    [Sample("user", "password", true, Description = "When the user id is not null or empty and the password is not null or empty")]
    void Ex01(string userId, string password, bool expected)
    {
        When("the user id is set", () => LoginContent.UserId.Value = userId);
        When("the password is set", () => LoginContent.Password.Value = password);
        When("the CanExecute event is raised", () =>
            Result = WpfController.CommandHandlersOf(Controller)
                .GetBy(SimpleLoginCommands.Login.Name)
                .With(SimpleLoginCommands.Login)
                .RaiseCanExecute(LoginContent)
                .First()
        );
        Then($"the login button should be {(expected ? "enabled" : "disabled")}", () => Result.CanExecute == expected);
    }
}