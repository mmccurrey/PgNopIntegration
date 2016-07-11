using System;
using System.Linq;
using Nop.Core.Data;
using Septa.PgNopIntegration.Plugin.Domain;
using System.Collections.Generic;

namespace Septa.PgNopIntegration.Plugin.PayamGostarService.Catalog
{
    public partial class PgProductMetaDataService : IPgProductMetaDataService
    {
        # region Properties

        private readonly IRepository<PgProductMetaData> _pgProductMetaDataRepository;

        # endregion

        # region Ctor

        public PgProductMetaDataService(IRepository<PgProductMetaData> pgProductMetaDataRepository)
        {
            this._pgProductMetaDataRepository = pgProductMetaDataRepository;
        }

        # endregion

        # region Implementation of IPgProductMetaDataService

        public virtual PgProductMetaData GetPgProductMetaDataById(int Id)
        {
            if (Id <= 0)
                return null;

            var query = from p in _pgProductMetaDataRepository.Table
                        where p.Code == Id
                        select p;

            var pgProductMetaData = query.FirstOrDefault();

            return pgProductMetaData;
        }

        public virtual PgProductMetaData GetPgProductMetaDataByCode(int Code)
        {
            if (Code <= 0)
                return null;

            var query = from  p in _pgProductMetaDataRepository.Table
                        where p.Code==Code
                        select p;

            var pgProductMetaData = query.FirstOrDefault();

            return pgProductMetaData;
        }

        public virtual void DeletePgProductMetaData(PgProductMetaData pgProductMetaData)
        {
            // because Product entity has one-to-one relationship with PgProductMetadata Entity virtually, it is not necessary to impelment delete method.

            throw new NotImplementedException();
        }

        public virtual void InsertPgProductMetaData(PgProductMetaData pgProductMetaData)
        {
            if (pgProductMetaData == null)
                throw new ArgumentNullException("pgProductMetaData");

            _pgProductMetaDataRepository.Insert(pgProductMetaData);
        }

        public virtual void InsertPgProductMetaData(IEnumerable<PgProductMetaData> pgProductMetaDataList)
        {
            if (pgProductMetaDataList == null && !pgProductMetaDataList.Any())
                throw new ArgumentNullException("pgProductMetaDataList");

            _pgProductMetaDataRepository.Insert(pgProductMetaDataList);
        }

        public virtual void UpdatePgProductMetaData(PgProductMetaData pgProductMetaData)
        {
            if (pgProductMetaData == null)
                throw new ArgumentNullException("pgProductMetaData");

            _pgProductMetaDataRepository.Update(pgProductMetaData);
        }

        public virtual void UpdatePgProductMetaData(IEnumerable<PgProductMetaData> pgProductMetaDataList)
        {
            if (pgProductMetaDataList == null && !pgProductMetaDataList.Any())
                throw new ArgumentNullException("pgProductMetaDataList");

            _pgProductMetaDataRepository.Update(pgProductMetaDataList);
        }
        # endregion
    }
}
