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
    class TargetPageElementDetailMapping : EntityTypeConfiguration<TargetPageElementDetail>
    {
        public TargetPageElementDetailMapping()
        {
            this.ToTable("TargetPageElementDetail");
            this.HasKey(x => x.ID);
            this.Property(x => x.ID).HasColumnName("id");
            this.Property(x => x.ModuleID).HasColumnName("module_id");
            this.Property(x => x.ModuleName).HasColumnName("module_name");
            this.Property(x => x.ModuleDescription).HasColumnName("module_description");
            this.Property(x => x.ModuleIcon).HasColumnName("module_icon");
            this.Property(x => x.PageID).HasColumnName("page_id");
            this.Property(x => x.PageName).HasColumnName("page_name");
            this.Property(x => x.PageDescription).HasColumnName("page_description");
            this.Property(x => x.Area).HasColumnName("area");
            this.Property(x => x.Controler).HasColumnName("controler");
            this.Property(x => x.Action).HasColumnName("action");
            this.Property(x => x.Method).HasColumnName("method");
            this.Property(x => x.ParentPageID).HasColumnName("parent_page_id");
            this.Property(x => x.PageTreePath).HasColumnName("page_tree_path");
            this.Property(x => x.PageTreeLevel).HasColumnName("page_tree_level");
            this.Property(x => x.ElementID).HasColumnName("element_id");
            this.Property(x => x.ElementName).HasColumnName("element_name");
            this.Property(x => x.ElementDescription).HasColumnName("element_description");
            this.Property(x => x.ElementIcon).HasColumnName("element_icon");
            this.Property(x => x.ElementType).HasColumnName("element_type");
            this.Property(x => x.NeedPermissionID).HasColumnName("need_permission_id");
            this.Property(x => x.Selector).HasColumnName("selector");
            this.Property(x => x.BlockMethod).HasColumnName("block_method");
            this.Property(x => x.TargetID).HasColumnName("target_id");
            this.Property(x => x.TargetType).HasColumnName("target_type");
        }
    }
}
