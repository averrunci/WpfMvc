// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Mvc;
using Fievus.Windows.Runners;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Properties;
using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login.LoginContentControllerTest
{
    [TestFixture]
    public class WhenRootElementIsLoaded
    {
        [Test]
        public void AddsPasswordChangedHandlerThatSetsPasswordOfPasswordBoxToLoginContentToPasswordBox()
        {
            WpfApplicationRunner.Start<Application>().Run(application =>
            {
                var passwordBox = new PasswordBox();
                var loginContent = new LoginContent();
                var controller = new LoginContentController { PasswordBox = passwordBox, Context = loginContent };

                WpfController.RetrieveRoutedEventHandlers(controller)
                    .GetBy(null)
                    .Raise("Loaded");
                passwordBox.Password = "password";

                Assert.That(loginContent.Password.Value, Is.EqualTo(passwordBox.Password));
            });
        }
    }

    [TestFixture]
    public class WhenLoginButtonIsClicked
    {
        [Test]
        public void LoginContext_LoginIsCalledWhenUserIsAuthenticated()
        {
            var loginContent = new LoginContent();
            loginContent.UserId.Value = "user";
            loginContent.Password.Value = "password";
            var handler = MockRepository.GenerateMock<ContentChangingEventHandler>();
            loginContent.ContentChanging += handler;

            var userAuthentication = MockRepository.GenerateStub<IUserAuthentication>();
            userAuthentication.Expect(u => u.Authenticate(loginContent.UserId.Value, loginContent.Password.Value)).Return(UserAuthenticationResult.Succeeded());

            var controller = new LoginContentController { Context = loginContent, UserAuthentication = userAuthentication };

            WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("LoginButton")
                .Raise("Click");

            handler.AssertWasCalled(
                h => h.Invoke(
                    Arg<object>.Is.Equal(loginContent),
                    Arg<ContentChangingEventArgs>.Matches(e => ((UserContent)e.NextContent).Id == loginContent.UserId.Value)
                )
            );
        }

        [Test]
        public void GetsLoginNotAvailableMessageWhenUserAuthenticationInterfaceIsNotSpecified()
        {
            var loginContent = new LoginContent();
            var controller = new LoginContentController { Context = loginContent };

            WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("LoginButton")
                .Raise("Click");

            Assert.That(loginContent.Message.Value, Is.EqualTo(Resources.LoginNotAvailable));
        }

        [Test]
        public void ClearsMessageWhenLoginContentIsNotValid()
        {
            var loginContent = new LoginContent();
            loginContent.Message.Value = "message";
            var userAuthentication = MockRepository.GenerateStub<IUserAuthentication>();
            var controller = new LoginContentController { Context = loginContent, UserAuthentication = userAuthentication };

            WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("LoginButton")
                .Raise("Click");

            Assert.That(loginContent.Message.Value, Is.Empty);
        }

        [Test]
        public void GetsLoginFailureMessageWhenUserAuthenticationIsFailure()
        {
            var loginContent = new LoginContent();
            loginContent.Message.Value = "message";
            loginContent.UserId.Value = "user";
            loginContent.Password.Value = "password";
            var userAuthentication = MockRepository.GenerateStub<IUserAuthentication>();
            userAuthentication.Expect(u => u.Authenticate(loginContent.UserId.Value, loginContent.Password.Value)).Return(UserAuthenticationResult.Failed());
            var controller = new LoginContentController { Context = loginContent, UserAuthentication = userAuthentication };

            WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("LoginButton")
                .Raise("Click");

            Assert.That(loginContent.Message.Value, Is.EqualTo(Resources.LoginFailureMessage));
        }
    }
}
