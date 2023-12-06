// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class DefaultCommandHandlerParameterFromDataContextResolver(object? dataContext) : EventHandlerParameterFromDataContextResolver(null)
{
    protected override object? FindDataContext() => dataContext;
}