using System.Windows;
using Microsoft.Extensions.Hosting;

namespace $safeprojectname$;

internal class $safeitemrootname$(Application application) : IHostedService
{
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
