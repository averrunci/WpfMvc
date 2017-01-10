// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Mvc
{
    internal sealed class SimpleWpfControllerFactory : IWpfControllerFactory
    {
        object IWpfControllerFactory.Create(Type controllerType)
            => Activator.CreateInstance(controllerType.RequireNonNull(nameof(controllerType)));
    }
}
