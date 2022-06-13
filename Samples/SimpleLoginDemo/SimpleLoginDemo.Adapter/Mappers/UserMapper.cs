// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Charites.Windows.Samples.SimpleLoginDemo.Core.Features.Users;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Charites.Windows.Samples.SimpleLoginDemo.Adapter.Mappers;

internal static class UserMapper
{
    public static User ToUser(this LoginContent @this)
        => new()
        {
            UserId = @this.UserId.Value,
            Password = @this.Password.Value
        };
}