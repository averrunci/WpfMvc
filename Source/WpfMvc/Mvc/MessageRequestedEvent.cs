// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

/// <summary>
/// Raises a <see cref="FrameworkElements.MessageRequestedEvent"/> routed event.
/// </summary>
public class MessageRequestedEvent
{
    private RoutedEvent routedEvent = FrameworkElements.MessageRequestedEvent;
    private object? source;

    private readonly string message;
    private string caption = string.Empty;
    private MessageBoxButton button = MessageBoxButton.OK;
    private MessageBoxImage icon = MessageBoxImage.None;
    private MessageBoxResult defaultButton = MessageBoxResult.None;
    private MessageBoxOptions options = MessageBoxOptions.None;

    /// <summary>
    /// Initializes a new instance of the <see cref="MessageRequestedEvent"/> class
    /// with the specified message.
    /// </summary>
    /// <param name="message">The message to request.</param>
    protected MessageRequestedEvent(string message)
    {
        this.message = message;
    }

    /// <summary>
    /// Creates a new instance of the <see cref="MessageRequestedEvent"/> class
    /// with the specified message.
    /// </summary>
    /// <param name="message">The string that specifies the text to display.</param>
    /// <returns>A new instance of the <see cref="MessageRequestedEvent"/> class.</returns>
    public static MessageRequestedEvent WithMessage(string message) => new(message);

    /// <summary>
    /// Raises a <see cref="FrameworkElements.MessageRequestedEvent"/> routed event
    /// from the specified element.
    /// </summary>
    /// <param name="element">
    /// The element on which a <see cref="FrameworkElements.MessageRequestedEvent"/>
    /// routed event is raised.
    /// </param>
    /// <returns>
    /// The data for the raised <see cref="FrameworkElements.MessageRequestedEvent"/> routed event.
    /// </returns>
    public MessageRequestedEventArgs RaiseFrom(FrameworkElement element)
    {
        var args = new MessageRequestedEventArgs
        {
            RoutedEvent = routedEvent,
            Source = source,
            Message = message,
            Caption = caption,
            Button = button,
            Icon = icon,
            DefaultButton = defaultButton,
            Options = options,
        };
        element.RaiseEvent(args);
        return args;
    }

    /// <summary>
    /// Sets the <see cref="RoutedEvent"/> associated with the
    /// <see cref="RoutedEventArgs"/> instance.
    /// </summary>
    /// <param name="routedEvent">
    /// The <see cref="RoutedEvent"/> associated with the
    /// <see cref="RoutedEventArgs"/> instance.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent At(RoutedEvent routedEvent)
    {
        this.routedEvent = routedEvent;
        return this;
    }

    /// <summary>
    /// Sets the reference to the object that raised the event.
    /// </summary>
    /// <param name="source">The reference to the object that raised the event.</param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent From(object? source)
    {
        this.source = source;
        return this;
    }

