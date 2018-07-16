// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using Carna;
using Charites.Windows.Runners;
using FluentAssertions;

namespace Charites.Windows.Mvc
{
    [Context("WpfControllerExtension")]
    class WpfControllerSpec_WpfControllerExtension : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; }

        class TestExtension : IWpfControllerExtension
        {
            public static object TestExtensionContainer { get; } = new object();
            public bool Attached { get; private set; }
            public bool Detached { get; private set; }
            void IControllerExtension<FrameworkElement>.Attach(object controller, FrameworkElement element) { Attached = true; }
            void IControllerExtension<FrameworkElement>.Detach(object controller, FrameworkElement element) { Detached = true; }
            object IControllerExtension<FrameworkElement>.Retrieve(object controller) => TestExtensionContainer;
        }

        const string ExtensionKey = "Extension";
        const string ElementKey = "Element";
        private const string ControllerKey = "Controller";

        TestWpfControllers.TestWpfController Controller { get; } = new TestWpfControllers.TestWpfController();

        public WpfControllerSpec_WpfControllerExtension()
        {
            WpfRunner = WpfApplicationRunner.Start<Application>();
            WpfRunner.Run((application, context) => context.Set(ExtensionKey, new TestExtension()));
        }

        public void Dispose()
        {
            WpfRunner.Run((application, context) =>
            {
                if (!(context.Get(ExtensionKey) is IWpfControllerExtension extension)) return;

                WpfController.RemoveExtension(extension);
            });

            WpfRunner.Shutdown();
        }

        [Example("Attaches an extension when the element is initialized and detaches it when the element is unloaded")]
        void Ex01()
        {
            Given("an element that contains a data context", () => WpfRunner.Run((application, context) => context.Set(ElementKey, new TestElement { DataContext = new TestDataContexts.TestDataContext() })));
            When("an extension is added to the WpfController", () => WpfRunner.Run((application, context) => WpfController.AddExtension(context.Get<TestExtension>(ExtensionKey))));
            When("the WpfController is enabled for the element", () => WpfRunner.Run((application, context) =>
            {
                var element = context.Get<TestElement>(ElementKey);
                WpfController.SetIsEnabled(element, true);
                context.Set(ControllerKey, element.GetController<TestWpfControllers.TestWpfController>());
            }));

            When("the Initialized event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseInitialized()));
            Then("the extension should be attached", () => WpfRunner.Run((application, context) => context.Get<TestExtension>(ExtensionKey).Attached.Should().BeTrue()));

            When("the Unloaded event is raised", () => WpfRunner.Run((application, context) => context.Get<TestElement>(ElementKey).RaiseEvent(new RoutedEventArgs(FrameworkElement.UnloadedEvent) { Source = context.Get<TestElement>(ElementKey) })));
            Then("the extension should be detached", () => WpfRunner.Run((application, context) => context.Get<TestExtension>(ExtensionKey).Detached.Should().BeTrue()));
        }

        [Example("Retrieves a container of an extension")]
        void Ex02()
        {
            When("an extension is added", () => WpfController.AddExtension(new TestExtension()));
            Then("the container of the extension should be retrieved", () => WpfController.Retrieve<TestExtension, object>(Controller) == TestExtension.TestExtensionContainer);
        }
    }
}
