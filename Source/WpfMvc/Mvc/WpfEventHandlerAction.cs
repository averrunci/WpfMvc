// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;

namespace Charites.Windows.Mvc;

internal sealed class WpfEventHandlerAction : EventHandlerAction
{
    public WpfEventHandlerAction(MethodInfo method, object? target) : base(method, target)
    {
    }

    protected override bool HandleUnhandledException(Exception exc) => WpfController.HandleUnhandledException(exc);

    protected override IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary<Type, Func<object?>>? dependencyResolver)
        => dependencyResolver is null ? new WpfParameterDependencyResolver() : new WpfParameterDependencyResolver(dependencyResolver);
}