// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows.Controls;
using System.Windows.Input;

using Fievus.Windows.Mvc;

using Ninject;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Properties;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    public class LoginContentController
    {
        [Inject]
        public IUserAuthentication UserAuthentication { get; set; }

        [DataContext]
        public LoginContent Context { get; set; }

        [Element]
        public PasswordBox PasswordBox { get; set; }

        [RoutedEventHandler(RoutedEvent = "Loaded")]
        private void Initialize()
        {
            PasswordBox.PasswordChanged += (s, e) => { Context.Password.Value = PasswordBox.Password; };
        }

        [CommandHandler(CommandName = "Login")]
        private void CanExecuteLogin(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = !string.IsNullOrEmpty(Context.UserId.Value) && !string.IsNullOrEmpty(Context.Password.Value);
        }

        [CommandHandler(CommandName = "Login")]
        private void ExecuteLogin(ExecutedRoutedEventArgs e)
        {
            Context.Message.Value = string.Empty;

            if (UserAuthentication == null)
            {
                Context.Message.Value = Resources.LoginNotAvailable;
                return;
            }

            if (!Context.IsValid) { return; }

            var result = UserAuthentication.Authenticate(Context.UserId.Value, Context.Password.Value);
            if (result.Success)
            {
                Context.Login();
            }
            else
            {
                Context.Message.Value = Resources.LoginFailureMessage;
            }
        }
    }
}
