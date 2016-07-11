using Septa.PgNopIntegration.Plugin.Domain;
using System;

namespace Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog
{
    /// <summary>
    /// PgProduct Metadata service interface
    /// </summary>
    public partial interface IPgProductMetaDataService
    {
        /// <summary>
        /// Gets PgProduct MetaData
        /// </summary>
        /// <param name="Code">PgProduct MetaData identifier</param>
        /// <returns>PgProduct MetaData</returns>
        PgProductMetaData GetPgProductMetaDataById(int Id);

        /// <summary>
        /// Deletes a PgProduct MetaData
        /// </summary>
        /// <param name="pgProductMetaData">PgProduct MetaData</param>
        void DeletePgProductMetaData(PgProductMetaData pgProductMetaData);

        /// <summary>
        /// Inserts a PgProduct MetaData
        /// </summary>
        /// <param name="pgProductMetaData">PgProduct MetaData</param>
        void InsertPgProductMetaData(PgProductMetaData pgProductMetaData);

        /// <summary>
        /// Updates the PgProduct MetaData
        /// </summary>
        /// <param name="pgProductMetaData">PgProduct MetaData</param>
        void UpdatePgProductMetaData(PgProductMetaData pgProductMetaData);
    }
}
