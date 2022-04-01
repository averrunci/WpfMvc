// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Specifies the target to inject a command handler.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
public class CommandHandlerAttribute : Attribute
{
    /// <summary>
    /// Gets or sets the name of the command.
    /// </summary>
    public string CommandName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the event which is raised on the command
    /// </summary>
    public string Event { get; set; } = string.Empty;
}