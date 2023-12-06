// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Reflection;
using System.Windows;

namespace Charites.Windows.Mvc;

internal sealed class WpfControllerTypeFinder(
    IWpfElementKeyFinder elementKeyFinder,
    IWpfDataContextFinder dataContextFinder
) : ControllerTypeFinder<FrameworkElement>(elementKeyFinder, dataContextFinder), IWpfControllerTypeFinder
{
    protected override IEnumerable<Type> FindControllerTypeCandidates(FrameworkElement view)
        => controllerTypeCandidates ??= AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(t => t.GetTypeInfo().GetCustomAttributes<ViewAttribute>(true).Any())
            .ToList();
    private IEnumerable<Type>? controllerTypeCandidates;
}