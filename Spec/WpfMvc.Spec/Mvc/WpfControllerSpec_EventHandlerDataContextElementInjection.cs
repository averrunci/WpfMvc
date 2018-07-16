// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Event handler, data context, and element injection")]
    class WpfControllerSpec_EventHandlerDataContextElementInjection
    {
        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection_AddEventHandler AddEventHandler { get; }

        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection_SetElement SetElement { get; }

        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection_SetDataContext SetDataContext { get; }
    }
}
