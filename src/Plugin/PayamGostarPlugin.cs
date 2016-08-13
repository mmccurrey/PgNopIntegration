using System;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;
using Nop.Services.Common;
using Nop.Core.Domain.Customers;

namespace Septa.PgNopIntegration.Plugin
{
    /// <summary>
    /// PayamGostar Plugin
    /// </summary>
    public class PayamGostarPlugin : BasePlugin
    {
        # region Fields

        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;

        # endregion

        # region Ctor

        public PayamGostarPlugin(ISettingService settingService, IStoreService storeService, IWorkContext workContext)
        {
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = workContext;
        }

        # endregion

        # region Utilities

        /// <summary>
        /// enables username authenticaion mode instead of email authentication mode
        /// </summary>
        private void EnableUsernameAuthenticationMode()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var customerSettings = _settingService.LoadSetting<CustomerSettings>(storeScope);

            customerSettings.UsernamesEnabled = true;

            _settingService.SaveSetting(customerSettings);
        }

        /// <summary>
        /// gets configuration scope for active store
        /// </summary>
        private int GetActiveStoreScopeConfiguration(IStoreService storeService, IWorkContext workContext)
        {
            //ensure that we have 2 (or more) stores
            if (storeService.GetAllStores().Count < 2)
                return 0;

            var storeId = workContext.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.AdminAreaStoreScopeConfiguration);
            var store = storeService.GetStoreById(storeId);
            return store != null ? store.Id : 0;
        }

        # endregion

        # region Methods

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            EnableUsernameAuthenticationMode();
            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            base.Uninstall();
        }

        # endregion
    }
}
