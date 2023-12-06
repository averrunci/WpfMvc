// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class DefaultCommandHandlerParameterFromElementResolver(string elementName, object? element) : EventHandlerParameterFromElementResolver(null)
{
    protected override object? FindElement(string name) => elementName == name ? element : null;
}