// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("Command handler injection")]
    class WpfControllerSpec_CommandHandlerInjection
    {
        [Context]
        WpfControllerSpec_CommandHandlerInjection_AttributedToField AttributedToField { get; }

        [Context]
        WpfControllerSpec_CommandHandlerInjection_AttributedToProperty AttributedProperty { get; }

        [Context]
        WpfControllerSpec_CommandHandlerInjection_AttributedToMethod AttributedToMethod { get; }
    }
}
