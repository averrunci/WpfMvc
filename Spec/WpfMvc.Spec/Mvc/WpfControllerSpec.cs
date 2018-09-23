// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc
{
    [Specification("WpfController Spec")]
    class WpfControllerSpec
    {
        [Context]
        WpfControllerSpec_EventHandlerDataContextElementInjection EventHandlerDataContextElementInjection { get; }

        [Context]
        WpfControllerSpec_CommandHandlerInjection CommandHandlerInjection { get; }

        [Context]
        WpfControllerSpec_AttachingAndDetachingController AttachingAndDetachingController { get; }

        [Context]
        WpfControllerSpec_ExecuteHandler ExecuteHandler { get; }

        [Context]
        WpfControllerSpec_WpfControllerExtension WpfControllerExtension { get; }

        [Context]
        WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent RoutedEventHandlerInjectionForAttachedEvent { get; }

        [Context]
        WpfControllerSpec_UnhandledException UnhandledException { get; }
    }
}
