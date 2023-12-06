// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[Specification(
    "LoginContentController Spec",
    typeof(LoginContentControllerSpec_PasswordBox_PasswordChanged),
    typeof(LoginContentControllerSpec_LoginCommandCanExecute),
    typeof(LoginContentControllerSpec_LoginButtonClick)
)]
class LoginContentControllerSpec;