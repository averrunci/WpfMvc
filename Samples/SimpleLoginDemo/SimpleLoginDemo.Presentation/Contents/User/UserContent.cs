// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Properties;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User;

public class UserContent(string id)
{
    public string Id { get; } = id;

    public string Message => string.Format(Resources.UserMessageFormat, Id);
}