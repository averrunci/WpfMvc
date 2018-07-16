// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Adds event handlers")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler
    {
        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToField AttributedToField { get; }

        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToProperty AttributedToProperty { get; }

        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler_AttributedToMethod AttributedToMethod { get; }
    }
}
