// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Provides the function to inject elements in an element to a WPF controller.
    /// </summary>
    public class ElementInjector : IElementInjector
    {
        /// <summary>
        /// Gets a BindingFlags for an element.
        /// </summary>
        protected virtual BindingFlags ElementBindingFlags { get; } = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Injects elements in the specified element to fields of the specified WPF controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements that are injected to the WPF controller.</param>
        /// <param name="controller">The WPF controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the WPF controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void InjectElementToField(FrameworkElement rootElement, object controller, bool foundElementOnly)
            => controller.GetType()
                .GetFields(ElementBindingFlags)
                .Select(Field => new { Field, Attribute = Field.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t =>
                {
                    var elementName = t.Attribute.Name ?? t.Field.Name;
                    var element = FindElement<object>(rootElement, elementName);
                    if (!foundElementOnly || element != null)
                    {
                        SetElement(elementName, controller, () => t.Field.SetValue(controller, element));
                    }
                });

        /// <summary>
        /// Injects elements in the specified element to properties of the specified WPF controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements that are injected to the WPF controller.</param>
        /// <param name="controller">The WPF controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the WPF controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void InjectElementToProperty(FrameworkElement rootElement, object controller, bool foundElementOnly)
        {
            controller.GetType()
                .GetProperties(ElementBindingFlags)
                .Select(Property => new { Property, Attribute = Property.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t =>
                {
                    var elementName = t.Attribute.Name ?? t.Property.Name;
                    var element = FindElement<object>(rootElement, elementName);
                    if (!foundElementOnly || element != null)
                    {
                        SetElement(elementName, controller, () => t.Property.SetValue(controller, element, null));
                    }
                });
        }

        /// <summary>
        /// Injects elements in the specified element to methods of the specified WPF controller.
        /// </summary>
        /// <param name="rootElement">The element that contains elements that are injected to the WPF controller.</param>
        /// <param name="controller">The WPF controller to inject elements.</param>
        /// <param name="foundElementOnly">
        /// If <c>true</c>, an element is not set to the WPF controller when it is not found in the specified element;
        /// otherwise, <c>null</c> is set.
        /// </param>
        protected virtual void InjectElementToMethod(FrameworkElement rootElement, object controller, bool foundElementOnly)
        {
            controller.GetType()
                .GetMethods(ElementBindingFlags)
                .Select(Method => new { Method, Attribute = Method.GetCustomAttribute<ElementAttribute>(true) })
                .Where(t => t.Attribute != null)
                .ForEach(t =>
                {
                    var elementName = ResolveElementMethodName(t.Method, t.Attribute);
                    var element = FindElement<object>(rootElement, elementName);
                    if (!foundElementOnly || element != null)
                    {
                        SetElement(elementName, controller, () => t.Method.Invoke(controller, new object[] { element }));
                    }
                });
        }

        /// <summary>
        /// Finds an element of the specified name in the specified element.
        /// </summary>
        /// <typeparam name="E">The type of the requested element.</typeparam>
        /// <param name="rootElement">The element that has an element of the specified name.</param>
        /// <param name="elementName">The name of an element.</param>
        /// <returns>
        /// The element of the speicifed name in the specified element.
        /// If not found, <c>null</c> is returned.
        /// </returns>
        protected virtual E FindElement<E>(FrameworkElement rootElement, string elementName) where E : class
            => rootElement.FindElement<E>(elementName);

        /// <summary>
        /// Resolves an element name from a method to get it.
        /// </summary>
        /// <param name="m">The <see cref="MethodInfo"/> of a method to get an element.</param>
        /// <param name="a">The attribute of an element.</param>
        /// <returns>
        /// The element name to resolve from the specified method to get the element.
        /// </returns>
        protected virtual string ResolveElementMethodName(MethodInfo m, ElementAttribute a)
            => a.Name ?? (m.Name.StartsWith("Set") ? m.Name.Substring(3) : m.Name);

        /// <summary>
        /// Sets the element of the specified name using the specified action.
        /// </summary>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="controller">The WPF controller to inject the element.</param>
        /// <param name="action">The action that set the element.</param>
        protected void SetElement(string elementName, object controller, Action action)
        {
            try
            {
                action();
            }
            catch (Exception exc)
            {
                throw new ElementInjectionException($"The injetion of {elementName} to {controller.GetType()} is failed.", exc);
            }
        }

        void IElementInjector.Inject(FrameworkElement rootElement, object controller, bool foundElementOnly)
        {
            if (controller == null) { return; }

            InjectElementToField(rootElement, controller, foundElementOnly);
            InjectElementToProperty(rootElement, controller, foundElementOnly);
            InjectElementToMethod(rootElement, controller, foundElementOnly);
        }
    }
}
