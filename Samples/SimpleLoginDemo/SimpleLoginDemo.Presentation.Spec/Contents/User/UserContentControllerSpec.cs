// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;
using Charites.Windows.Mvc;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User
{
    [Specification("UserContentController Spec")]
    class UserContentControllerSpec : FixtureSteppable
    {
        UserContentController Controller { get; } = new UserContentController();

        UserContent UserContent { get; } = Substitute.For<UserContent>("User");

        public UserContentControllerSpec()
        {
            WpfController.SetDataContext(UserContent, Controller);
        }

        [Example("Logs the user out")]
        void Ex01()
        {
            When("the Logout command is executed", () =>
                WpfController.CommandHandlersOf(Controller)
                    .GetBy(SimpleLoginCommands.Logout.Name)
                    .With(SimpleLoginCommands.Logout)
                    .RaiseExecuted(UserContent)
            );
            Then("the Logout of the UserContent is called", () => UserContent.Received().Logout());
        }
    }
}
