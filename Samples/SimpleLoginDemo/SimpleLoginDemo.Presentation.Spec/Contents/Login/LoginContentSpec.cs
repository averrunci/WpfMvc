// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    [Specification("LoginContent Spec")]
    class LoginContentSpec : FixtureSteppable
    {
        LoginContent LoginContent { get; } = new LoginContent();

        ContentChangingEventHandler ContentChangingEventHandler { get; } = Substitute.For<ContentChangingEventHandler>();

        public LoginContentSpec()
        {
            LoginContent.ContentChanging += ContentChangingEventHandler;
        }

        [Example("Validates the user id and the password")]
        [Sample(null, null, false, Description = "When the user id is null and the password is null")]
        [Sample("", "", false, Description = "When the user id is empty and the password is empty")]
        [Sample(null, "password", false, Description = "When the user id is null and the password is not null or empty")]
        [Sample("", "password", false, Description = "When the user id is empty and the password is not null or empty")]
        [Sample("user", null, false, Description = "When the user id is not null or empty and the password is null")]
        [Sample("user", "", false, Description = "When the user id is not null or empty and the password is empty")]
        [Sample("user", "password", true, Description = "when the user id is not null or empty and the password is not null or empty")]
        void Ex01(string userId, string password, bool expected)
        {
            When("the user id is set", () => LoginContent.UserId.Value = userId);
            When("the password is set", () => LoginContent.Password.Value = password);
            Then($"the validation result should be {expected}", () => LoginContent.IsValid == expected);
        }

        [Example("Logs in when the content is valid")]
        void Ex02()
        {
            When("the valid user id is set", () => LoginContent.UserId.Value = "user");
            When("the valid password is set", () => LoginContent.Password.Value = "password");
            When("to log in", () => LoginContent.Login());
            Then("the ContentChanging event should be raised", () =>
                ContentChangingEventHandler.Received(1).Invoke(
                    LoginContent,
                    Arg.Is<ContentChangingEventArgs>(e => (e.NextContent as UserContent).Id == LoginContent.UserId.Value)
                )
            );
        }

        [Example("Logs in when the content is invalid")]
        void Ex03()
        {
            When("the invalid user id is set", () => LoginContent.UserId.Value = null);
            When("the invalid password is set", () => LoginContent.Password.Value = null);
            When("to log in", () => LoginContent.Login());
            Then("the ContentChanging event should not be raised", () =>
                ContentChangingEventHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<ContentChangingEventArgs>())
            );
        }
    }
}
