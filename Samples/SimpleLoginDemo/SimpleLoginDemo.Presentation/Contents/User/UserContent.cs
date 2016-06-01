// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Properties;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.User
{
    public class UserContent : ILoginDemoContent
    {
        public event ContentChangingEventHandler ContentChanging;
        public event EventHandler LoggedOut;

        public string Id { get; }

        public string Message { get { return string.Format(Resources.UserMessageFormat, Id); } }

        public UserContent(string id)
        {
            Id = id;
        }

        public void Logout()
        {
            OnLoggedOut(EventArgs.Empty);
        }

        protected virtual void OnContentChanging(ContentChangingEventArgs e) => ContentChanging?.Invoke(this, e);
        protected virtual void OnLoggedOut(EventArgs e) => LoggedOut?.Invoke(this, e);
    }
}
