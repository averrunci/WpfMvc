// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

namespace Fievus.Windows.Mvc
{
    /// <summary>
    /// Represents errors that occur when a data context injection is failed.
    /// </summary>
    public class DataContextInjectionException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextInjectionException"/> class.
        /// </summary>
        public DataContextInjectionException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextInjectionException"/> class
        /// with the specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataContextInjectionException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataContextInjectionException"/> class
        /// with the specified error message and reference to the inner exception that is the
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception.</param>
        public DataContextInjectionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
