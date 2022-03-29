using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc;

internal class DataContextChangedEventHandlerAction
{
    private readonly MethodInfo method;
    private readonly object? target;

    public DataContextChangedEventHandlerAction(MethodInfo method, object? target)
    {
        this.method = method;
        this.target = target;
    }

    public void OnHandled(object? sender, DependencyPropertyChangedEventArgs e) => Handle(sender, e);

    public object? Handle(object? sender, DependencyPropertyChangedEventArgs e)
    {
        return Handle(sender, e, null);
    }

    public object? Handle(object? sender, DependencyPropertyChangedEventArgs e, IDictionary<Type, Func<object?>>? dependencyResolver)
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