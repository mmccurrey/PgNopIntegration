using System;
using System.Linq;
using Nop.Services.Catalog;
using Nop.Services.Tasks;
using Septa.PayamGostar.ServiceLayer.ServiceType.Api;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog;

namespace Septa.PgNopIntegration.Plugin.PayamGostarService.Tasks
{
    public partial class ProductSyncTask : ITask
    {
        # region Properties
        private readonly IProductSyncService _productSyncService;
        private readonly IProductService _productService;
        private readonly IPgProductMetaDataService _pgProductMetaDataService;
        # endregion

        # region Ctor

        public ProductSyncTask(IProductSyncService productSyncService, IProductService productService, IPgProductMetaDataService PgProductMetaDataService)
        {
            this._productSyncService = productSyncService;
            this._productService = productService;
            this._productSyncService = productSyncService;
        }

        # endregion

        # region Utilities

        # endregion

        # region Implementation of ITask

        public virtual void Execute()
        {
            var productChnages = _productSyncService.GetProductChangesSince(DateTime.Now);

            if (productChnages.NewProducts != null && productChnages.NewProducts.Any())
            {
                foreach (var product in productChnages.NewProducts)
                {
                    // TODO: insert into Product Entity
                    // TODO : insert into PgProductMetaData Entity
                }
            }

            if (productChnages.EditedProducts != null && productChnages.EditedProducts.Any())
            {
                foreach (var p in productChnages.EditedProducts)
                {
                    // TODO: update into Product Entity
                    // TODO : update into PgProductMetaData Entity
                }
            }

            if (productChnages.DeletedProductCodes != null && productChnages.DeletedProductCodes.Any())
            {
                foreach (var p in productChnages.DeletedProductCodes)
                {
                    // TODO: delete from Product Entity
                    // TODO : delete from PgProductMetaData Entity
                }
            }
        }

        # endregion
    }
}
