// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

[Specification("LoginContentController Spec")]
class LoginContentControllerSpec
{
    [Context]
    LoginContentControllerSpec_Loaded Loaded => default!;

    [Context]
    LoginContentControllerSpec_LoginCommandCanExecute LoginCommandCanExecute => default!;

    [Context]
    LoginContentControllerSpec_LoginButtonClick LoginButtonClick => default!;
}