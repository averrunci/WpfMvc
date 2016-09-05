// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Fievus.Windows.Mvc;

using Ninject;

using Fievus.Windows.Samples.SimpleLoginDemo.Adapter;

namespace Fievus.Windows.Samples.SimpleLoginDemo
{
    public class SimpleLoginDemoInjector : IWpfControllerInjector
    {
        private readonly StandardKernel kernel = new StandardKernel(new SimpleLoginDemoModule());

        protected virtual void Inject(object controller)
        {
            kernel.Inject(controller);
        }

        void IWpfControllerInjector.Inject(object controller)
        {
            Inject(controller);
        }
    }
}
