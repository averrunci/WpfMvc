// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading;

using NUnit.Framework;

namespace Fievus.Windows.Runners
{
    [TestFixture]
    public class StaActionRunnerTest
    {
        [Test]
        public void RunsActionInSTAWhenCurrentThreadApartmentStateIsMTA()
        {
            var actualApartmentState = ApartmentState.Unknown;

            var thread = new Thread(() =>
            {
                StaActionRunner.Run(() =>
                {
                    actualApartmentState = Thread.CurrentThread.GetApartmentState();
                });
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();

            Assert.That(actualApartmentState, Is.EqualTo(ApartmentState.STA));
        }

        [Test]
        public void RunsActionInSTAWhenCurrentThreadApartmentStateIsSTA()
        {
            var actualApartmentState = ApartmentState.Unknown;

            var thread = new Thread(() => {
                StaActionRunner.Run(() =>
                {
                    actualApartmentState = Thread.CurrentThread.GetApartmentState();
                });
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.That(actualApartmentState, Is.EqualTo(ApartmentState.STA));
        }

        [Test]
        public void ThrowsExceptionThatIsOccurredWhileActionIsRunning()
        {
            Assert.Throws<ApplicationException>(() => StaActionRunner.Run(() => { throw new ApplicationException(); }));
        }

        [Test]
        public void ThrowsExceptionWhenActionThatIsNullIsRun()
        {
            Assert.Throws<ArgumentNullException>(() => StaActionRunner.Run(null));
        }

        [Test]
        public void RunsActionIsSTAAsynchronouslyWhenCurrentThreadApartmentStateIsMTA()
        {
            var actualApartmentState = ApartmentState.Unknown;

            var thread = new Thread(() =>
            {
                var actionRunEvent = new AutoResetEvent(false);
                StaActionRunner.RunAsync(() =>
                {
                    actualApartmentState = Thread.CurrentThread.GetApartmentState();
                    actionRunEvent.Set();
                });
                actionRunEvent.WaitOne();
            });
            thread.SetApartmentState(ApartmentState.MTA);
            thread.Start();
            thread.Join();

            Assert.That(actualApartmentState, Is.EqualTo(ApartmentState.STA));
        }

        [Test]
        public void RunsActionIsSTAAsynchronouslyWhenCurrentThreadApartmentStateIsSTA()
        {
            var actualApartmentState = ApartmentState.Unknown;

            var thread = new Thread(() =>
            {
                var actionRunEvent = new AutoResetEvent(true);
                StaActionRunner.RunAsync(() =>
                {
                    actualApartmentState = Thread.CurrentThread.GetApartmentState();
                    actionRunEvent.Set();
                });
                actionRunEvent.WaitOne();
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            Assert.That(actualApartmentState, Is.EqualTo(ApartmentState.STA));
        }
    }
}
