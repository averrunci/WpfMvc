using Charites.Windows.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WpfMvcApp;

public class WpfMvcAppControllerFactory : IWpfControllerFactory
{
    private readonly IServiceProvider services;

    public WpfMvcAppControllerFactory(IServiceProvider services) => this.services = services;

    public object Create(Type controllerType) => services.GetRequiredService(controllerType);
}
