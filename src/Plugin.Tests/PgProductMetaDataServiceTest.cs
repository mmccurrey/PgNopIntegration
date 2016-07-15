using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Moq;
using Nop.Core.Data;
using Septa.PgNopIntegration.Plugin.Domain;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog;

namespace Septa.PgNopIntegration.Plugin.Tests
{
    public class PgProductMetaDataServiceTest
    {
        # region Fields

        private Mock<IRepository<PgProductMetaData>> _pgProductMetaDataRepository;
        public List<PgProductMetaData> pgProductMetaDataGetList { get; set; }

        # endregion

        # region Ctor

        public PgProductMetaDataServiceTest()
        {
            pgProductMetaDataGetList = new List<PgProductMetaData>
            {
                new PgProductMetaData() { Id = 1, Code = "1", ProductId = 1, LastSyncDate = DateTime.UtcNow },
                new PgProductMetaData() { Id = 2, Code = "2", ProductId = 2, LastSyncDate = DateTime.UtcNow },
                new PgProductMetaData() { Id = 3, Code = "3", ProductId = 3, LastSyncDate = DateTime.UtcNow },
            };

            _pgProductMetaDataRepository = new Mock<IRepository<PgProductMetaData>>();
            _pgProductMetaDataRepository.SetupGet(m => m.Table).Returns(pgProductMetaDataGetList.AsQueryable());
        }

        # endregion

        # region Facts

        [Fact]
        public void GetPgProductMetaDataByIdTest()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);
            var initialPgProductMetaData = new PgProductMetaData() { Id = 1, Code = "1", ProductId = 1, LastSyncDate = DateTime.UtcNow };

            // Act
            var result = target.GetPgProductMetaDataByCode("1");

            // Assert
            Assert.Equal(initialPgProductMetaData, result);
            Assert.Equal(initialPgProductMetaData.Id, result.Id);
        }

        [Fact]
        public void GetPgProductMetaDataByCodeTest()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);
            var initialPgProductMetaData = new PgProductMetaData() { Id = 2, Code = "2", ProductId = 2, LastSyncDate = DateTime.UtcNow };

            // Act
            var result = target.GetPgProductMetaDataByCode("2");

            // Assert
            Assert.Equal(initialPgProductMetaData, result);
            Assert.Equal(initialPgProductMetaData.Code, result.Code);
        }

        [Fact]
        public void GetPgProductMetaDataByCodesTest()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);
            var initialPgProductMetaDataList = new List<PgProductMetaData>
            {
                new PgProductMetaData() { Id = 1, Code = "1", ProductId = 1, LastSyncDate = DateTime.UtcNow },
                new PgProductMetaData() { Id = 3, Code = "3", ProductId = 3, LastSyncDate = DateTime.UtcNow },
            };

            // Act
            var result = target.GetPgProductMetaDataByCodes(new List<string> { "1", "3" });

            // Assert
            Assert.NotNull(initialPgProductMetaDataList);

            Assert.Equal(initialPgProductMetaDataList, result);

            Assert.Equal(initialPgProductMetaDataList.FirstOrDefault().Id, result.FirstOrDefault().Id);
            Assert.Equal(initialPgProductMetaDataList.FirstOrDefault().Code, result.FirstOrDefault().Code);
            Assert.Equal(initialPgProductMetaDataList.FirstOrDefault().ProductId, result.FirstOrDefault().ProductId);
        }

        //[Fact]
        public void InsertPgProductMetaDataTest()
        {
            throw new NotImplementedException();
        }

        [Fact]
        public void UpdatePgProductMetaDataTest()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);
            var initialProduct = target.GetPgProductMetaDataById(1);
            initialProduct.Code = "10";

            // Act
            target.UpdatePgProductMetaData(initialProduct);

            var updateProduct = target.GetPgProductMetaDataById(1);

            // Assert
            Assert.Equal(initialProduct.Code, updateProduct.Code);
            Assert.Equal(initialProduct.Id, updateProduct.Id);
            Assert.Equal(initialProduct.LastSyncDate, updateProduct.LastSyncDate);

            Assert.Equal("10", updateProduct.Code);

            Assert.True(initialProduct.Code == "10");
            Assert.True(updateProduct.Code == "10");
        }

        [Fact]
        public void UpdatePgProductMetaDataListTest()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);

            var initialPgProductMetaData1 = target.GetPgProductMetaDataById(2);
            initialPgProductMetaData1.ProductId = 15;

            var initialPgProductMetaData2 = target.GetPgProductMetaDataById(3);
            initialPgProductMetaData2.ProductId = 20;

            var initialPgProductMetaDataList = new List<PgProductMetaData>();
            initialPgProductMetaDataList.Add(initialPgProductMetaData1);
            initialPgProductMetaDataList.Add(initialPgProductMetaData2);

            // Act
            target.UpdatePgProductMetaData(initialPgProductMetaDataList);

            var updatepgProductMetaDataList = target.GetPgProductMetaDataByCodes(new List<string> { "2", "3" });

            // Assert
            Assert.NotNull(initialPgProductMetaDataList);
            Assert.NotNull(updatepgProductMetaDataList);

            Assert.Equal(initialPgProductMetaData1.Code, updatepgProductMetaDataList.FirstOrDefault().Code);
            Assert.Equal(initialPgProductMetaData2.Code, updatepgProductMetaDataList.LastOrDefault().Code);

            Assert.Equal(initialPgProductMetaData1.ProductId, updatepgProductMetaDataList.FirstOrDefault().ProductId);
            Assert.Equal(initialPgProductMetaData2.ProductId, updatepgProductMetaDataList.LastOrDefault().ProductId);

            Assert.Equal(15, updatepgProductMetaDataList.FirstOrDefault().ProductId);
            Assert.Equal(20, updatepgProductMetaDataList.LastOrDefault().ProductId);
        }

        # endregion

    }
}
