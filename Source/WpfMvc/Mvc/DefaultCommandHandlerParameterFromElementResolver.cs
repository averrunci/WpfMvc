// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class DefaultCommandHandlerParameterFromElementResolver : EventHandlerParameterFromElementResolver
{
    private readonly string elementName;
    private readonly object? element;

    public DefaultCommandHandlerParameterFromElementResolver(string elementName, object? element) : base(null)
    {
        this.elementName = elementName;
        this.element = element;
    }

    protected override object? FindElement(string name) => elementName == name ? element : null;
}