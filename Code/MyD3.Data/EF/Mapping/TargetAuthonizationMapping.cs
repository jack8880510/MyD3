﻿using MyD3.Entity.DBEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.EF.Mapping
{
    class TargetAuthonizationMapping : EntityTypeConfiguration<TargetAuthonization>
    {
        public TargetAuthonizationMapping()
        {
            this.ToTable("target_authorization");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.CreateDate).HasColumnName("create_date");
            this.Property(x => x.CreatorID).HasColumnName("creator_id");
            this.Property(x => x.ModifyDate).HasColumnName("modify_date");
            this.Property(x => x.ModifyUser).HasColumnName("modify_user");
            this.Property(x => x.OrderNo).HasColumnName("order_no");
            this.Property(x => x.Status).HasColumnName("status");
            this.Property(x => x.TargetType).HasColumnName("target_type");
            this.Property(x => x.TargetID).HasColumnName("target_id");
            this.Property(x => x.ModulePermissionID).HasColumnName("module_permission_id");
        }
    }
}
