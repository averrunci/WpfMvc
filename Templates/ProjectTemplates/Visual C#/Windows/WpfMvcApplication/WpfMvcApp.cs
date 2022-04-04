using System.Windows;
using Microsoft.Extensions.Hosting;

namespace $safeprojectname$;

internal class $safeitemrootname$ : IHostedService
{
    private readonly Application application;

    public $safeitemrootname$(Application application)
    {
        this.application = application;
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
