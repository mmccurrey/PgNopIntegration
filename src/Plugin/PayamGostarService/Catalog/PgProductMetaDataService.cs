﻿using System;
using System.Linq;
using System.Transactions;
using Nop.Core.Data;
using Septa.PgNopIntegration.Plugin.Domain;
using System.Collections.Generic;
using System.Transactions;
using Septa.PgNopIntegration.Plugin.PayamGostarService.Extensions;

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

        public virtual PgProductMetaData GetPgProductMetaDataById(int id)
        {
            if (id <= 0)
                return null;

            var query = from p in _pgProductMetaDataRepository.Table
                        where p.Id == id
                        select p;

            var pgProductMetaData = query.FirstOrDefault();

            return pgProductMetaData;
        }

        public virtual PgProductMetaData GetPgProductMetaDataByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return null;

            var query = from p in _pgProductMetaDataRepository.Table
                        where p.Code == code
                        select p;

            PgProductMetaData pgProductMetaData = query.FirstOrDefault();

            return pgProductMetaData;
        }

        public virtual IEnumerable<PgProductMetaData> GetPgProductMetaDataByCodes(List<string> codes)
        {
            if (codes.IsNullOrEmpty())
                return null;

            var query = from p in _pgProductMetaDataRepository.Table
                        where codes.Contains(p.Code)
                        select p;

            IEnumerable<PgProductMetaData> pgProductMetaDataList = query.ToList();

            return pgProductMetaDataList;
        }

        public DateTime GetLastSyncDate()
        {
            return _pgProductMetaDataRepository.Table.Max(p => p.LastSyncDate);
        }

        public virtual void InsertPgProductMetaData(PgProductMetaData pgProductMetaData)
        {
            if (pgProductMetaData == null)
                throw new ArgumentNullException("pgProductMetaData");

            _pgProductMetaDataRepository.Insert(pgProductMetaData);

        }

        public virtual void InsertPgProductMetaData(IEnumerable<PgProductMetaData> pgProductMetaDataList)
        {
            if (pgProductMetaDataList.IsNullOrEmpty())
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
            if (pgProductMetaDataList.IsNullOrEmpty())
                throw new ArgumentNullException("pgProductMetaDataList");

            _pgProductMetaDataRepository.Update(pgProductMetaDataList);
        }
        # endregion

    }
}
