using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.EF.Mapping
{
    class ModuleMapping : EntityTypeConfiguration<Module>
    {
        public ModuleMapping()
        {
            this.ToTable("module");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.Name).HasColumnName("name");
            this.Property(x => x.CreateDate).HasColumnName("create_date");
            this.Property(x => x.CreatorID).HasColumnName("creator_id");
            this.Property(x => x.ModifyDate).HasColumnName("modify_date");
            this.Property(x => x.ModifyUser).HasColumnName("modify_user");
            this.Property(x => x.OrderNo).HasColumnName("order_no");
            this.Property(x => x.Status).HasColumnName("status");
            this.Property(x => x.Icon).HasColumnName("icon");
            this.Property(x => x.Description).HasColumnName("description");
        }
    }
}
