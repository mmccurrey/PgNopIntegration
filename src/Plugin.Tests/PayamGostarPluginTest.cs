using System;
using Moq;
using Xunit;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Plugins;

namespace Septa.PgNopIntegration.Plugin.Tests
{
    public class PayamGostarPluginTest
    {
        [Fact]
        public void UsernameAuthenticationModeIsEnabledAfterPluginIsInstalled()
        {
            // Arrange
            //var settingServiceMock = new Mock<ISettingService>();
            //var storeServiceMock = new Mock<IStoreService>();
            //var workContextMock = new Mock<IWorkContext>();

            //settingServiceMock.Setup(x => x.LoadSetting<CustomerSettings>(It.IsAny<int>())).
            //    Returns(new CustomerSettings() { UsernamesEnabled = true });

            //storeServiceMock.Setup(x => x.GetAllStores().Count)
            //    .Returns(1);

            //BasePlugin payamGostarPlugin = new PayamGostarPlugin(
            //    settingServiceMock.Object,
            //    storeServiceMock.Object,
            //    workContextMock.Object);

            //// Act
            //payamGostarPlugin.Install();

            //// Assert
            //settingServiceMock.Verify(x => x.LoadSetting<CustomerSettings>(It.IsAny<int>()).UsernamesEnabled == true);
        }
    }
}
