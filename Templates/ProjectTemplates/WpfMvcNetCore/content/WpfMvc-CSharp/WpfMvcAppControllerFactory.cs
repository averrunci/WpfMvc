using Charites.Windows.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WpfMvcApp;

public class WpfMvcAppControllerFactory(IServiceProvider services) : IWpfControllerFactory
{
    public object Create(Type controllerType) => services.GetRequiredService(controllerType);
}
