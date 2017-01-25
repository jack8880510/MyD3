using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.EF.Mapping
{
    class ModulePermissionMapping : EntityTypeConfiguration<ModulePermission>
    {
        public ModulePermissionMapping()
        {
            this.ToTable("module_permission");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.CreateDate).HasColumnName("create_date");
            this.Property(x => x.CreatorID).HasColumnName("creator_id");
            this.Property(x => x.ModifyDate).HasColumnName("modify_date");
            this.Property(x => x.ModifyUser).HasColumnName("modify_user");
            this.Property(x => x.OrderNo).HasColumnName("order_no");
            this.Property(x => x.Status).HasColumnName("status");
            this.Property(x => x.ModuleID).HasColumnName("module_id");
            this.Property(x => x.Area).HasColumnName("area");
            this.Property(x => x.Controler).HasColumnName("controler");
            this.Property(x => x.Action).HasColumnName("action");
            this.Property(x => x.Method).HasColumnName("method");
            this.Property(x => x.ActionName).HasColumnName("action_name");
            this.Property(x => x.ActionDescription).HasColumnName("action_description");
        }
    }
}
