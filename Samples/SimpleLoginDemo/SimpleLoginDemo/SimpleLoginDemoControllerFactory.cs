// Copyright (C) 2022 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using Charites.Windows.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Charites.Windows.Samples.SimpleLoginDemo;

public class SimpleLoginDemoControllerFactory : IWpfControllerFactory
{
    private readonly IServiceProvider services;

    public SimpleLoginDemoControllerFactory(IServiceProvider services) => this.services = services;

    protected virtual object Create(Type controllerType) => services.GetRequiredService(controllerType);
    object IWpfControllerFactory.Create(Type controllerType) => Create(controllerType);
}