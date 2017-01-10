// Copyright (C) 2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Fievus.Windows.Mvc;

using Ninject;

using Fievus.Windows.Samples.SimpleLoginDemo.Adapter;

namespace Fievus.Windows.Samples.SimpleLoginDemo
{
    public class SimpleLoginDemoControllerFactory : IWpfControllerFactory
    {
        private static StandardKernel Kernel { get; } = new StandardKernel(new SimpleLoginDemoModule());

        protected virtual object Create(Type controllerType) => Kernel.Get(controllerType);
        object IWpfControllerFactory.Create(Type controllerType) => Create(controllerType);
    }
}
