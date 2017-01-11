// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Fievus.Windows.Mvc;
using System.Windows.Input;

namespace Fievus.Windows.Samples.SimpleTodo.Contents
{
    public class MainContentController
    {
        [DataContext]
        public MainContent Content { get; set; }

        [RoutedEventHandler(ElementName = "TodoContentTextBox", RoutedEvent = "KeyDown")]
        private void OnTodoContentTextBoxKeyDown(KeyEventArgs e)
        {
            if (e.Key != Key.Enter) { return; }

            Content.AddCurrentTodoContent();
        }
    }
}
