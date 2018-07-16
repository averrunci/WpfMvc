// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;
using Charites.Windows.Mvc.Bindings;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents
{
    public class MainContent
    {
        public ObservableProperty<ILoginDemoContent> Content { get; } = new ObservableProperty<ILoginDemoContent>();

        public MainContent(ILoginDemoContent initialContent)
        {
            Content.PropertyValueChanged += (s, e) =>
            {
                if (e.OldValue != null)
                {
                    e.OldValue.ContentChanging -= LoginDemoContent_ContentChanging;
                    e.OldValue.LoggedOut -= LoginDemoContent_LoggedOut;
                }

                if (e.NewValue != null)
                {
                    e.NewValue.ContentChanging += LoginDemoContent_ContentChanging;
                    e.NewValue.LoggedOut += LoginDemoContent_LoggedOut;
                }
            };
            Content.Value = initialContent;
        }

        private void LoginDemoContent_ContentChanging(object sender, ContentChangingEventArgs e)
        {
            Content.Value = e.NextContent;
        }

        private void LoginDemoContent_LoggedOut(object sender, EventArgs e)
        {
            Content.Value = new LoginContent();
        }
    }
}
