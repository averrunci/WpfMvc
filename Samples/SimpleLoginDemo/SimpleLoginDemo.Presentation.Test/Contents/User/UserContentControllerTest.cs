// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using NUnit.Framework;

using Rhino.Mocks;

using Fievus.Windows.Mvc;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User.UserContentControllerTest
{
    [TestFixture]
    public class WhenLogoutButtonIsClicked
    {
        [Test]
        public void ExecutesLogout()
        {
            var userContent = MockRepository.GenerateMock<UserContent>("User");
            var controller = new UserContentController { Context = userContent };

            WpfController.RetrieveRoutedEventHandlers(controller)
                .GetBy("LogoutButton")
                .Raise("Click");

            userContent.AssertWasCalled(u => u.Logout());
        }
    }
}
