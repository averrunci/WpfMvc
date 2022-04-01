// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Charites.Windows.Mvc;

/// <summary>
/// Represents an item of a command event handler.
/// </summary>
public sealed class CommandHandlerItem
{
    private readonly string commandName;
    private readonly ICommand? command;
    private readonly FrameworkElement? element;
    private readonly CommandBinding? commandBinding;

    private ExecutedRoutedEventHandler? executedHandler;
    private CanExecuteRoutedEventHandler? canExecuteHandler;
    private ExecutedRoutedEventHandler? previewExecutedHandler;
    private CanExecuteRoutedEventHandler? previewCanExecuteHandler;

    internal CommandHandlerItem(string commandName, ICommand? command, FrameworkElement? element)
    {
        this.commandName = commandName;
        this.command = command;
        this.element = element;
        if (command is not null) commandBinding = new CommandBinding(command);
    }

    /// <summary>
    /// Gets a value that indicates whether <see cref="CommandHandlerItem"/> has the specified command name.
    /// </summary>
    /// <param name="commandName">The name of the command.</param>
    /// <returns>
    /// <c>true</c> if <see cref="CommandHandlerItem"/> has the specified command name; otherwise, <c>false</c>.
    /// </returns>
    public bool Has(string commandName) => this.commandName == commandName;

    /// <summary>
    /// Gets a value that indicates whether <see cref="CommandHandlerItem"/> has the specified command name and element.
    /// </summary>
    /// <param name="commandName">The name of the command.</param>
    /// <param name="element">The element that binds the command.</param>
    /// <returns>
    /// <c>true</c> if <see cref="CommandHandlerItem"/> has the specified command name and element; otherwise, <c>false</c>.
    /// </returns>
    public bool Has(string commandName, FrameworkElement? element) => this.commandName == commandName && this.element == element;

    /// <summary>
    /// Applies the specified command event handler of the specified event name.
    /// </summary>
    /// <param name="eventName">The name of the event.</param>
    /// <param name="handler">The command event handler to apply.</param>
    public void Apply(string eventName, Delegate? handler)
    {
        switch (eventName)
        {
            case nameof(CommandBinding.Executed) when handler is ExecutedRoutedEventHandler executedRoutedEventHandler:
                executedHandler = executedRoutedEventHandler;
                if (commandBinding is not null) commandBinding.Executed += executedRoutedEventHandler;
                break;
            case nameof(CommandBinding.CanExecute) when handler is CanExecuteRoutedEventHandler canExecuteRoutedEventHandler:
                canExecuteHandler = canExecuteRoutedEventHandler;
                if (commandBinding is not null) commandBinding.CanExecute += canExecuteRoutedEventHandler;
                break;
            case nameof(CommandBinding.PreviewExecuted) when handler is ExecutedRoutedEventHandler previewExecutedRoutedEventHandler:
                previewExecutedHandler = previewExecutedRoutedEventHandler;
                if (commandBinding is not null) commandBinding.PreviewExecuted += previewExecutedRoutedEventHandler;
                break;
            case nameof(CommandBinding.PreviewCanExecute) when handler is CanExecuteRoutedEventHandler previewCanExecuteRoutedEventHandler:
                previewCanExecuteHandler = previewCanExecuteRoutedEventHandler;
                if (commandBinding is not null) commandBinding.PreviewCanExecute += previewCanExecuteRoutedEventHandler;
                break;
        }
    }

    /// <summary>
    /// Adds the command handler to the element.
    /// </summary>
    public void AddCommandHandler()
    {
        if (element is null || commandBinding is null) return;

        element.CommandBindings.Add(commandBinding);
    }

    /// <summary>
    /// Removes the command handler from the element.
    /// </summary>
    public void RemoveCommandHandler()
    {
        if (element is null || commandBinding is null) return;

        element.CommandBindings.Remove(commandBinding);
    }

