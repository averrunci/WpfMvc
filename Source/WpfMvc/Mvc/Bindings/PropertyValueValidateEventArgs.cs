// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Fievus.Windows.Mvc.Bindings
{
    /// <summary>
    /// Represents the method that handles the <see cref="ObservableProperty{T}.PropertyValueValidate"/> event
    /// raised when a value of the property is validated.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">A <see cref="PropertyValueValidateEventArgs{T}"/> that contains the event data.</param>
    public delegate void PropertyValueValidateEventHandler<T>(object sender, PropertyValueValidateEventArgs<T> e);

    /// <summary>
    /// Provides data for the <see cref="ObservableProperty{T}.PropertyValueValidate"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the property value.</typeparam>
    public class PropertyValueValidateEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the value to validate.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets results of the validation.
        /// </summary>
        public IEnumerable<ValidationResult> Results { get { return results; } }
        private ICollection<ValidationResult> results = new Collection<ValidationResult>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyValueValidateEventArgs{T}"/> class
        /// with the specified value to validate.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        public PropertyValueValidateEventArgs(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Adds the validation result with the specified error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        public void Add(string errorMessage)
        {
            Add(new ValidationResult(errorMessage));
        }

        /// <summary>
        /// Adds the validation result with the specified error message and
        /// list of the member names that have validation errors.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="memberNames">The list of the member names that have validation errors.</param>
        public void Add(string errorMessage, IEnumerable<string> memberNames)
        {
            Add(new ValidationResult(errorMessage, memberNames));
        }

        /// <summary>
        /// Adds the specified result of the validation.
        /// </summary>
        /// <param name="result">The result of the validation.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="result"/> is <c>null</c>.
        /// </exception>
        public void Add(ValidationResult result)
        {
            if (result.RequireNonNull(nameof(result)) == ValidationResult.Success) { return; }

            results.Add(result);
        }

        /// <summary>
        /// Adds the specified results of the validation.
        /// </summary>
        /// <param name="results">The results of the validation.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="results"/> is <c>null</c>.
        /// </exception>
        public void AddRange(IEnumerable<ValidationResult> results)
        {
            results.RequireNonNull(nameof(results)).ForEach(Add);
        }
    }
}
