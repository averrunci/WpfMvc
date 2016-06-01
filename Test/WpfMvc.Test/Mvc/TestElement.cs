// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows.Controls;

namespace Fievus.Windows.Mvc
{
    public class TestElement : ContentControl
    {
        public void RaiseInitialized()
        {
            OnInitialized(EventArgs.Empty);
        }
    }
}
