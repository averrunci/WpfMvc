// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows.Controls;

namespace Charites.Windows.Mvc;

public class TestElement : ContentControl
{
    public event EventHandler? Changed;

    public void RaiseInitialized() => OnInitialized(EventArgs.Empty);
    public void RaiseChanged() => Changed?.Invoke(this, EventArgs.Empty);
}