    /// <summary>
    /// Raises the command Executed event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    public void RaiseExecuted(ICommand? command, object? sender, object? parameter)
    {
        if (executedHandler is null) return;

        Handle<ExecutedRoutedEventArgs>(executedHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command Executed event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolvers">The resolver to resolve dependencies of parameters.</param>
    public void RaiseExecuted(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolvers)
    {
        if (executedHandler is null) return;

        Handle<ExecutedRoutedEventArgs>(executedHandler, command, sender, parameter, dependencyResolvers);
    }

    /// <summary>
    /// Raises the command PreviewExecuted event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    public void RaisePreviewExecuted(ICommand? command, object? sender, object? parameter)
    {
        if (previewExecutedHandler is null) return;

        Handle<ExecutedRoutedEventArgs>(previewExecutedHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command PreviewExecuted event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    public void RaisePreviewExecuted(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        if (previewExecutedHandler is null) return;

        Handle<ExecutedRoutedEventArgs>(previewExecutedHandler, command, sender, parameter, dependencyResolver);
    }

    /// <summary>
    /// Raises the command CanExecute event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
    public CanExecuteRoutedEventArgs? RaiseCanExecute(ICommand? command, object? sender, object? parameter)
    {
        return canExecuteHandler is null ? null : Handle<CanExecuteRoutedEventArgs>(canExecuteHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command CanExecute event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">the resolver to resolve dependencies of parameters.</param>
    /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the CanExecute event.</returns>
    public CanExecuteRoutedEventArgs? RaiseCanExecute(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        return canExecuteHandler is null ? null : Handle<CanExecuteRoutedEventArgs>(canExecuteHandler, command, sender, parameter, dependencyResolver);
    }

    /// <summary>
    /// Raises the command PreviewCanExecute event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the PreviewCanExecute event.</returns>
    public CanExecuteRoutedEventArgs? RaisePreviewCanExecute(ICommand? command, object? sender, object? parameter)
    {
        return previewCanExecuteHandler is null ? null : Handle<CanExecuteRoutedEventArgs>(previewCanExecuteHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command PreviewCanExecute event using the specified parameter.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns><see cref="CanExecuteRoutedEventArgs"/> used to raise the PreviewCanExecute event.</returns>
    public CanExecuteRoutedEventArgs? RaisePreviewCanExecute(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        return previewCanExecuteHandler is null ? null : Handle<CanExecuteRoutedEventArgs>(previewCanExecuteHandler, command, sender, parameter, dependencyResolver);
    }

    /// <summary>
    /// Raises the command Executed event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task RaiseExecutedAsync(ICommand? command, object? sender, object? parameter)
    {
        if (executedHandler is null) return;

        await HandleAsync<ExecutedRoutedEventArgs>(executedHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command Executed event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task RaiseExecutedAsync(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        if (executedHandler is null) return;

        await HandleAsync<ExecutedRoutedEventArgs>(executedHandler, command, sender, parameter, dependencyResolver);
    }

    /// <summary>
    /// Raises the command PreviewExecuted event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task RaisePreviewExecutedAsync(ICommand? command, object? sender, object? parameter)
    {
        if (previewExecutedHandler is null) return;

        await HandleAsync<ExecutedRoutedEventArgs>(previewExecutedHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command PreviewExecuted event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>A task represents the asynchronous raise operation.</returns>
    public async Task RaisePreviewExecutedAsync(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        if (previewExecutedHandler is null) return;

        await HandleAsync<ExecutedRoutedEventArgs>(previewExecutedHandler, command, sender, parameter, dependencyResolver);
    }

    /// <summary>
    /// Raises the command CanExecute event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task<CanExecuteRoutedEventArgs?> RaiseCanExecuteAsync(ICommand? command, object? sender, object? parameter)
    {
        return canExecuteHandler is null ? null : await HandleAsync<CanExecuteRoutedEventArgs>(canExecuteHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command CanExecute event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task<CanExecuteRoutedEventArgs?> RaiseCanExecuteAsync(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        return canExecuteHandler is null ? null : await HandleAsync<CanExecuteRoutedEventArgs>(canExecuteHandler, command, sender, parameter, dependencyResolver);
    }

    /// <summary>
    /// Raises the command PreviewCanExecute event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task<CanExecuteRoutedEventArgs?> RaisePreviewCanExecuteAsync(ICommand? command, object? sender, object? parameter)
    {
        return previewCanExecuteHandler is null ? null : await HandleAsync<CanExecuteRoutedEventArgs>(previewCanExecuteHandler, command, sender, parameter);
    }

    /// <summary>
    /// Raises the command PreviewCanExecute event using the specified parameter asynchronously.
    /// </summary>
    /// <param name="command">The command that is executed.</param>
    /// <param name="sender">The object where the event handler is attached.</param>
    /// <param name="parameter">The parameter of the command.</param>
    /// <param name="dependencyResolver">The resolver to resolve dependencies of parameters.</param>
    /// <returns>A task that represents the asynchronous raise operation.</returns>
    public async Task<CanExecuteRoutedEventArgs?> RaisePreviewCanExecuteAsync(ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>> dependencyResolver)
    {
        return previewCanExecuteHandler is null ? null : await HandleAsync<CanExecuteRoutedEventArgs>(previewCanExecuteHandler, command, sender, parameter, dependencyResolver);
    }

    private T? Handle<T>(Delegate handler, ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>>? dependencyResolver = null) where T : RoutedEventArgs
    {
        var e = CreateEventArgs<T>(command, parameter);
        if (e is null) return null;

        Handle(handler, sender, e, dependencyResolver);

        return e;
    }

    private async Task<T?> HandleAsync<T>(Delegate handler, ICommand? command, object? sender, object? parameter, IDictionary<Type, Func<object?>>? dependencyResolver = null) where T : RoutedEventArgs
    {
        var e = CreateEventArgs<T>(command, parameter);
        if (e is null) return null;

        if (Handle(handler, sender, e, dependencyResolver) is Task task)
        {
            await task;
        }

        return e;
    }

    private object? Handle<T>(Delegate handler, object? sender, T e, IDictionary<Type, Func<object?>>? dependencyResolver = null) where T : RoutedEventArgs
        => handler.Target is WpfEventHandlerAction action ?
            action.Handle(sender, e, dependencyResolver) :
            handler.DynamicInvoke(CreateDependencyResolver(dependencyResolver).Resolve(handler.Method, sender, e));

    private T? CreateEventArgs<T>(ICommand? command, object? parameter) where T : RoutedEventArgs
    {
        if (command is null && this.command is null) return null;

        return Activator.CreateInstance(
            typeof(T), BindingFlags.NonPublic | BindingFlags.Instance,
            null, new[] { command ?? this.command, parameter }, null
        ) as T;
    }

    private IParameterDependencyResolver CreateDependencyResolver(IDictionary<Type, Func<object?>>? dependencyResolver)
        => dependencyResolver is null ? new WpfParameterDependencyResolver() : new WpfParameterDependencyResolver(dependencyResolver);
}