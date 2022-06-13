// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Properties;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[View(Key = nameof(LoginContent))]
public class LoginContentController
{
    [DataContext]
    private LoginContent? Content { get; set; }

    [Element]
    private void SetPasswordBox(PasswordBox? passwordBox)
    {
        this.passwordBox.IfPresent(UnsubscribePasswordBoxEvent);
        this.passwordBox = passwordBox;
        this.passwordBox.IfPresent(SubscribePasswordBoxEvent);
    }
    private PasswordBox? passwordBox;

    private void SubscribePasswordBoxEvent(PasswordBox passwordBox)
    {
        passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
    }

    private void UnsubscribePasswordBoxEvent(PasswordBox passwordBox)
    {
        passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        Content.IfPresent(x => x.Password.Value = passwordBox?.Password ?? string.Empty);
    }

    private void Login_CanExecute(CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = !string.IsNullOrEmpty(Content?.UserId.Value) && !string.IsNullOrEmpty(Content?.Password.Value);
    }

    private async Task Login_ExecutedAsync([FromDI] ILoginCommand loginCommand, [FromDI] IContentNavigator navigator)
    {
        if (Content is null) return;

        Content.Message.Value = string.Empty;

        if (!Content.IsValid) return;

        var currentContent = Content;
        var result = await loginCommand.AuthenticateAsync(currentContent);
        if (result.Success)
        {
            navigator.NavigateTo(new UserContent(currentContent.UserId.Value));
        }
        else
        {
            currentContent.Message.Value = Resources.LoginFailureMessage;
        }
    }
}