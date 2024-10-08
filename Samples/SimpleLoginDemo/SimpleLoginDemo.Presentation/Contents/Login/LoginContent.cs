﻿// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.ComponentModel.DataAnnotations;
using Charites.Windows.Mvc.Bindings;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

public class LoginContent
{
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

    public bool CanExecute => !string.IsNullOrEmpty(UserId.Value) && !string.IsNullOrEmpty(Password.Value);

    public LoginContent()
    {
        UserId.EnableValidation(() => UserId);
        Password.EnableValidation(() => Password);
    }
}