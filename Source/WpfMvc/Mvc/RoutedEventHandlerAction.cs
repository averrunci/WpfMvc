// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc;

internal class RoutedEventHandlerAction<T> where T : RoutedEventArgs
{
    private readonly MethodInfo method;
    private readonly object? target;

    public RoutedEventHandlerAction(MethodInfo method, object? target)
    {
        this.target = target;
        this.method = method;
    }

    public void OnHandled(object? sender, T e) => Handle(sender, e);

    public object? Handle(object? sender, T e)
    {
        return Handle(sender, e, null);
    }

    public object? Handle(object? sender, T e, IDictionary<Type, Func<object?>>? dependencyResolver)
    {
        return Handle(CreateParameterDependencyResolver(dependencyResolver).Resolve(method, sender, e));
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

    private IParameterDependencyResolver CreateParameterDependencyResolver(IDictionary<Type, Func<object?>>? dependencyResolver)
        => dependencyResolver is null ? new WpfParameterDependencyResolver() : new WpfParameterDependencyResolver(dependencyResolver);
}