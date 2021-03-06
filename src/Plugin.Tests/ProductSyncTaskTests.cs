﻿using Moq;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Septa.PayamGostar.ServiceLayer.Contract.DataContracts;
using Septa.PayamGostar.ServiceLayer.ServiceType.Api;
using Septa.PgNopIntegration.Plugin.Domain;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Tasks;
using System;
using System.Collections.Generic;
using Xunit;

namespace Septa.PgNopIntegration.Plugin.Tests
{
    public class ProductSyncTaskTests
    {
        [Fact]
        public void NewProductsFromPayamGostarAreInserted()
        {

            var productSyncServiceMock = new Mock<IProductSyncService>();
            var productServiceMock = new Mock<IProductService>();
            var pgProductMetaDataServiceMock = new Mock<IPgProductMetaDataService>();
            var productTemplateServiceMock = new Mock<IProductTemplateService>();
            var logger = new Mock<ILogger>();

            productTemplateServiceMock.Setup(x => x.GetAllProductTemplates())
                .Returns(new List<Nop.Core.Domain.Catalog.ProductTemplate> 
                {
                    new Nop.Core.Domain.Catalog.ProductTemplate() {Id=1, Name="Simple product", DisplayOrder=1, ViewPath=""}
                }
                );

            productSyncServiceMock.Setup(x => x.GetProductChangesSince(It.IsAny<DateTime>()))
                .Returns(new ProductChanges
                {
                    NewProducts = new
                 List<Product>
                {
                    new Product { Code = "1", Name = "test product", UnitPrice = 1000.0M }
                }
                });

            var insertedProductId = 5;
            productServiceMock.Setup(x => x.InsertProduct(It.IsAny<Nop.Core.Domain.Catalog.Product>()))
                .Callback<Nop.Core.Domain.Catalog.Product>(x => x.Id = insertedProductId);

            var productSyncTask = new ProductSyncTask(
                productSyncServiceMock.Object,
                productServiceMock.Object,
                pgProductMetaDataServiceMock.Object,
                productTemplateServiceMock.Object,
                logger.Object);

            productSyncTask.Execute();

            productServiceMock.Verify(x => x.InsertProduct(It.Is<Nop.Core.Domain.Catalog.Product>(product => product.Price == 1000.0M && product.Name == "test product")));
            pgProductMetaDataServiceMock.Verify(x => x.InsertPgProductMetaData(It.Is<PgProductMetaData>(meta => meta.Code == "1" && meta.ProductId == insertedProductId)));
        }

        [Fact]
        public void EditedProductsFromPayamGostarAreUpdated()
        {
            var productSyncServiceMock = new Mock<IProductSyncService>();
            var productServiceMock = new Mock<IProductService>();
            var pgProductMetaDataServiceMock = new Mock<IPgProductMetaDataService>();
            var productTemplateServiceMock = new Mock<IProductTemplateService>();
            var logger = new Mock<ILogger>();

            productTemplateServiceMock.Setup(x => x.GetAllProductTemplates())
             .Returns(new List<Nop.Core.Domain.Catalog.ProductTemplate> 
                {
                    new Nop.Core.Domain.Catalog.ProductTemplate() {Id=1, Name="Simple product", DisplayOrder=1, ViewPath=""}
                }
             );

            productSyncServiceMock.Setup(x => x.GetProductChangesSince(It.IsAny<DateTime>()))
                .Returns(new ProductChanges
                {
                    NewProducts = null,

                    EditedProducts = new
                    List<Product>
                    {
                        new Product { Code = "1", Name = "test product", UnitPrice = 1000.0M }
                    },

                    DeletedProductCodes = null
                });

            var updattedProductId = 5;

            pgProductMetaDataServiceMock.Setup(x => x.GetPgProductMetaDataByCode(It.IsAny<string>())).
                Returns(new PgProductMetaData() { Id = 1, ProductId = updattedProductId });

            productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).
                Returns(new Nop.Core.Domain.Catalog.Product() { Id = updattedProductId });

            productServiceMock.Setup(x => x.UpdateProduct(It.IsAny<Nop.Core.Domain.Catalog.Product>()))
                .Callback<Nop.Core.Domain.Catalog.Product>(x => x.Id = updattedProductId);

            var productSyncTask = new ProductSyncTask(
                productSyncServiceMock.Object,
                productServiceMock.Object,
                pgProductMetaDataServiceMock.Object,
                productTemplateServiceMock.Object,
                logger.Object);

            productSyncTask.Execute();

            productServiceMock.Verify(x => x.UpdateProduct(It.Is<Nop.Core.Domain.Catalog.Product>(product => product.Price == 1000.0M && product.Name == "test product")));
            pgProductMetaDataServiceMock.Verify(x => x.UpdatePgProductMetaData(It.Is<PgProductMetaData>(meta => meta.Code == "1" && meta.ProductId == updattedProductId)));

        }

        [Fact]
        public void DeletedProductsFromPayamGostarAreDeleted()
        {
            var productSyncServiceMock = new Mock<IProductSyncService>();
            var productServiceMock = new Mock<IProductService>();
            var pgProductMetaDataServiceMock = new Mock<IPgProductMetaDataService>();
            var productTemplateServiceMock = new Mock<IProductTemplateService>();
            var logger = new Mock<ILogger>();

            productTemplateServiceMock.Setup(x => x.GetAllProductTemplates())
             .Returns(new List<Nop.Core.Domain.Catalog.ProductTemplate> 
                {
                    new Nop.Core.Domain.Catalog.ProductTemplate() {Id=1, Name="Simple product", DisplayOrder=1, ViewPath=""}
                }
             );

            productSyncServiceMock.Setup(x => x.GetProductChangesSince(It.IsAny<DateTime>()))
                .Returns(new ProductChanges
                {
                    NewProducts = null,

                    EditedProducts = null,

                    DeletedProductCodes = new
                    List<string>
                    {
                        "1"
                    }
                });


            var updattedProductId = 5;

            pgProductMetaDataServiceMock.Setup(x => x.GetPgProductMetaDataByCode(It.IsAny<string>())).
                Returns(new PgProductMetaData() { Id = 1, ProductId = updattedProductId });

            productServiceMock.Setup(x => x.GetProductById(It.IsAny<int>())).
                Returns(new Nop.Core.Domain.Catalog.Product() { Id = updattedProductId });

            productServiceMock.Setup(x => x.UpdateProduct(It.IsAny<Nop.Core.Domain.Catalog.Product>()))
                .Callback<Nop.Core.Domain.Catalog.Product>(x => x.Id = updattedProductId);

            var productSyncTask = new ProductSyncTask(
                productSyncServiceMock.Object,
                productServiceMock.Object,
                pgProductMetaDataServiceMock.Object,
                productTemplateServiceMock.Object,
                logger.Object);

            productSyncTask.Execute();

            productServiceMock.Verify(x => x.UpdateProduct(It.Is<Nop.Core.Domain.Catalog.Product>(product => product.Deleted == true)));
        }
    }
}
