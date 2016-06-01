// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Windows;

using NUnit.Framework;

namespace Fievus.Windows.Runners
{
    [TestFixture]
    public class WpfApplicationRunnerTest
    {
        [Test]
        public void RunsActionWithRunnerThatRunsOnWpfApplicationThread()
        {
            WpfApplicationRunner.Start<Application>()
                .Run(application =>
                {
                    Assert.That(application.CheckAccess(), Is.True);
                    Assert.That(application, Is.Not.Null);
                    Assert.That(application.MainWindow, Is.Null);
                }).Shutdown();
        }

        [Test]
        public void RunsActionWithRunnerThatIsInitializedWithApplicationInitialAction()
        {
            WpfApplicationRunner.Start<Application>(application =>
            {
                application.Startup += (s, e) =>
                {
                    new Window().Show();
                };
            }).Run(application =>
            {
                Assert.That(application.CheckAccess(), Is.True);
                Assert.That(application, Is.Not.Null);
                Assert.That(application.MainWindow, Is.Not.Null);
            }).Shutdown();
        }

        [Test]
        public void RunsActionWithRunnerThatIsInitializedWithMainWindow()
        {
            WpfApplicationRunner.Start<Application, Window>()
                .Run(application =>
                {
                    Assert.That(application.CheckAccess(), Is.True);
                    Assert.That(application, Is.Not.Null);
                    Assert.That(application.MainWindow, Is.Not.Null);
                }).Shutdown();
        }

        [Test]
        public void RunsActionWithRunnerThatIsInitializedWithMainWindowAndApplicationInitialAction()
        {
            WpfApplicationRunner.Start<Application, Window>(application =>
            {
                Assert.That(application, Is.Not.Null);
            }).Run(application =>
            {
                Assert.That(application.CheckAccess(), Is.True);
                Assert.That(application, Is.Not.Null);
                Assert.That(application.MainWindow, Is.Not.Null);
            }).Shutdown();
        }
    }
}
