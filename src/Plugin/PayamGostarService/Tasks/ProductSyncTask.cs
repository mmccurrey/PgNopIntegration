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
                _productService.InsertProduct(product);

                var pgProductMetaData = new PgProductMetaData();
                InstantiateNewPgProductMetaData(p, pgProductMetaData);
                pgProductMetaData.ProductId = product.Id;
                _pgProductMetaDataService.InsertPgProductMetaData(pgProductMetaData);
            }
        }

        protected virtual void SyncEditedProducts(IEnumerable<Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product> products)
        {
            foreach (var p in products)
            {
                var pgProductMetaData = _pgProductMetaDataService.GetPgProductMetaDataByCode(p.Code);

                // update product
                var product = _productService.GetProductById(pgProductMetaData.ProductId);
                InstantiateEditedProduct(p, product);
                _productService.UpdateProduct(product);


                // update pgProductMetaData
                pgProductMetaData.Code = p.Code;
                pgProductMetaData.ProductId = pgProductMetaData.ProductId;
                pgProductMetaData.LastSyncDate = DateTime.UtcNow;
                _pgProductMetaDataService.UpdatePgProductMetaData(pgProductMetaData);

            }
        }

        protected virtual void SyncDeletedProducts(IEnumerable<string> productCodeList)
        {
            foreach (var productCode in productCodeList)
            {
                var pgProductMetaData = _pgProductMetaDataService.GetPgProductMetaDataByCode(productCode);
                var product = _productService.GetProductById(pgProductMetaData.ProductId);

                // update product
                InstantiateDeletedProduct(product);
                _productService.UpdateProduct(product);

                // update pgProductMetaData
                pgProductMetaData.Code = productCode;
                pgProductMetaData.ProductId = pgProductMetaData.ProductId;
                pgProductMetaData.LastSyncDate = DateTime.UtcNow;
                _pgProductMetaDataService.UpdatePgProductMetaData(pgProductMetaData);

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

        protected virtual void InstantiateEditedProduct(Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product sourceProduct, Nop.Core.Domain.Catalog.Product targetProduct)
        {
            targetProduct.Name = sourceProduct.Name;
            targetProduct.Price = sourceProduct.UnitPrice;

            targetProduct.ProductTemplateId = targetProduct.ProductTemplateId;

            targetProduct.ProductTypeId = targetProduct.ProductTypeId;
            targetProduct.MaximumCustomerEnteredPrice = targetProduct.MaximumCustomerEnteredPrice;
            targetProduct.MaxNumberOfDownloads = targetProduct.MaxNumberOfDownloads;
            targetProduct.RecurringCycleLength = targetProduct.RecurringCycleLength;
            targetProduct.RecurringTotalCycles = targetProduct.RecurringTotalCycles;
            targetProduct.RentalPriceLength = targetProduct.RentalPriceLength;
            targetProduct.StockQuantity = targetProduct.StockQuantity;
            targetProduct.NotifyAdminForQuantityBelow = targetProduct.NotifyAdminForQuantityBelow;
            targetProduct.OrderMinimumQuantity = targetProduct.OrderMinimumQuantity;
            targetProduct.OrderMaximumQuantity = targetProduct.OrderMaximumQuantity;

            targetProduct.UnlimitedDownloads = targetProduct.UnlimitedDownloads;
            targetProduct.AllowCustomerReviews = targetProduct.AllowCustomerReviews;
            targetProduct.VisibleIndividually = targetProduct.VisibleIndividually;
            targetProduct.ShowOnHomePage = targetProduct.ShowOnHomePage;

            targetProduct.Published = targetProduct.Published;
            targetProduct.Deleted = targetProduct.Deleted;

            targetProduct.CreatedOnUtc = targetProduct.CreatedOnUtc;
            targetProduct.UpdatedOnUtc = DateTime.UtcNow;
        }

        protected virtual void InstantiateDeletedProduct(Nop.Core.Domain.Catalog.Product targetProduct)
        {
            targetProduct.Name = targetProduct.Name;
            targetProduct.Price = targetProduct.Price;

            targetProduct.ProductTemplateId = targetProduct.ProductTemplateId;

            targetProduct.ProductTypeId = targetProduct.ProductTypeId;
            targetProduct.MaximumCustomerEnteredPrice = targetProduct.MaximumCustomerEnteredPrice;
            targetProduct.MaxNumberOfDownloads = targetProduct.MaxNumberOfDownloads;
            targetProduct.RecurringCycleLength = targetProduct.RecurringCycleLength;
            targetProduct.RecurringTotalCycles = targetProduct.RecurringTotalCycles;
            targetProduct.RentalPriceLength = targetProduct.RentalPriceLength;
            targetProduct.StockQuantity = targetProduct.StockQuantity;
            targetProduct.NotifyAdminForQuantityBelow = targetProduct.NotifyAdminForQuantityBelow;
            targetProduct.OrderMinimumQuantity = targetProduct.OrderMinimumQuantity;
            targetProduct.OrderMaximumQuantity = targetProduct.OrderMaximumQuantity;

            targetProduct.UnlimitedDownloads = targetProduct.UnlimitedDownloads;
            targetProduct.AllowCustomerReviews = targetProduct.AllowCustomerReviews;
            targetProduct.VisibleIndividually = targetProduct.VisibleIndividually;
            targetProduct.ShowOnHomePage = targetProduct.ShowOnHomePage;

            targetProduct.Published = targetProduct.Published;
            targetProduct.Deleted = true; ;

            targetProduct.CreatedOnUtc = targetProduct.CreatedOnUtc;
            targetProduct.UpdatedOnUtc = DateTime.UtcNow;
        }

        # endregion

        # region Implementation of ITask

        /// <summary>
        /// Executes a task
        /// </summary>
        public virtual void Execute()
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
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

                scope.Complete();
            }
        }

        # endregion
    }
}
