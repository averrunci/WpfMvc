// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Charites.Windows.Mvc
{
    internal sealed class SimpleWpfControllerFactory : IWpfControllerFactory
    {
        public object Create(Type controllerType)
            => Activator.CreateInstance(controllerType.RequireNonNull(nameof(controllerType)));
    }
}
