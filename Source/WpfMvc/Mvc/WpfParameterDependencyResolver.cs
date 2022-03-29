// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

internal sealed class WpfParameterDependencyResolver : ParameterDependencyResolver
{
    public WpfParameterDependencyResolver()
    {
    }

    public WpfParameterDependencyResolver(IDictionary<Type, Func<object?>> dependencyResolver) : base(dependencyResolver)
    {
    }

    protected override object? ResolveParameterFromDependency(ParameterInfo parameter)
    {
        return WpfController.ControllerFactory.Create(parameter.ParameterType) ?? base.ResolveParameterFromDependency(parameter);
    }
}