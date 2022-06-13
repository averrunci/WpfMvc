// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Carna;

namespace Charites.Windows.Mvc;

[Specification(
    "WpfController Spec",
    typeof(WpfControllerSpec_EventHandlerDataContextElementInjection),
    typeof(WpfControllerSpec_CommandHandlerInjection),
    typeof(WpfControllerSpec_AttachingAndDetachingController),
    typeof(WpfControllerSpec_ExecuteHandler),
    typeof(WpfControllerSpec_WpfControllerExtension),
    typeof(WpfControllerSpec_RoutedEventHandlerInjectionForAttachedEvent),
    typeof(WpfControllerSpec_UnhandledException)
)]
class WpfControllerSpec
{
}