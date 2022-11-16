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
public class LoginContentController : ControllerBase<LoginContent>
{
    private void SetPasswordValue(LoginContent content, PasswordBox passwordBox)
    {
        content.Password.Value = passwordBox.Password;
    }

    private void SetCanExecute(LoginContent content, CanExecuteRoutedEventArgs e)
    {
        e.CanExecute = content.CanExecute;
    }

    private async Task LoginAsync(LoginContent content, ILoginCommand loginCommand, IContentNavigator navigator)
    {
        content.Message.Value = string.Empty;

        if (!content.IsValid) return;

        var result = await loginCommand.AuthenticateAsync(content);
        if (result.Success)
        {
            navigator.NavigateTo(new UserContent(content.UserId.Value));
        }
        else
        {
            content.Message.Value = Resources.LoginFailureMessage;
        }
    }

    private void PasswordBox_PasswordChanged(object? sender, RoutedEventArgs e) => DataContext.IfPresent(sender as PasswordBox, SetPasswordValue);
    private void Login_CanExecute(CanExecuteRoutedEventArgs e) => DataContext.IfPresent(e, SetCanExecute);
    private Task Login_ExecutedAsync([FromDI] ILoginCommand loginCommand, [FromDI] IContentNavigator navigator) => DataContext.IfPresentAsync(loginCommand, navigator, LoginAsync);
}