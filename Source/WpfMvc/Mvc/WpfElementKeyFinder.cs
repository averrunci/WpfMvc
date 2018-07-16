// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc
{
    internal sealed class WpfElementKeyFinder : IWpfElementKeyFinder
    {
        public string FindKey(FrameworkElement element) => WpfController.GetKey(element);
    }
}
