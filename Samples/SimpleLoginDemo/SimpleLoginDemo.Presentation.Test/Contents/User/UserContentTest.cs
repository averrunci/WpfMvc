// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using NUnit.Framework;

using Rhino.Mocks;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User
{
    [TestFixture]
    public class UserContentTest
    {
        [Test]
        public void RaisesLoggedOutEventWhenLogoutIsCalled()
        {
            var userContent = new UserContent("User");
            var handler = MockRepository.GenerateMock<EventHandler>();
            userContent.LoggedOut += handler;

            userContent.Logout();

            handler.AssertWasCalled(h => h.Invoke(userContent, EventArgs.Empty));
        }
    }
}
