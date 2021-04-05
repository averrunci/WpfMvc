// Copyright (C) 2018-2021 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Properties;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    [View(Key = nameof(LoginContent))]
    public class LoginContentController
    {
        private readonly IContentNavigator navigator;
        private readonly IUserAuthentication userAuthentication;

        [DataContext]
        private LoginContent Content { get; set; }

        [Element]
        private PasswordBox PasswordBox { get; set; }

        public LoginContentController(IContentNavigator navigator, IUserAuthentication userAuthentication)
        {
            this.navigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            this.userAuthentication = userAuthentication;
        }

        [EventHandler(Event = nameof(FrameworkElement.Loaded))]
        private void Initialize()
        {
            PasswordBox.PasswordChanged += (s, e) => { Content.Password.Value = PasswordBox.Password; };
        }

        [CommandHandler(CommandName = nameof(SimpleLoginCommands.Login))]
        private void CanExecuteLogin(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(Content.UserId.Value) && !string.IsNullOrEmpty(Content.Password.Value);
        }

        [CommandHandler(CommandName = nameof(SimpleLoginCommands.Login))]
        private void ExecuteLogin(ExecutedRoutedEventArgs e)
        {
            Content.Message.Value = string.Empty;

            if (userAuthentication == null)
            {
                Content.Message.Value = Resources.LoginNotAvailable;
                return;
            }

            if (!Content.IsValid) return;
           
            var result = userAuthentication.Authenticate(Content.UserId.Value, Content.Password.Value);
            if (result.Success)
            {
                navigator.NavigateTo(new UserContent(Content.UserId.Value));
            }
            else
            {
                Content.Message.Value = Resources.LoginFailureMessage;
            }
        }
    }
}
