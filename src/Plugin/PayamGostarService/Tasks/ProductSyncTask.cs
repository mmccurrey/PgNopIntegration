using System;
using System.Linq;
using Nop.Services.Tasks;
using Nop.Services.Logging;
using Nop.Services.Catalog;
using Nop.Core.Domain.Catalog;
using Septa.PayamGostar.ServiceLayer.ServiceType.Api;
using Septa.PgNopIntegration.Plugin.Domain;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Extensions;
using System.Collections.Generic;
using System.Transactions;

namespace Septa.PgNopIntegration.Plugin.PayamGostarService.Tasks
{
    /// <summary>
    /// Represents a task for syncing products from PayamGostar
    /// </summary>
    public partial class ProductSyncTask : ITask
    {
        # region Properties
        private readonly IProductSyncService _productSyncService;
        private readonly IProductService _productService;
        private readonly IPgProductMetaDataService _pgProductMetaDataService;
        private readonly IProductTemplateService _productTemplateService;
        private readonly ILogger _logger;
        # endregion

        # region Ctor

        public ProductSyncTask(IProductSyncService productSyncService, IProductService productService, IPgProductMetaDataService pgProductMetaDataService, IProductTemplateService productTemplateService, ILogger logger)
        {
            this._productSyncService = productSyncService;
            this._productService = productService;
            this._productSyncService = productSyncService;
            this._pgProductMetaDataService = pgProductMetaDataService;
            this._productTemplateService = productTemplateService;
            this._logger = logger;
        }

        # endregion

        # region Utilities

        protected virtual void SyncNewProducts(IEnumerable<Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product> products)
        {
            foreach (var p in products)
            {
                var product = new Nop.Core.Domain.Catalog.Product();
                InstantiateNewProduct(p, product);
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    _productService.InsertProduct(product);
                    scope.Complete();
                }

                var pgProductMetaData = new PgProductMetaData();
                InstantiateNewPgProductMetaData(p, pgProductMetaData);
                pgProductMetaData.ProductId = product.Id;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    _pgProductMetaDataService.InsertPgProductMetaData(pgProductMetaData);
                    scope.Complete();
                }
            }
        }

        protected virtual void SyncEditedProducts(IEnumerable<Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product> products)
        {
            foreach (var p in products)
            {
                var pgProductMetaData = _pgProductMetaDataService.GetPgProductMetaDataByCode(p.Code);

                // update product
                var product = _productService.GetProductById(pgProductMetaData.ProductId);
                product.Name = p.Name;
                product.Price = p.UnitPrice;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    _productService.UpdateProduct(product);
                    scope.Complete();
                }

                // update pgProductMetaData
                pgProductMetaData.LastSyncDate = DateTime.UtcNow;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    _pgProductMetaDataService.UpdatePgProductMetaData(pgProductMetaData);
                    scope.Complete();
                }
            }
        }

        protected virtual void SyncDeletedProducts(IEnumerable<string> productCodeList)
        {
            foreach (var productCode in productCodeList)
            {
                var pgProductMetaData = _pgProductMetaDataService.GetPgProductMetaDataByCode(productCode);
                var product = _productService.GetProductById(pgProductMetaData.ProductId);

                // update product
                product.Deleted = true;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    _productService.UpdateProduct(product);
                    scope.Complete();
                }

                // update pgProductMetaData
                pgProductMetaData.LastSyncDate = DateTime.UtcNow;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    _pgProductMetaDataService.UpdatePgProductMetaData(pgProductMetaData);
                    scope.Complete();
                }
            }
        }

        protected virtual void InstantiateNewProduct(Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product sourceProduct, Nop.Core.Domain.Catalog.Product targetProduct)
        {
            targetProduct.Name = sourceProduct.Name;
            targetProduct.Price = sourceProduct.UnitPrice;

            var productTemplateId = _productTemplateService.GetAllProductTemplates().Where(p => p.Name.Contains("Simple product")).FirstOrDefault().Id;
            targetProduct.ProductTemplateId = productTemplateId;

            targetProduct.ProductTypeId = (int)ProductType.SimpleProduct;
            targetProduct.MaximumCustomerEnteredPrice = 1000;
            targetProduct.MaxNumberOfDownloads = 10;
            targetProduct.RecurringCycleLength = 100;
            targetProduct.RecurringTotalCycles = 10;
            targetProduct.RentalPriceLength = 1;
            targetProduct.StockQuantity = 100;
            targetProduct.NotifyAdminForQuantityBelow = 1;
            targetProduct.OrderMinimumQuantity = 1;
            targetProduct.OrderMaximumQuantity = 100;

            targetProduct.UnlimitedDownloads = true;
            targetProduct.AllowCustomerReviews = true;
            targetProduct.VisibleIndividually = true;
            targetProduct.ShowOnHomePage = false;

            targetProduct.Published = true;
            targetProduct.Deleted = false;
            targetProduct.CreatedOnUtc = DateTime.UtcNow;
            targetProduct.UpdatedOnUtc = DateTime.UtcNow;
        }

        protected virtual void InstantiateNewPgProductMetaData(Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product sourceProduct, PgProductMetaData targetProduct)
        {
            targetProduct.Code = sourceProduct.Code;
            targetProduct.LastSyncDate = DateTime.UtcNow;
        }

        # endregion

        # region Implementation of ITask

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual void Execute()
        {
            try
            {
                var lastSyncDate = _pgProductMetaDataService.GetLastSyncDate();
                var productChnages = _productSyncService.GetProductChangesSince(lastSyncDate); 

                if (!productChnages.NewProducts.IsNullOrEmpty())
                {
                    SyncNewProducts(productChnages.NewProducts);
                }

                if (!productChnages.EditedProducts.IsNullOrEmpty())
                {
                    SyncEditedProducts(productChnages.EditedProducts);
                }

                if (!productChnages.DeletedProductCodes.IsNullOrEmpty())
                {
                    SyncDeletedProducts(productChnages.DeletedProductCodes);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format("Error syncing products from PayamGostar. {0}", ex.Message), ex);
            }
        }

        # endregion
    }
}
