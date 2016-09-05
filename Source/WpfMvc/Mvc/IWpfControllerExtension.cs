// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Provides an function that is added to a WPF controller.
    /// </summary>
    public interface IWpfControllerExtension
    {
        /// <summary>
        /// Attaches an extension that is defined in the specified controller to the specified element.
        /// </summary>
        /// <param name="element">The element to which an extension is attached.</param>
        /// <param name="controller">The WPF controller in which an extension is deinfed.</param>
        void Attach(FrameworkElement element, object controller);

        /// <summary>
        /// Detaches an extension that is defined in the specified controller from the specified element.
        /// </summary>
        /// <param name="element">The element from which an extension is detached.</param>
        /// <param name="controller">The WPF controller in which an extension is defined.</param>
        void Detach(FrameworkElement element, object controller);

        /// <summary>
        /// Retrieves an extension that is defined in the specified controller.
        /// </summary>
        /// <param name="controller">The WPF controller in which an extension is defined.</param>
        /// <returns>The container of the retrieved extension.</returns>
        object Retrieve(object controller);
    }
}
