namespace DataAccess
{
    using System;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Entities;
    using DataAccess.Exceptions;

    public partial class FurnitureHelperContext : DbContext
    {
        public FurnitureHelperContext()
            : base("name=FurnitureHelperConnection") { }

        public virtual DbSet<AccountEntity> accounts { get; set; }
        public virtual DbSet<AdminEntity> admins { get; set; }
        public virtual DbSet<SuperAdminEntity> super_admins { get; set; }
        public virtual DbSet<AccountExtensionEntity> accounts_extensions { get; set; }
        public virtual DbSet<PartColorEntity> colors { get; set; }
        public virtual DbSet<ConcretePartEntity> concrete_parts { get; set; }
        public virtual DbSet<FurnitureItemPartsConnectionEntity> furniture_item_parts_connections { get; set; }
        public virtual DbSet<FurnitureItemEntity> furniture_items { get; set; }
        public virtual DbSet<ManufacturerSellPositionEntity> manufacturer_sell_positions { get; set; }
        public virtual DbSet<ManufacturerSellEntity> manufacturer_sells { get; set; }
        public virtual DbSet<MaterialEntity> materials { get; set; }
        public virtual DbSet<OwningEntity> ownings { get; set; }
        public virtual DbSet<PartControllerEmbedRelativePositionEntity> part_controllers_embed_relative_positions { get; set; }
        public virtual DbSet<PartEntity> parts { get; set; }
        public virtual DbSet<PartsConnectionGlueEntity> parts_connection_glues { get; set; }
        public virtual DbSet<TwoPartsConnectionEntity> two_parts_connection { get; set; }
        public virtual DbSet<TwoPartsConnectionGlueEntity> two_parts_connection_glues { get; set; }
        public virtual DbSet<UsedPartEntity> used_parts { get; set; }
        public virtual DbSet<UserSellPositionEntity> user_sell_positions { get; set; }
        public virtual DbSet<UserSellEntity> user_sells { get; set; }

        public override int SaveChanges()
        {
            IEnumerable<DbEntityEntry> changedEntries = ChangeTracker.Entries().Where(entry => 
                entry.State == EntityState.Added || 
                entry.State == EntityState.Modified
            );

            ICollection<ValidationResult> errors = new List<ValidationResult>();

            foreach (DbEntityEntry entry in changedEntries)
                Validator.TryValidateObject(entry.Entity, new ValidationContext(entry.Entity), errors, true);

            if (errors.Count != 0)
            {
                foreach (DbEntityEntry entry in changedEntries)
                {
                    if(entry.State == EntityState.Added)
                        entry.State = EntityState.Detached;
                    else if(entry.State == EntityState.Modified)
                    {
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                    }
                }

                throw new DatabaseActionValidationException(errors);
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>()
                .ToTable("accounts");

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.pwd)
                .IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.first_name);
                //.IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .Property(e => e.last_name);
                //.IsUnicode(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.accounts_extensions)
                .WithRequired(e => e.accounts)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.admins)
                .WithRequired(e => e.accounts)
                .HasForeignKey(e => e.account_id);

            modelBuilder.Entity<AccountEntity>()
                .HasMany(e => e.ownings)
                .WithRequired(e => e.accounts)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AdminEntity>()
                .ToTable("admins");

            modelBuilder.Entity<AdminEntity>()
                .HasMany(e => e.super_admins)
                .WithRequired(e => e.admins)
                .HasForeignKey(e => e.admin_account_id);

            modelBuilder.Entity<SuperAdminEntity>()
                .ToTable("super_admins");

            modelBuilder.Entity<AccountExtensionEntity>()
                .ToTable("accounts_extensions");

