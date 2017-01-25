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
    class TargetAuthonizationDetailMapping : EntityTypeConfiguration<TargetAuthonizationDetail>
    {
        public TargetAuthonizationDetailMapping()
        {
            this.ToTable("TargetAuthrizationDetail");
            this.HasKey(x => x.ID);
            this.Property(x => x.ModuleID).HasColumnName("module_id");
            this.Property(x => x.ModuleName).HasColumnName("module_name");
            this.Property(x => x.PermissionID).HasColumnName("permission_id");
            this.Property(x => x.PermissionName).HasColumnName("permission_name");
            this.Property(x => x.Area).HasColumnName("area");
            this.Property(x => x.Controler).HasColumnName("controler");
            this.Property(x => x.Action).HasColumnName("action");
            this.Property(x => x.Method).HasColumnName("method");
            this.Property(x => x.ActionName).HasColumnName("action_name");
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.TargetID).HasColumnName("target_id");
            this.Property(x => x.TargetType).HasColumnName("target_type");
        }
    }
}
