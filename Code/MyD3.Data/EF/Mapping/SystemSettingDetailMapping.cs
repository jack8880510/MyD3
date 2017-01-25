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
    class SystemSettingDetailMapping : EntityTypeConfiguration<SystemSettingDetail>
    {
        public SystemSettingDetailMapping()
        {
            this.ToTable("SystemSettingDetail");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.GroupID).HasColumnName("group_id");
            this.Property(x => x.GroupName).HasColumnName("group_name");
            this.Property(x => x.GroupOrderNo).HasColumnName("group_order_no");
            this.Property(x => x.Name).HasColumnName("name");
            this.Property(x => x.DisplayName).HasColumnName("display_name");
            this.Property(x => x.CreateDate).HasColumnName("create_date");
            this.Property(x => x.CreatorID).HasColumnName("creator_id");
            this.Property(x => x.Description).HasColumnName("description");
            this.Property(x => x.MaxLength).HasColumnName("max_length");
            this.Property(x => x.MinLength).HasColumnName("min_length");
            this.Property(x => x.ModifyDate).HasColumnName("modify_date");
            this.Property(x => x.ModifyUser).HasColumnName("modify_user");
            this.Property(x => x.OptionText).HasColumnName("option_text");
            this.Property(x => x.OptionType).HasColumnName("option_type");
            this.Property(x => x.OptionValue).HasColumnName("option_value");
            this.Property(x => x.OrderNo).HasColumnName("order_no");
            this.Property(x => x.Required).HasColumnName("required");
            this.Property(x => x.Status).HasColumnName("status");
            this.Property(x => x.TextReg).HasColumnName("text_reg");
            this.Property(x => x.Value).HasColumnName("value");
            this.Property(x => x.ValueType).HasColumnName("value_type");
        }
    }
}
