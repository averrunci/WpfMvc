// Copyright (C) 2018 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading;
using Carna;

namespace Charites.Windows.Runners
{
    [Specification("StaActionRunner Spec")]
    class StaActionRunnerSpec : FixtureSteppable
    {
        ApartmentState ActualApartmentState { get; set; } = ApartmentState.Unknown;

        [Example("Runs an action in STA")]
        [Sample(ApartmentState.STA, Description = "When the apartment state of the current thread is STA")]
        [Sample(ApartmentState.MTA, Description = "When the apartment state of the current thread is MTA")]
        void Ex01(ApartmentState apartmentState)
        {
            When($"to run an action in {apartmentState}", () =>
            {
                var thread = new Thread(() =>
                    StaActionRunner.Run(() => ActualApartmentState = Thread.CurrentThread.GetApartmentState())
                );
                thread.SetApartmentState(apartmentState);
                thread.Start();
                thread.Join();
            });
            Then("the apartment state of the thread in which the action is performed should be STA", () => ActualApartmentState == ApartmentState.STA);
        }

        [Example("Runs an action asynchronously in STA")]
        [Sample(ApartmentState.STA, Description = "When the apartment state of the current thread is STA")]
        [Sample(ApartmentState.MTA, Description = "When the apartment state of the current thread is MTA")]
        void Ex02(ApartmentState apartmentState)
        {
            When($"to run an action asynchronously in {apartmentState}", () =>
            {
                var thread = new Thread(() =>
                    StaActionRunner.RunAsync(() => ActualApartmentState = Thread.CurrentThread.GetApartmentState()).Wait()
                );
                thread.SetApartmentState(apartmentState);
                thread.Start();
                thread.Join();
            });
            Then("the apartment state of the thread in which the action is performed should be STA", () => ActualApartmentState == ApartmentState.STA);
        }

        [Example("Throws an exception that occurred while an action is running")]
        void Ex03()
        {
            When("to throw an exception while an action is running", () => StaActionRunner.Run(() => throw new ApplicationException()));
            Then<ApplicationException>("the exception that is thrown while an action is running should be thrown");
        }

        [Example("Throws an exception when to run an action that is null")]
        void Ex04()
        {
            When("to run an action that is null", () => StaActionRunner.Run(null));
            Then<ArgumentNullException>($"{typeof(ArgumentNullException)} should be thrown");
        }

        [Example("Throws an exception when to run an action that is null synchronously")]
        void Ex05()
        {
            When("to run an action that is null asynchronously", () => StaActionRunner.RunAsync(null));
            Then<ArgumentNullException>($"{typeof(ArgumentNullException)} should be thrown");
        }
    }
}