            modelBuilder.Entity<AccountExtensionEntity>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<AccountExtensionEntity>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<AccountExtensionEntity>()
                .HasMany(e => e.manufacturer_sells)
                .WithRequired(e => e.accounts_extensions)
                .HasForeignKey(e => e.accounts_extension_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountExtensionEntity>()
                .HasMany(e => e.user_sells)
                .WithRequired(e => e.accounts_extensions)
                .HasForeignKey(e => e.accounts_extension_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountExtensionEntity>()
                .HasMany(e => e.user_sells1)
                .WithRequired(e => e.accounts_extensions1)
                .HasForeignKey(e => e.accounts_extension_other_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PartColorEntity>()
                .ToTable("colors");

            modelBuilder.Entity<PartColorEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<PartColorEntity>()
                .Property(e => e.hex);
                //.IsUnicode(false);

            modelBuilder.Entity<PartColorEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<PartColorEntity>()
                .HasMany(e => e.concrete_parts)
                .WithRequired(e => e.colors)
                .HasForeignKey(e => e.color_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConcretePartEntity>()
                .ToTable("concrete_parts");

            modelBuilder.Entity<ConcretePartEntity>()
                .HasMany(e => e.manufacturer_sell_positions)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConcretePartEntity>()
                .HasMany(e => e.ownings)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id);

            modelBuilder.Entity<ConcretePartEntity>()
                .HasMany(e => e.user_sell_positions)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FurnitureItemPartsConnectionEntity>()
                .ToTable("furniture_item_parts_connections");

            modelBuilder.Entity<FurnitureItemPartsConnectionEntity>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<FurnitureItemPartsConnectionEntity>()
                .HasMany(e => e.parts_connection_glues)
                .WithRequired(e => e.furniture_item_parts_connections)
                .HasForeignKey(e => e.connection_id);

            modelBuilder.Entity<FurnitureItemPartsConnectionEntity>()
                .HasMany(e => e.two_parts_connection)
                .WithRequired(e => e.furniture_item_parts_connections)
                .HasForeignKey(e => e.global_connection_id);

            modelBuilder.Entity<FurnitureItemEntity>()
                .ToTable("furniture_items");

            modelBuilder.Entity<FurnitureItemEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<FurnitureItemEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<FurnitureItemEntity>()
                .HasMany(e => e.furniture_item_parts_connections)
                .WithRequired(e => e.furniture_items)
                .HasForeignKey(e => e.furniture_item_id);

            modelBuilder.Entity<FurnitureItemEntity>()
                .HasMany(e => e.used_parts)
                .WithRequired(e => e.furniture_items)
                .HasForeignKey(e => e.furniture_item_id);

            modelBuilder.Entity<ManufacturerSellEntity>()
                .ToTable("manufacturer_sells");

            modelBuilder.Entity<ManufacturerSellEntity>()
                .HasMany(e => e.manufacturer_sell_positions)
                .WithRequired(e => e.manufacturer_sells)
                .HasForeignKey(e => e.manufacturer_sell_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ManufacturerSellPositionEntity>()
                .ToTable("manufacturer_sell_positions");

            modelBuilder.Entity<MaterialEntity>()
                .ToTable("materials");

            modelBuilder.Entity<MaterialEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<MaterialEntity>()
                .Property(e => e.texture_url)
                .IsUnicode(false);

            modelBuilder.Entity<MaterialEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<MaterialEntity>()
                .HasMany(e => e.concrete_parts)
                .WithRequired(e => e.materials)
                .HasForeignKey(e => e.material_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MaterialEntity>()
                .HasMany(e => e.colors)
                .WithMany(e => e.materials)
                .Map(m => m.ToTable("materials_possible_colors").MapLeftKey("material_id").MapRightKey("color_id"));

            modelBuilder.Entity<PartControllerEmbedRelativePositionEntity>()
                .ToTable("part_controllers_embed_relative_positions");

            modelBuilder.Entity<PartControllerEmbedRelativePositionEntity>()
                .HasMany(e => e.two_parts_connection)
                .WithRequired(e => e.part_controllers_embed_relative_positions)
                .HasForeignKey(e => e.part_controller_id);

            modelBuilder.Entity<PartControllerEmbedRelativePositionEntity>()
                .HasMany(e => e.two_parts_connection1)
                .WithRequired(e => e.part_controllers_embed_relative_positions1)
                .HasForeignKey(e => e.part_controller_other_id);

            modelBuilder.Entity<PartEntity>()
                .ToTable("parts");

            modelBuilder.Entity<PartEntity>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<PartEntity>()
                .Property(e => e.model_url)
                .IsUnicode(false);

            modelBuilder.Entity<PartEntity>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<PartEntity>()
                .HasMany(e => e.concrete_parts)
                .WithRequired(e => e.parts)
                .HasForeignKey(e => e.part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PartEntity>()
                .HasMany(e => e.part_controllers_embed_relative_positions)
                .WithRequired(e => e.parts)
                .HasForeignKey(e => e.part_id);

            modelBuilder.Entity<PartEntity>()
                .HasMany(e => e.parts_connection_glues)
                .WithOptional(e => e.parts)
                .HasForeignKey(e => e.glue_part_id);

            modelBuilder.Entity<PartEntity>()
                .HasMany(e => e.two_parts_connection_glues)
                .WithOptional(e => e.parts)
                .HasForeignKey(e => e.glue_part_id);

            modelBuilder.Entity<PartEntity>()
                .HasMany(e => e.materials)
                .WithMany(e => e.parts)
                .Map(m => m.ToTable("parts_possible_materials").MapLeftKey("part_id").MapRightKey("material_id"));

            modelBuilder.Entity<PartEntity>()
                .HasMany(e => e.used_parts)
                .WithOptional(e => e.parts)
                .HasForeignKey(e => e.part_id);

            modelBuilder.Entity<PartsConnectionGlueEntity>()
                .ToTable("parts_connection_glues");

            modelBuilder.Entity<PartsConnectionGlueEntity>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<TwoPartsConnectionEntity>()
                .ToTable("two_parts_connection");

            modelBuilder.Entity<TwoPartsConnectionEntity>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<TwoPartsConnectionEntity>()
                .HasMany(e => e.two_parts_connection_glues)
                .WithRequired(e => e.two_parts_connection)
                .HasForeignKey(e => e.two_parts_connection_id);

            modelBuilder.Entity<TwoPartsConnectionGlueEntity>()
                .ToTable("two_parts_connection_glues");

            modelBuilder.Entity<TwoPartsConnectionGlueEntity>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<UserSellEntity>()
                .ToTable("user_sells");

            modelBuilder.Entity<UserSellEntity>()
                .HasMany(e => e.user_sell_positions)
                .WithRequired(e => e.user_sells)
                .HasForeignKey(e => e.user_sell_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserSellPositionEntity>()
                .ToTable("user_sell_positions");

            modelBuilder.Entity<OwningEntity>()
                .ToTable("ownings");

            modelBuilder.Entity<UsedPartEntity>()
                .ToTable("used_parts");

            modelBuilder.Entity<UsedPartEntity>()
                .HasMany(e => e.two_parts_connection)
                .WithRequired(e => e.used_parts)
                .HasForeignKey(e => e.used_part_id);

            modelBuilder.Entity<UsedPartEntity>()
                .HasMany(e => e.two_parts_connection1)
                .WithRequired(e => e.used_parts1)
                .HasForeignKey(e => e.used_part_other_id);
        }
    }
}
