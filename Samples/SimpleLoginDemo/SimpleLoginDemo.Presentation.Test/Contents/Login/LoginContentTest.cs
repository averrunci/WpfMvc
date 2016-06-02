// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    [TestFixture]
    public class LoginContentTest
    {
        [Test]
        public void IsNotValidWhenUserIdAndPasswordAreNull()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = null;
            loginContent.Password.Value = null;

            Assert.That(loginContent.IsValid, Is.False);
        }

        [Test]
        public void IsNotValidWhenUserIdAndPasswordAreEmpty()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = string.Empty;
            loginContent.Password.Value = string.Empty;

            Assert.That(loginContent.IsValid, Is.False);
        }

        [Test]
        public void IsNotValidWhenUserIdIsNullAndPasswordIsNotNullOrEmpty()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = null;
            loginContent.Password.Value = "password";

            Assert.That(loginContent.IsValid, Is.False);
        }

        [Test]
        public void IsNotValidWhenUserIdIsEmptyAndPasswordIsNotNullOrEmpty()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = string.Empty;
            loginContent.Password.Value = "password";

            Assert.That(loginContent.IsValid, Is.False);
        }

        [Test]
        public void IsNotValidWhenUserIdIsNotNullOrEmptyAndPasswordIsNull()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = "user";
            loginContent.Password.Value = null;

            Assert.That(loginContent.IsValid, Is.False);
        }

        [Test]
        public void IsNotValidWhenUserIdIsNotNullOrEmptyAndPasswordIsEmpty()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = "user";
            loginContent.Password.Value = string.Empty;

            Assert.That(loginContent.IsValid, Is.False);
        }

        [Test]
        public void IsValidWhenUserIdIsNotNullOrEmptyAndPasswordIsNotNullOrEmpty()
        {
            var loginContent = new LoginContent();

            loginContent.UserId.Value = "user";
            loginContent.Password.Value = "password";

            Assert.That(loginContent.IsValid, Is.True);
        }

        [Test]
        public void RaisesContentChangingEventWithUserContentOfUserIdWhenLoginIsCalled()
        {
            var loginContent = new LoginContent();
            loginContent.UserId.Value = "user";
            loginContent.Password.Value = "password";

            var handler = MockRepository.GenerateMock<ContentChangingEventHandler>();
            loginContent.ContentChanging += handler;

            loginContent.Login();

            handler.AssertWasCalled(
                h => h.Invoke(
                    Arg<object>.Is.Equal(loginContent),
                    Arg<ContentChangingEventArgs>.Matches(e => ((UserContent)e.NextContent).Id == "user")
                )
            );
        }

        [Test]
        public void DoesNotRaiseContentChangingEventWhenContentIsNotValid()
        {
            var loginContent = new LoginContent();

            var handler = MockRepository.GenerateMock<ContentChangingEventHandler>();
            loginContent.ContentChanging += handler;

            loginContent.Login();

            handler.AssertWasNotCalled(h => h.Invoke(Arg<object>.Is.Anything, Arg<ContentChangingEventArgs>.Is.Anything));
        }
    }
}
