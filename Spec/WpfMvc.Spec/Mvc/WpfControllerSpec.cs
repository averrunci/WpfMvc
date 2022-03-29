// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc;

[Specification("WpfController Spec")]
class WpfControllerSpec
{
    [Context]
    WpfControllerSpec_EventHandlerDataContextElementInjection EventHandlerDataContextElementInjection => default!;

    [Context]
    WpfControllerSpec_CommandHandlerInjection CommandHandlerInjection => default!;

    [Context]
    WpfControllerSpec_AttachingAndDetachingController AttachingAndDetachingController => default!;

    [Context]
    WpfControllerSpec_ExecuteHandler ExecuteHandler => default!;

    [Context]
    WpfControllerSpec_WpfControllerExtension WpfControllerExtension => default!;

    [Context]
    WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent RoutedEventHandlerInjectionForAttachedEvent => default!;

    [Context]
    WpfControllerSpec_UnhandledException UnhandledException => default!;
}