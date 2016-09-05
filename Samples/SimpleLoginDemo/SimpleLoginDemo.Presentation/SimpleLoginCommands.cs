// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows.Input;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Properties;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation
{
    public static class SimpleLoginCommands
    {
        public static readonly RoutedUICommand Login = new RoutedUICommand(Resources.LoginCommandText, "Login", typeof(SimpleLoginCommands));
        public static readonly RoutedUICommand Logout = new RoutedUICommand(Resources.LogoutCommandText, "Logout", typeof(SimpleLoginCommands));
    }
}
