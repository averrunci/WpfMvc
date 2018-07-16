// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.ComponentModel.DataAnnotations;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;
using Charites.Windows.Mvc.Bindings;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login
{
    public class LoginContent : ILoginDemoContent
    {
        public event ContentChangingEventHandler ContentChanging;
        public event EventHandler LoggedOut;

        [Display(Name = "User ID")]
        [Required]
        public ObservableProperty<string> UserId { get; } = string.Empty.ToObservableProperty();

        [Display(Name = "Password")]
        [Required]
        public ObservableProperty<string> Password { get; } = string.Empty.ToObservableProperty();

        public ObservableProperty<string> Message { get; } = string.Empty.ToObservableProperty();

        public bool IsValid
        {
            get
            {
                UserId.EnsureValidation();
                Password.EnsureValidation();
                return !UserId.HasErrors && !Password.HasErrors;
            }
        }

        public LoginContent()
        {
            UserId.EnableValidation(() => UserId);
            Password.EnableValidation(() => Password);
        }

        public void Login()
        {
            if (!IsValid) return;

            OnContentChanging(new ContentChangingEventArgs(new UserContent(UserId.Value)));
        }

        protected virtual void OnContentChanging(ContentChangingEventArgs e) => ContentChanging?.Invoke(this, e);
        protected virtual void OnLoggedOut(EventArgs e) => LoggedOut?.Invoke(this, e);
    }
}
