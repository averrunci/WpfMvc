using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Hosting;

namespace WpfMvcApp
{
    internal class WpfMvcApp : IHostedService
    {
        private readonly Application application;

        public WpfMvcApp(Application application)
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
