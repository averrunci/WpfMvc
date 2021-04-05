// Copyright (C) 2018-2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using Carna;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;
using NSubstitute;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User
{
    [Specification("UserContentController Spec")]
    class UserContentControllerSpec : FixtureSteppable
    {
        UserContentController Controller { get; }

        UserContent UserContent { get; } = Substitute.For<UserContent>("User");
        IContentNavigator Navigator { get; } = Substitute.For<IContentNavigator>();

        public UserContentControllerSpec()
        {
            Controller = new UserContentController(Navigator);

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
            Then("the content should be navigated to the LoginContent", () =>
            {
                Navigator.Received(1).NavigateTo(Arg.Any<LoginContent>());
            });
        }

        [Example("When the IContentNavigator is not specified")]
        void Ex02()
        {
            When("a controller to which the IContentNavigator is not specified is created", () => new UserContentController(null));
            Then<ArgumentNullException>($"{typeof(ArgumentNullException)} should be thrown");
        }
    }
}
