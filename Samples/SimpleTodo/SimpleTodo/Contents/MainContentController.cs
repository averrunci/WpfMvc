// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Input;
using Charites.Windows.Mvc;

namespace Charites.Windows.Samples.SimpleTodo.Contents;

[View(Key = nameof(MainContent))]
public class MainContentController
{
    [DataContext]
    private MainContent? Content { get; set; }

    private void TodoContentTextBox_KeyDown(KeyEventArgs e)
    {
        if (e.Key != Key.Enter) return;

        Content?.AddCurrentTodoContent();
    }
}