// Copyright (C) 2018-2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System.Windows;
using Carna;

namespace Charites.Windows.Mvc
{
    [Context("WpfControllerExtension")]
    class WpfControllerSpec_WpfControllerExtension : FixtureSteppable
    {
        class TestExtension : IWpfControllerExtension
        {
            public static object TestExtensionContainer { get; } = new object();
            public bool Attached { get; private set; }
            public bool Detached { get; private set; }
            void IControllerExtension<FrameworkElement>.Attach(object controller, FrameworkElement element) { Attached = true; }
            void IControllerExtension<FrameworkElement>.Detach(object controller, FrameworkElement element) { Detached = true; }
            object IControllerExtension<FrameworkElement>.Retrieve(object controller) => TestExtensionContainer;
        }

        TestWpfControllers.TestWpfController Controller { get; } = new TestWpfControllers.TestWpfController();
        TestExtension Extension { get; } = new TestExtension();
        TestElement Element { get; } = new TestElement { DataContext = new TestDataContexts.TestDataContext() };

        [Example("Attaches an extension when the element is initialized and detaches it when the element is unloaded")]
        void Ex01()
        {
            When("an extension is added to the WpfController", () => WpfController.AddExtension(Extension));
            When("the WpfController is enabled for the element", () => WpfController.SetIsEnabled(Element, true));

            When("the Initialized event is raised", () => Element.RaiseInitialized());
            Then("the extension should be attached", () => Extension.Attached);

            When("the Unloaded event is raised", () => Element.RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = Element }));
            Then("the extension should be detached", () => Extension.Detached);
        }

        [Example("Retrieves a container of an extension")]
        void Ex02()
        {
            When("an extension is added", () => WpfController.AddExtension(new TestExtension()));
            Then("the container of the extension should be retrieved", () => WpfController.Retrieve<TestExtension, object>(Controller) == TestExtension.TestExtensionContainer);
        }
    }
}
