// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Linq;
using System.Reflection;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Provides the function to inject a data context to a WPF controller.
    /// </summary>
    public class DataContextInjector : IDataContextInjector
    {
        /// <summary>
        /// Gets a BindingFlags for a data context.
        /// </summary>
        protected virtual BindingFlags DataContextBindingFlags { get; } = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// Injects the specified data context to fields of the specified WPF controller.
        /// </summary>
        /// <param name="dataContext">The data context that is injected to the WPF controller.</param>
        /// <param name="controller">The WPF controller to inject a data context.</param>
        protected virtual void InjectDataContextToField(object dataContext, object controller)
            => controller.GetType()
                .GetFields(DataContextBindingFlags)
                .Where(field => field.GetCustomAttribute<DataContextAttribute>(true) != null)
                .ForEach(field => field.SetValue(controller, dataContext));

        /// <summary>
        /// Injects the specified data context to properties of the specified WPF controller.
        /// </summary>
        /// <param name="dataContext">The data context that is injected to the WPF controller.</param>
        /// <param name="controller">The WPF controller to inject a data context.</param>
        protected virtual void InjectDataContextToProperty(object dataContext, object controller)
            => controller.GetType()
                .GetProperties(DataContextBindingFlags)
                .Where(property => property.GetCustomAttribute<DataContextAttribute>(true) != null)
                .ForEach(property => property.SetValue(controller, dataContext, null));

        /// <summary>
        /// Injects the specified data context to methods of the specified WPF controller.
        /// </summary>
        /// <param name="dataContext">The data context that is injected to the WPF controller.</param>
        /// <param name="controller">The WPF controller to inject a data context.</param>
        protected virtual void InjectDataContextToMethod(object dataContext, object controller)
            => controller.GetType()
                .GetMethods(DataContextBindingFlags)
                .Where(method => method.GetCustomAttribute<DataContextAttribute>(true) != null)
                .ForEach(method => method.Invoke(controller, new object[] { dataContext }));

        void IDataContextInjector.Inject(object dataContext, object controller)
        {
            if (controller == null) { return; }

            InjectDataContextToField(dataContext, controller);
            InjectDataContextToProperty(dataContext, controller);
            InjectDataContextToMethod(dataContext, controller);
        }
    }
}
