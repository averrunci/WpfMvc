// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class DefaultCommandHandlerParameterFromDIResolver(Type targetParameterType, Func<object?> resolver) : EventHandlerParameterFromDIResolver(null)
{
    protected override object? CreateParameter(Type parameterType) => targetParameterType == parameterType ? resolver() : null;
}