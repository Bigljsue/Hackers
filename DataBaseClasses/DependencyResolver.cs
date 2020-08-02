using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using WPF_HackersList.DataBaseClasses.DataBaseMethods;

namespace WPF_HackersList.DataBaseClasses
{
    public static class DependencyResolver
    {
        private static IServiceProvider _container;
        private static IServiceProvider Container => _container ??= BuildContainer();

        private static IServiceProvider BuildContainer()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<IDataBaseGetMethods, DataBaseMethods.DataBaseMethods>();
            serviceCollection.AddTransient<IDataBaseAddMethods, DataBaseMethods.DataBaseMethods>();
            serviceCollection.AddTransient<IDataBaseDeleteMethods, DataBaseMethods.DataBaseMethods>();
            serviceCollection.AddTransient<IDataBaseUpdateMethods, DataBaseMethods.DataBaseMethods>();

            return serviceCollection.BuildServiceProvider();
        }

        public static TInterface Resolve<TInterface>()
        {
            return Container.GetRequiredService<TInterface>();
        }
    }
}
