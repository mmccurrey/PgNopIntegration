using Septa.PgNopIntegration.Plugin.Domain;
using System;
using System.Collections;
using System.Collections.Generic;

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
        /// <param name="Id">PgProduct MetaData identifier</param>
        /// <returns>PgProduct MetaData</returns>
        PgProductMetaData GetPgProductMetaDataById(int Id);

        /// <summary>
        /// Gets PgProduct MetaData
        /// </summary>
        /// <param name="Code">PgProduct MetaData identifier</param>
        /// <returns>PgProduct MetaData</returns>
        PgProductMetaData GetPgProductMetaDataByCode(int Code);

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
        /// Inserts a list of PgProduct MetaData
        /// </summary>
        /// <param name="pgProductMetaDataList">PgProduct MetaData list</param>
        void InsertPgProductMetaData(IEnumerable<PgProductMetaData> pgProductMetaDataList);

        /// <summary>
        /// Updates the PgProduct MetaData
        /// </summary>
        /// <param name="pgProductMetaData">PgProduct MetaData</param>
        void UpdatePgProductMetaData(PgProductMetaData pgProductMetaData);

        /// <summary>
        /// Updates a list of PgProduct MetaData
        /// </summary>
        /// <param name="pgProductMetaDataList">PgProduct MetaData list</param>
        void UpdatePgProductMetaData(IEnumerable<PgProductMetaData> pgProductMetaDataList);
    }
}
