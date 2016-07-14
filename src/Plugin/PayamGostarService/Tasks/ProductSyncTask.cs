using System;
using System.Linq;
using Nop.Services.Tasks;
using Nop.Services.Logging;
using Nop.Services.Catalog;
using Nop.Core.Domain.Catalog;
using Septa.PayamGostar.ServiceLayer.ServiceType.Api;
using Septa.PgNopIntegration.Plugin.Domain;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog;
using System.Collections.Generic;

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
        private readonly ILogger _logger;
        # endregion

        # region Ctor

        public ProductSyncTask(IProductSyncService productSyncService, IProductService productService, IPgProductMetaDataService pgProductMetaDataService, ILogger logger)
        {
            this._productSyncService = productSyncService;
            this._productService = productService;
            this._productSyncService = productSyncService;
            this._pgProductMetaDataService = pgProductMetaDataService;
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
                product.Name = p.Name;
                product.Price = p.UnitPrice;
                _productService.UpdateProduct(product);

                // update pgProductMetaData
                pgProductMetaData.Code = p.Code;
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
                product.Deleted = true;
                _productService.UpdateProduct(product);

                // update pgProductMetaData
                pgProductMetaData.LastSyncDate = DateTime.UtcNow;
                _pgProductMetaDataService.UpdatePgProductMetaData(pgProductMetaData);
            }
        }

        protected virtual void InstantiateNewProduct(Septa.PayamGostar.ServiceLayer.Contract.DataContracts.Product sourceProduct, Nop.Core.Domain.Catalog.Product targetProduct)
        {
            targetProduct.Name = sourceProduct.Name;
            targetProduct.Price = sourceProduct.UnitPrice;

            targetProduct.ProductTypeId = (int)ProductType.SimpleProduct;
            targetProduct.ParentGroupedProductId = 0; // TODO: check whether the product parent code will be received from PayamGostar or not?
            targetProduct.VisibleIndividually = true;
            targetProduct.ProductTemplateId = 1;
            targetProduct.VendorId = 0;
            targetProduct.ShowOnHomePage = false;
            targetProduct.AllowCustomerReviews = true;
            targetProduct.ApprovedRatingSum = 0;
            targetProduct.NotApprovedRatingSum = 0;
            targetProduct.ApprovedTotalReviews = 0;
            targetProduct.NotApprovedTotalReviews = 0;
            targetProduct.SubjectToAcl = false;
            targetProduct.LimitedToStores = false;
            targetProduct.IsGiftCard = false;
            targetProduct.GiftCardTypeId = 0;
            targetProduct.RequireOtherProducts = false;
            targetProduct.AutomaticallyAddRequiredProducts = false;
            targetProduct.IsDownload = false;
            targetProduct.DownloadId = 0;
            targetProduct.UnlimitedDownloads = false;
            targetProduct.MaxNumberOfDownloads = 0;
            targetProduct.DownloadActivationTypeId = 0;
            targetProduct.HasSampleDownload = false;
            targetProduct.SampleDownloadId = 0;
            targetProduct.HasUserAgreement = false;
            targetProduct.IsRecurring = false;
            targetProduct.RecurringCycleLength = 0;
            targetProduct.RecurringCyclePeriodId = 0;
            targetProduct.RecurringTotalCycles = 0;
            targetProduct.IsRental = false;
            targetProduct.RentalPriceLength = 0;
            targetProduct.RentalPricePeriodId = 0;
            targetProduct.IsShipEnabled = false;
            targetProduct.IsFreeShipping = false;
            targetProduct.ShipSeparately = false;
            targetProduct.AdditionalShippingCharge = 0;
            targetProduct.DeliveryDateId = 0;
            targetProduct.IsTaxExempt = false;
            targetProduct.TaxCategoryId = 0;
            targetProduct.IsTelecommunicationsOrBroadcastingOrElectronicServices = false;
            targetProduct.ManageInventoryMethodId = 0;
            targetProduct.UseMultipleWarehouses = false;
            targetProduct.WarehouseId = 0;
            targetProduct.StockQuantity = 0;
            targetProduct.DisplayStockAvailability = false;
            targetProduct.DisplayStockQuantity = false;
            targetProduct.MinStockQuantity = 0;
            targetProduct.LowStockActivityId = 0;
            targetProduct.NotifyAdminForQuantityBelow = 0;
            targetProduct.BackorderModeId = 0;
            targetProduct.AllowBackInStockSubscriptions = false;
            targetProduct.OrderMinimumQuantity = 0;
            targetProduct.OrderMaximumQuantity = 0;
            targetProduct.AllowAddingOnlyExistingAttributeCombinations = false;
            targetProduct.DisableBuyButton = false;
            targetProduct.DisableWishlistButton = false;
            targetProduct.AvailableForPreOrder = false;
            targetProduct.CallForPrice = false;
            targetProduct.OldPrice = 0;
            targetProduct.ProductCost = 0;
            targetProduct.CustomerEntersPrice = false;
            targetProduct.MinimumCustomerEnteredPrice = 0;
            targetProduct.MaximumCustomerEnteredPrice = 0;
            targetProduct.BasepriceEnabled = false;
            targetProduct.BasepriceAmount = 0;
            targetProduct.BasepriceUnitId = 0;
            targetProduct.BasepriceBaseAmount = 0;
            targetProduct.BasepriceBaseUnitId = 0;
            targetProduct.MarkAsNew = false;
            targetProduct.HasTierPrices = false;
            targetProduct.HasDiscountsApplied = false;
            targetProduct.Weight = 0;
            targetProduct.Length = 0;
            targetProduct.Width = 0;
            targetProduct.Height = 0;
            targetProduct.DisplayOrder = 0;
            targetProduct.Published = false;
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
                var productChnages = _productSyncService.GetProductChangesSince(DateTime.Now); // use DateTime.Now for just as a sample

                if (productChnages.NewProducts != null && productChnages.NewProducts.Any())
                {
                    SyncNewProducts(productChnages.NewProducts);
                }

                if (productChnages.EditedProducts != null && productChnages.EditedProducts.Any())
                {
                    SyncEditedProducts(productChnages.EditedProducts);
                }

                if (productChnages.DeletedProductCodes != null && productChnages.DeletedProductCodes.Any())
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
