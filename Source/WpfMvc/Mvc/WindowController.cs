﻿// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using System.Windows.Media;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Handles the <see cref="FrameworkElements.MessageRequestedEvent"/> and
    /// <see cref="FrameworkElements.WindowRequestedEvent"/> routed events.
    /// </summary>
    public sealed class WindowController
    {
        [RoutedEventHandler(RoutedEvent = "Loaded")]
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) { return; }

            element.AddHandler(FrameworkElements.MessageRequestedEvent, new MessageRequestedEventHandler(OnMessageRequested));
            element.AddHandler(FrameworkElements.WindowRequestedEvent, new WindowRequestedEventHandler(OnWindowRequested));
        }

        [RoutedEventHandler(RoutedEvent = "Unloaded")]
        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) { return; }

            element.RemoveHandler(FrameworkElements.MessageRequestedEvent, new MessageRequestedEventHandler(OnMessageRequested));
            element.RemoveHandler(FrameworkElements.WindowRequestedEvent, new WindowRequestedEventHandler(OnWindowRequested));
        }

        private void OnMessageRequested(object sender, MessageRequestedEventArgs e)
        {
            var ownerWindow = FindWindowFrom(sender);
            if (ownerWindow == null) { return; }

            e.Result = MessageBox.Show(ownerWindow, e.Message, e.Caption, e.Button, e.Icon, e.DefaultButton, e.Options);
        }

        private void OnWindowRequested(object sender, WindowRequestedEventArgs e)
        {
            var ownerWindow = FindWindowFrom(sender);

            var window = Activator.CreateInstance(e.WindowType ?? (ownerWindow == null ? typeof(Window) : ownerWindow.GetType())) as Window;
            if (window == null) { throw new InvalidOperationException(); }

            window.Content = e.Content;
            window.DataContext = e.DataContext;
            if (e.OwnedWindow) { window.Owner = ownerWindow; }
            window.Style = e.Style;
            window.WindowStartupLocation = e.WindowStartupLocation;
            if (window.WindowStartupLocation == WindowStartupLocation.Manual)
            {
                window.Left = e.Location.X;
                window.Top = e.Location.Y;
            }

            e.WindowCreated?.Invoke(window);
            if (e.Modal)
            {
                e.Result = window.ShowDialog();
            }
            else
            {
                window.Show();
            }
        }

        private Window FindWindowFrom(object sender)
        {
            var element = sender as DependencyObject;
            while (element != null)
            {
                var ownerWindow = element as Window;
                if (ownerWindow != null) { return ownerWindow; }

                element = VisualTreeHelper.GetParent(element);
            }

            return null;
        }
    }
}
