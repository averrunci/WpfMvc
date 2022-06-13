// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

internal sealed class DefaultCommandHandlerParameterFromDIResolver : EventHandlerParameterFromDIResolver
{
    private readonly Type parameterType;
    private readonly Func<object?> resolver;

    public DefaultCommandHandlerParameterFromDIResolver(Type parameterType, Func<object?> resolver) : base(null)
    {
        this.parameterType = parameterType;
        this.resolver = resolver;
    }

    protected override object? CreateParameter(Type parameterType) => this.parameterType == parameterType ? resolver() : null;
}