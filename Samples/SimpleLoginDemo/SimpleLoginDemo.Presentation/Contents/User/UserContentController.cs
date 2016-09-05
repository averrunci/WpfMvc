// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows.Input;

using Fievus.Windows.Mvc;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User
{
    public class UserContentController
    {
        [DataContext]
        public UserContent Context { get; set; }

        [CommandHandler(CommandName = "Logout")]
        private void Logout(ExecutedRoutedEventArgs e)
        {
            Context.Logout();
        }
    }
}
