// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents
{
    public class ContentChangingEventArgs : EventArgs
    {
        public ILoginDemoContent NextContent { get; }

        public ContentChangingEventArgs(ILoginDemoContent nextContent)
        {
            NextContent = nextContent;
        }
    }

    public delegate void ContentChangingEventHandler(object sender, ContentChangingEventArgs e);
}
