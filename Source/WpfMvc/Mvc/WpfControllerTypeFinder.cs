// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfControllerTypeFinder : ControllerTypeFinder<FrameworkElement>, IWpfControllerTypeFinder
{
    public WpfControllerTypeFinder(IWpfElementKeyFinder elementKeyFinder, IWpfDataContextFinder dataContextFinder) : base(elementKeyFinder, dataContextFinder)
    {
    }

    protected override IEnumerable<Type> FindControllerTypeCandidates(FrameworkElement view)
        => AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes());
}