    /// <summary>
    /// Sets the string that specifies the title bar caption to display.
    /// </summary>
    /// <param name="caption">The string that specifies the title bar caption to display.</param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent WithCaption(string caption)
    {
        this.caption = caption;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="MessageBoxButton.OK"/> value
    /// that is displayed on a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent OKButton() => With(MessageBoxButton.OK);

    /// <summary>
    /// Sets the <see cref="MessageBoxButton.OKCancel"/> value
    /// that is displayed on a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent OKCancelButton() => With(MessageBoxButton.OKCancel);

    /// <summary>
    /// Sets the <see cref="MessageBoxButton.YesNo"/> value
    /// that is displayed on a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent YesNoButton() => With(MessageBoxButton.YesNo);

    /// <summary>
    /// Sets the <see cref="MessageBoxButton.YesNoCancel"/> value
    /// that is displayed on a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent YesNoCancelButton() => With(MessageBoxButton.YesNoCancel);

    /// <summary>
    /// Sets the <see cref="MessageBoxButton"/> value that specifies
    /// which button or buttons to display.
    /// </summary>
    /// <param name="button">
    /// The <see cref="MessageBoxButton"/> value that specifies
    /// which button or buttons to display.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent With(MessageBoxButton button)
    {
        this.button = button;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Asterisk"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent AsteriskIcon() => With(MessageBoxImage.Asterisk);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Error"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent ErrorIcon() => With(MessageBoxImage.Error);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Exclamation"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent ExclamationIcon() => With(MessageBoxImage.Exclamation);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Hand"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent HandIcon() => With(MessageBoxImage.Hand);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Information"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent InformationIcon() => With(MessageBoxImage.Information);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Question"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent QuestionIcon() => With(MessageBoxImage.Question);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Stop"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent StopIcon() => With(MessageBoxImage.Stop);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage.Warning"/> value
    /// that is displayed by a message box.
    /// </summary>
    /// <returns></returns>
    public MessageRequestedEvent WarningIcon() => With(MessageBoxImage.Warning);

    /// <summary>
    /// Sets the <see cref="MessageBoxImage"/> value that specifies
    /// the icon to display.
    /// </summary>
    /// <param name="icon">
    /// The <see cref="MessageBoxImage"/> value that specifies
    /// the icon to display.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent With(MessageBoxImage icon)
    {
        this.icon = icon;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="MessageBoxResult.Cancel"/> value
    /// that is the default result of the message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent CancelAsDefaultButton() => With(MessageBoxResult.Cancel);

    /// <summary>
    /// Sets the <see cref="MessageBoxResult.No"/> value
    /// that is the default result of the message box.
    /// </summary>
    /// <returns></returns>
    public MessageRequestedEvent NoAsDefaultButton() => With(MessageBoxResult.No);

    /// <summary>
    /// Sets the <see cref="MessageBoxResult.OK"/> value
    /// that is the default value of the message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent OKAsDefaultButton() => With(MessageBoxResult.OK);

    /// <summary>
    /// Sets the <see cref="MessageBoxResult.Yes"/> value
    /// that is the default result of the message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent YesAsDefaultButton() => With(MessageBoxResult.Yes);

    /// <summary>
    /// Sets the <see cref="MessageBoxResult"/> value that specifies
    /// the default result of the message box.
    /// </summary>
    /// <param name="defaultButton">
    /// The <see cref="MessageBoxResult"/> value that specifies
    /// the default result of the message box.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent With(MessageBoxResult defaultButton)
    {
        this.defaultButton = defaultButton;
        return this;
    }

    /// <summary>
    /// Sets the <see cref="MessageBoxOptions.DefaultDesktopOnly"/> value
    /// that is the special display option for a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent DefaultDesktopOnly() => With(MessageBoxOptions.DefaultDesktopOnly);

    /// <summary>
    /// Sets the <see cref="MessageBoxOptions.RightAlign"/> value
    /// that is the special display option for a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent RightAlign() => With(MessageBoxOptions.RightAlign);

    /// <summary>
    /// Sets the <see cref="MessageBoxOptions.RtlReading"/> value
    /// that is the special display option for a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent RtlReading() => With(MessageBoxOptions.RtlReading);

    /// <summary>
    /// Sets the <see cref="MessageBoxOptions.ServiceNotification"/> value
    /// that is the special display option for a message box.
    /// </summary>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent ServiceNotification() => With(MessageBoxOptions.ServiceNotification);

    /// <summary>
    /// Sets the <see cref="MessageBoxOptions"/> value that specifies
    /// the options.
    /// </summary>
    /// <param name="options">
    /// The <see cref="MessageBoxOptions"/> value that specifies the options.
    /// </param>
    /// <returns>
    /// The instance of the <see cref="MessageRequestedEvent"/> class.
    /// </returns>
    public MessageRequestedEvent With(MessageBoxOptions options)
    {
        this.options = options;
        return this;
    }
}