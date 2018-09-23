// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Reflection;

namespace Charites.Windows.Mvc
{
    internal sealed class WpfEventHandlerAction : EventHandlerAction
    {
        public WpfEventHandlerAction(MethodInfo method, object target) : base(method ?? throw new ArgumentNullException(nameof(method)), target)
        {
        }

        protected override bool HandleUnhandledException(Exception exc) => WpfController.HandleUnhandledException(exc);
    }
}
