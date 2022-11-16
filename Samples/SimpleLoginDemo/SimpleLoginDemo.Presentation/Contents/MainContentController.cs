// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System.Windows;
using Charites.Windows.Mvc;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents;

[View(Key = nameof(MainContent))]
public class MainContentController : ControllerBase<MainContent>, IDisposable
{
    private readonly IContentNavigator navigator;

    public MainContentController(IContentNavigator navigator)
    {
        this.navigator = navigator;

        SubscribeToContentNavigatorEvent();
    }

    public void Dispose()
    {
        UnsubscribeFromContentNavigatorEvent();
    }

    private void SubscribeToContentNavigatorEvent()
    {
        navigator.Navigated += OnContentNavigated;
    }

    private void UnsubscribeFromContentNavigatorEvent()
    {
        navigator.Navigated -= OnContentNavigated;
    }

    private void Navigate(MainContent content, object navigatedContent)
    {
        content.Content.Value = navigatedContent;
    }

    private void NavigateToLoginContent()
    {
        navigator.NavigateTo(new LoginContent());
    }

    private void OnContentNavigated(object? sender, ContentNavigatedEventArgs e) => DataContext.IfPresent(e.Content, Navigate);

    [EventHandler(Event = nameof(FrameworkElement.Loaded))]
    private void Initialize() => NavigateToLoginContent();
}