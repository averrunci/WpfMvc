// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Properties;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    [Context("When the login button is clicked")]
    class LoginContentControllerSpec_LoginButtonClick : FixtureSteppable
    {
        LoginContentController Controller { get; set; }

        LoginContent LoginContent { get; } = new LoginContent { Message = { Value = "message" } };
        IUserAuthentication UserAuthentication { get; } = Substitute.For<IUserAuthentication>();
        ContentChangingEventHandler ContentChangingEventHandler { get; } = Substitute.For<ContentChangingEventHandler>();

        public LoginContentControllerSpec_LoginButtonClick()
        {
            Controller = new LoginContentController(UserAuthentication);
            LoginContent.ContentChanging += ContentChangingEventHandler;

            WpfController.SetDataContext(LoginContent, Controller);
        }

        [Example("When the user is authenticated")]
        void Ex01()
        {
            When("the valid user id and password are set", () =>
            {
                LoginContent.UserId.Value = "user";
                LoginContent.Password.Value = "password";

                UserAuthentication.Authenticate(LoginContent.UserId.Value, LoginContent.Password.Value)
                    .Returns(UserAuthenticationResult.Succeeded());
            });
            When("the login command is executed", () =>
                WpfController.CommandHandlersOf(Controller)
                    .GetBy(SimpleLoginCommands.Login.Name)
                    .With(SimpleLoginCommands.Login)
                    .RaiseExecuted(LoginContent)
            );
            Then("the ContentChanging event should be raised", () =>
                ContentChangingEventHandler.Received(1).Invoke(
                    LoginContent,
                    Arg.Is<ContentChangingEventArgs>(e => (e.NextContent as UserContent).Id == LoginContent.UserId.Value)
                )
            );
        }

        [Example("When the IUserAuthentication is not specified")]
        void Ex02()
        {
            Given("a controller to which the IUserAuthentication is not specified", () =>
            {
                Controller = new LoginContentController(null);
                WpfController.SetDataContext(LoginContent, Controller);
            });
            When("the login command is executed", () =>
                WpfController.CommandHandlersOf(Controller)
                    .GetBy(SimpleLoginCommands.Login.Name)
                    .With(SimpleLoginCommands.Login)
                    .RaiseExecuted(LoginContent)
            );
            Then("the ContentChanging event should not be raised", () =>
                ContentChangingEventHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<ContentChangingEventArgs>())
            );
            Then("the LoginNotAvailable message should be set", () => LoginContent.Message.Value == Resources.LoginNotAvailable);
        }

        [Example("When the login content is not valid")]
        void Ex03()
        {
            When("the invalid user id and password are set", () =>
            {
                LoginContent.UserId.Value = null;
                LoginContent.Password.Value = null;
            });
            When("the login command is executed", () =>
                WpfController.CommandHandlersOf(Controller)
                    .GetBy(SimpleLoginCommands.Login.Name)
                    .With(SimpleLoginCommands.Login)
                    .RaiseExecuted(LoginContent)
            );
            Then("the ContentChanging event should not be raised", () =>
                ContentChangingEventHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<ContentChangingEventArgs>())
            );
            Then("the message of the login content should be empty", () => LoginContent.Message.Value == string.Empty);
        }

        [Example("When the user is not authenticated")]
        void Ex04()
        {
            When("the no authenticated user id and password are set", () =>
            {
                LoginContent.UserId.Value = "user";
                LoginContent.Password.Value = "password";

                UserAuthentication.Authenticate(LoginContent.UserId.Value, LoginContent.Password.Value)
                    .Returns(UserAuthenticationResult.Failed());
            });
            When("the login command is executed", () =>
                WpfController.CommandHandlersOf(Controller)
                    .GetBy(SimpleLoginCommands.Login.Name)
                    .With(SimpleLoginCommands.Login)
                    .RaiseExecuted(LoginContent)
            );
            Then("the ContentChanging event should not be raised", () =>
                ContentChangingEventHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<ContentChangingEventArgs>())
            );
            Then("the LoginFailureMessage message should be set", () => LoginContent.Message.Value == Resources.LoginFailureMessage);
        }
    }
}
