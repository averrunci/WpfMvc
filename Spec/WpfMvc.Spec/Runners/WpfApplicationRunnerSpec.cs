// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;
using Carna;
using FluentAssertions;

namespace Charites.Windows.Runners
{
    [Specification("WpfApplicationRunner Spec")]
    class WpfApplicationRunnerSpec : FixtureSteppable, IDisposable
    {
        IWpfApplicationRunner<Application> WpfRunner { get; set; }

        public void Dispose()
        {
            WpfRunner?.Shutdown();
        }

        [Example("Runs an action on the WPF application thread")]
        void Ex01()
        {
            Given("a runner to act on the WPF application thread", () => WpfRunner = WpfApplicationRunner.Start<Application>());
            Expect("the application object should not be null", () => WpfRunner.Run(application => application.Should().NotBeNull()));
            Expect("the thread should be able to access to the application object", () => WpfRunner.Run(application => application.CheckAccess().Should().BeTrue()));
            Expect("the main window of the application object should be null", () => WpfRunner.Run(application => application.MainWindow.Should().BeNull()));
        }

        [Example("Runs an action with the main window on the WPF application thread")]
        void Ex02()
        {
            Given("a runner to act with the main window on the WPF application thread", () => WpfRunner = WpfApplicationRunner.Start<Application, Window>());
            Expect("the application object should not be null", () => WpfRunner.Run(application => application.Should().NotBeNull()));
            Expect("the thread should be able to access to the application object", () => WpfRunner.Run(application => application.CheckAccess().Should().BeTrue()));
            Expect("the main window of the application object should not be null", () => WpfRunner.Run(application => application.MainWindow.Should().NotBeNull()));
        }

        [Example("Runs an action with an application initial action on the WPF application thread")]
        void Ex03()
        {
            Given("a runner to act with an initial action that shows the main window on the WPF application thread", () =>
                WpfRunner = WpfApplicationRunner.Start<Application>(application => application.Startup += (s, e) => new Window().Show())
            );
            Expect("the application object should not be null", () => WpfRunner.Run(application => application.Should().NotBeNull()));
            Expect("the thread should be able to access to the application object", () => WpfRunner.Run(application => application.CheckAccess().Should().BeTrue()));
            Expect("the main window of the application object should not be null", () => WpfRunner.Run(application => application.MainWindow.Should().NotBeNull()));
        }

        [Example("Runs an action with the main window and an application initial action on the WPF application thread")]
        void Ex04()
        {
            Given("a runner to act with the main window and an initial action that asserts the application should not be null on the WPF application thread", () =>
                WpfRunner = WpfApplicationRunner.Start<Application, Window>(application => application.Should().NotBeNull())
            );
            Expect("the application object should not be null", () => WpfRunner.Run(application => application.Should().NotBeNull()));
            Expect("the thread should be able to access to the application object", () => WpfRunner.Run(application => application.CheckAccess().Should().BeTrue()));
            Expect("the main window of the application object should not be null", () => WpfRunner.Run(application => application.MainWindow.Should().NotBeNull()));
        }

        [Example("Runs an action with some context associated with the WPF application thread")]
        void Ex05()
        {
            Given("a runner to act on the WPF application thread", () => WpfRunner = WpfApplicationRunner.Start<Application>());
            When("an object is set on the WPF application thread", () => WpfRunner.Run((application, context) => context.Set("Item1", new FrameworkElement { Name = "Item1" })));
            Then("the object should be got on the WPF application thread", () => WpfRunner.Run((application, context) => context.Get<FrameworkElement>("Item1").Name.Should().Be("Item1")));
        }
    }
}
