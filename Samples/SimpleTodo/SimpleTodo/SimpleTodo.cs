﻿// Copyright (C) 2020 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace Charites.Windows.Samples.SimpleTodo
{
    internal class SimpleTodo : IHostedService
    {
        private readonly Application application;

        public SimpleTodo(Application application)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            application.Run();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
