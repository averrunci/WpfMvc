// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Charites.Windows.Mvc
{
    /// <summary>
    /// Provides the function to create a WPF controller.
    /// </summary>
    public interface IWpfControllerFactory
    {
        /// <summary>
        /// Creates a WPF controller.
        /// </summary>
        /// <param name="controllerType">The type of a WPF controller.</param>
        /// <returns>The new instance of a WPF controller.</returns>
        object Create(Type controllerType);
    }
}
