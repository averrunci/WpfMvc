// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Adapter.User
{
    public class UserAuthentication : IUserAuthentication
    {
        protected virtual UserAuthenticationResult Authenticate(string userId, string password)
        {
            return userId == password ? UserAuthenticationResult.Succeeded() : UserAuthenticationResult.Failed();
        }

        UserAuthenticationResult IUserAuthentication.Authenticate(string userId, string password)
        {
            return Authenticate(userId, password);
        }
    }
}
