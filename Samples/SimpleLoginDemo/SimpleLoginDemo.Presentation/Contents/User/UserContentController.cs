// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Input;

using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User
{
    [View(Key = nameof(UserContent))]
    public class UserContentController
    {
        [DataContext]
        private UserContent Context { get; set; }

        [CommandHandler(CommandName = nameof(SimpleLoginCommands.Logout))]
        private void Logout(ExecutedRoutedEventArgs e)
        {
            Context.Logout();
        }
    }
}
