// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System.Windows;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents;

[View(Key = nameof(MainContent))]
public class MainContentController : IDisposable
{
    private readonly IContentNavigator navigator;

    [DataContext]
    private MainContent? Content { get; set; }

    public MainContentController(IContentNavigator navigator)
    {
        this.navigator = navigator;

        SubscribeContentNavigatorEvent();
    }

    public void Dispose()
    {
        UnsubscribeContentNavigatorEvent();
    }

    private void SubscribeContentNavigatorEvent()
    {
        navigator.Navigated += OnContentNavigated;
    }

    private void UnsubscribeContentNavigatorEvent()
    {
        navigator.Navigated -= OnContentNavigated;
    }

    private void OnContentNavigated(object? sender, ContentNavigatedEventArgs e)
    {
        Content.IfPresent(x => x.Content.Value = e.Content);
    }

    [EventHandler(Event = nameof(FrameworkElement.Loaded))]
    private void Initialize()
    {
        navigator.NavigateTo(new LoginContent());
    }
}