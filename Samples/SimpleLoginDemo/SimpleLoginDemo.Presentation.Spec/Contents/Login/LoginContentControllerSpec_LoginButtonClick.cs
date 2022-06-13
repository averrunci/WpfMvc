// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Properties;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[Context("When the login button is clicked")]
class LoginContentControllerSpec_LoginButtonClick : FixtureSteppable
{
    LoginContentController Controller { get; } = new();

    LoginContent LoginContent { get; } = new() { Message = { Value = "message" } };
    IContentNavigator Navigator { get; } = Substitute.For<IContentNavigator>();
    ILoginCommand LoginCommand { get; } = Substitute.For<ILoginCommand>();

    public LoginContentControllerSpec_LoginButtonClick()
    {
        WpfController.SetDataContext(LoginContent, Controller);
    }

    [Example("When the user is authenticated")]
    void Ex01()
    {
        When("the valid user id and password are set", () =>
        {
            LoginContent.UserId.Value = "user";
            LoginContent.Password.Value = "password";

            LoginCommand.AuthenticateAsync(LoginContent)
                .Returns(LoginAuthenticationResult.Succeeded());
        });
        When("the login command is executed", async () =>
            await WpfController.CommandHandlersOf(Controller)
                .GetBy(SimpleLoginCommands.Login.Name)
                .With(SimpleLoginCommands.Login)
                .ResolveFromDI<IContentNavigator>(() => Navigator)
                .ResolveFromDI<ILoginCommand>(() => LoginCommand)
                .RaiseExecutedAsync(LoginContent)
        );
        Then("the content should be navigated to the UserContent", () =>
        {
            Navigator.Received(1).NavigateTo(Arg.Is<UserContent>(content => content.Id == LoginContent.UserId.Value));
        });
    }

    [Example("When the login content is not valid")]
    void Ex02()
    {
        When("the invalid user id and password are set", () =>
        {
            LoginContent.UserId.Value = string.Empty;
            LoginContent.Password.Value = string.Empty;
        });
        When("the login command is executed", async () =>
            await WpfController.CommandHandlersOf(Controller)
                .GetBy(SimpleLoginCommands.Login.Name)
                .With(SimpleLoginCommands.Login)
                .ResolveFromDI<IContentNavigator>(() => Navigator)
                .ResolveFromDI<ILoginCommand>(() => LoginCommand)
                .RaiseExecutedAsync(LoginContent)
        );
        Then("the content should not be navigated to any contents", () =>
        {
            Navigator.DidNotReceive().NavigateTo(Arg.Any<object>());
        });
        Then("the message of the login content should be empty", () => LoginContent.Message.Value == string.Empty);
    }

    [Example("When the user is not authenticated")]
    void Ex03()
    {
        When("the no authenticated user id and password are set", () =>
        {
            LoginContent.UserId.Value = "user";
            LoginContent.Password.Value = "password";

            LoginCommand.AuthenticateAsync(LoginContent)
                .Returns(LoginAuthenticationResult.Failed());
        });
        When("the login command is executed", async () =>
            await WpfController.CommandHandlersOf(Controller)
                .GetBy(SimpleLoginCommands.Login.Name)
                .With(SimpleLoginCommands.Login)
                .ResolveFromDI<IContentNavigator>(() => Navigator)
                .ResolveFromDI<ILoginCommand>(() => LoginCommand)
                .RaiseExecutedAsync(LoginContent)
        );
        Then("the content should not be navigated to any contents", () =>
        {
            Navigator.DidNotReceive().NavigateTo(Arg.Any<object>());
        });
        Then("the LoginFailureMessage message should be set", () => LoginContent.Message.Value == Resources.LoginFailureMessage);
    }
}