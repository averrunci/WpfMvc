// Copyright (C) 2016 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
using System;

using Ninject.Modules;

using Fievus.Windows.Samples.SimpleLoginDemo.Adapter.User;

using Fievus.Windows.Samples.SimpleLoginDemo.Presentation.Contents.Login;

namespace Fievus.Windows.Samples.SimpleLoginDemo.Adapter
{
    public class SimpleLoginDemoModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserAuthentication>().To<UserAuthentication>();
        }
    }
}
