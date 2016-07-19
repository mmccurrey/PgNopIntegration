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
        public void GetPgProductMetaDataById()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);

            // Act
            var result = target.GetPgProductMetaDataById(1);

            // Assert
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public void GetPgProductMetaDataByCode()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);

            // Act
            var result = target.GetPgProductMetaDataByCode("2");

            // Assert
            Assert.Equal("2", result.Code);
        }

        [Fact]
        public void GetPgProductMetaDataByCodes()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);

            // Act
            var result = target.GetPgProductMetaDataByCodes(new List<string> { "1", "3" });

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("1", result.FirstOrDefault().Code);
            Assert.Equal("3", result.LastOrDefault().Code);
        }

        [Fact]
        public void InsertPgProductMetaDataTest()
        {
            // Arrange
            var insertedProductId = 1;
            _pgProductMetaDataRepository.Setup(x => x.Insert(It.IsAny<PgProductMetaData>()))
                .Callback<PgProductMetaData>(y => y.Id = insertedProductId);
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);

            // Act
            target.InsertPgProductMetaData(new PgProductMetaData() { Id = 1, Code = "1" });

            // Assert
            _pgProductMetaDataRepository.Verify(x => x.Insert(It.Is<PgProductMetaData>(y => y.Id == 1 && y.Code == "1")));

        }

        [Fact]
        public void UpdatePgProductMetaData()
        {
            // Arrange
            var target = new PgProductMetaDataService(_pgProductMetaDataRepository.Object);
            var initialProduct = target.GetPgProductMetaDataById(1);
            initialProduct.Code = "10";

            // Act
            target.UpdatePgProductMetaData(initialProduct);

            var updateProduct = target.GetPgProductMetaDataById(1);

            // Assert
            Assert.Equal(initialProduct.Id, updateProduct.Id);
            Assert.Equal(initialProduct.Code, updateProduct.Code);
        }

        [Fact]
        public void UpdatePgProductMetaDataList()
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
            Assert.Equal(initialPgProductMetaData1.Id, updatepgProductMetaDataList.FirstOrDefault().Id);
            Assert.Equal(initialPgProductMetaData2.Id, updatepgProductMetaDataList.LastOrDefault().Id);

            Assert.Equal(initialPgProductMetaData1.Code, updatepgProductMetaDataList.FirstOrDefault().Code);
            Assert.Equal(initialPgProductMetaData2.Code, updatepgProductMetaDataList.LastOrDefault().Code);
        }

        # endregion

    }
}
