// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Specifies the target to inject an element
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ElementAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        public string Name { get; set; }
    }
}
