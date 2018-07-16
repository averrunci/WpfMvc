// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using System.Windows.Input;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleTodo.Contents
{
    [View(Key = nameof(MainContent))]
    public class MainContentController
    {
        [DataContext]
        private MainContent Content { get; set; }

        [EventHandler(ElementName = "TodoContentTextBox", Event = nameof(UIElement.KeyDown))]
        private void OnTodoContentTextBoxKeyDown(KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            Content.AddCurrentTodoContent();
        }
    }
}
