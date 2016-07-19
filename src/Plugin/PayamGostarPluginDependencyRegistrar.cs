using System;
using Autofac;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Septa.PayamGostar.ServiceLayer.ServiceType.Api;
using Septa.PgNopIntegration.Plugin.PayamGostarService;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Septa.PgNopIntegration.Plugin
{
    public class PayamGostarPluginDependencyRegistrar : IDependencyRegistrar
    {
        # region Implementation of IDependencyRegistrar

        public int Order
        {
            get
            {
                return 0;
            }
        }

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder, NopConfig config)
        {
            builder.RegisterType<ProductSyncService>().As<IProductSyncService>();
            builder.RegisterType<PgProductMetaDataService>().As<IPgProductMetaDataService>();
        }

        # endregion
    }
}
