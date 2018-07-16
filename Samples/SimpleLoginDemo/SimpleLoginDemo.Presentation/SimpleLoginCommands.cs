// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Input;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Properties;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation
{
    public static class SimpleLoginCommands
    {
        public static readonly RoutedUICommand Login = new RoutedUICommand(Resources.LoginCommandText, nameof(Login), typeof(SimpleLoginCommands));
        public static readonly RoutedUICommand Logout = new RoutedUICommand(Resources.LogoutCommandText, nameof(Logout), typeof(SimpleLoginCommands));
    }
}
