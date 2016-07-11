using Septa.PgNopIntegration.Plugin.Domain;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Septa.PgNopIntegration.Plugin.Data
{
    public partial class PgProductMetaDataMap : EntityTypeConfiguration<PgProductMetaData>
    {
        public PgProductMetaDataMap()
        {
            // Table & Collumn Mappings
            this.ToTable("PgProductMetaData");
            this.Property(p => p.ProductId).HasColumnName("ProductId");
            this.Property(p => p.Code).HasColumnName("Code");
            this.Property(p => p.LastSyncDate).HasColumnName("LastSyncDate");


            // Primary Key
            this.HasKey(p => p.Id);
        }
    }
}
