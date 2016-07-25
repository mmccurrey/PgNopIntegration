using System;
using Nop.Core.Plugins;
using Nop.Services.Configuration;
using Nop.Services.Stores;
using Nop.Core;

namespace Septa.PgNopIntegration.Plugin
{
    public class PgNopIntegrationPlugin : BasePlugin
    {
        # region Fields

        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        # endregion

        # region Ctor

        public PgNopIntegrationPlugin(ISettingService settingService, IStoreService storeService, IWorkContext _workContext)
        {
            this._settingService = settingService;
            this._storeService = storeService;
            this._workContext = _workContext;
        }

        # endregion

        # region Utilities

        private void EnableUsernameAuthentication()
        {
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var customerSettings = _settingService.LoadSetting<CustomerSettings>(storeScope);
        }

        # endregion

        # region Method

        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            this.EnableUsernameAuthentication();

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
