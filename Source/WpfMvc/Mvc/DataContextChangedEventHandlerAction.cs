// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc;

internal class DataContextChangedEventHandlerAction
{
    private readonly MethodInfo method;
    private readonly object? target;
    private readonly IParameterDependencyResolver parameterDependencyResolver;

    public DataContextChangedEventHandlerAction(MethodInfo method, object? target, IParameterDependencyResolver parameterDependencyResolver)
    {
        this.method = method;
        this.target = target;
        this.parameterDependencyResolver = parameterDependencyResolver;
    }

    public void OnHandled(object? sender, DependencyPropertyChangedEventArgs e) => Handle(sender, e);

    public object? Handle(object? sender, DependencyPropertyChangedEventArgs e)
    {
        return Handle(sender, e, parameterDependencyResolver);
    }

    public object? Handle(object? sender, DependencyPropertyChangedEventArgs e, IParameterDependencyResolver parameterDependency)
    {
        return Handle(parameterDependency.Resolve(method, sender, e));
    }

    private object? Handle(object?[] parameters)
    {
        try
        {
            var returnValue = method.Invoke(target, parameters);
            if (returnValue is Task task) Await(task);
            return returnValue;
        }
        catch (Exception exc)
        {
            if (!HandleUnhandledException(exc)) throw;

            return null;
        }
    }

    private async void Await(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception exc)
        {
            if (!HandleUnhandledException(exc)) throw;
        }
    }

    private bool HandleUnhandledException(Exception exc) => WpfController.HandleUnhandledException(exc);
}