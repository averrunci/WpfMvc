﻿// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Charites.Windows.Samples.SimpleLoginDemo.Presentation.Contents
{
    public interface ILoginDemoContent
    {
        event ContentChangingEventHandler ContentChanging;
        event EventHandler LoggedOut;
    }
}
