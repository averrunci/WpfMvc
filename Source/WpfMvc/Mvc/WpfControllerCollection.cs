// Copyright (C) 2016-2017 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Represents a collection of WPF controller objects.
    /// </summary>
    public sealed class WpfControllerCollection : Collection<object>
    {
        /// <summary>
        /// Gets the element to which WPF controllers are attached.
        /// </summary>
        public FrameworkElement AssociatedElement { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfControllerCollection"/> class.
        /// </summary>
        public WpfControllerCollection()
        {
        }

        /// <summary>
        /// Attaches WPF controllers to the specified element.
        /// </summary>
        /// <param name="element">The element to which WPF controllers are attached.</param>
        public void AttachTo(FrameworkElement element)
        {
            if (element == AssociatedElement) { return; }
            if (AssociatedElement != null) { throw new InvalidOperationException("Assosiated element must be null."); }

            AssociatedElement = element;
            this.ForEach(controller => WpfController.Attach(controller, element));
        }
        
        /// <summary>
        /// Detaches WPF controllers from the element to which WPF controllers are attached.
        /// </summary>
        public void Detach()
        {
            this.ForEach(controller => WpfController.Detach(controller, AssociatedElement));
            AssociatedElement = null;
        }

        /// <summary>
        /// Removes all elements of the <see cref="WpfControllerCollection"/>.
        /// </summary>
        protected override void ClearItems()
        {
            Detach();

            base.ClearItems();
        }

        /// <summary>
        /// Inserts an element into the <see cref="WpfControllerCollection"/> at the specified index.
        /// </summary>
        /// <param name="index">
        /// The zero-based index at which <paramref name="item"/> should be inserted.
        /// </param>
        /// <param name="item">
        /// The object to insert. The value can be <c>null</c> for reference types.
        /// </param>
        protected override void InsertItem(int index, object item)
        {
            var controller = item;
            (controller as IWpfControllerFactory).IfPresent(factory => controller = factory.Create(null));
            (controller as WpfController).IfPresent(wpfController => controller = wpfController.Create());

            base.InsertItem(index, controller);

            if (AssociatedElement == null) { return; }
            WpfController.Attach(controller, AssociatedElement);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="WpfControllerCollection"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected override void RemoveItem(int index)
        {
            WpfController.Detach(this[index], AssociatedElement);

            base.RemoveItem(index);
        }
    }
}
