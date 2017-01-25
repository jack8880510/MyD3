using MyD3.Entity.DBEntity;
using MyD3.Entity.DBView;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.EF.Mapping
{
    class DictionaryDataDetailMapping : EntityTypeConfiguration<DictionaryDataDetail>
    {
        public DictionaryDataDetailMapping()
        {
            this.ToTable("DictionaryDataDetail");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.DataName).HasColumnName("data_name");
            this.Property(x => x.DataValue).HasColumnName("data_value");
            this.Property(x => x.Index).HasColumnName("index");
            this.Property(x => x.IndexName).HasColumnName("index_name");
            this.Property(x => x.IsDefault).HasColumnName("is_default");
        }
    }
}
