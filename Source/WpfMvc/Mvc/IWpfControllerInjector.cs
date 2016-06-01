// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Provides the function to inject dependency components to the specified WPF controller.
    /// </summary>
    public interface IWpfControllerInjector
    {
        /// <summary>
        /// Injects dependency components to the specified WPF controller.
        /// </summary>
        /// <param name="controller">The WPF controller injected dependency components.</param>
        void Inject(object controller);
    }
}
