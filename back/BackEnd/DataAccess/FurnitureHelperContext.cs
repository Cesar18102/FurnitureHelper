namespace DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class FurnitureHelperContext : DbContext
    {
        public FurnitureHelperContext()
            : base("name=FurnitureHelperConnection")
        {
        }

        public virtual DbSet<Account> accounts { get; set; }
        public virtual DbSet<AccountExtension> accounts_extensions { get; set; }
        public virtual DbSet<Color> colors { get; set; }
        public virtual DbSet<ConcreteController> concrete_controllers { get; set; }
        public virtual DbSet<ConcretePart> concrete_parts { get; set; }
        public virtual DbSet<FurnitureItemPartsConnection> furniture_item_parts_connections { get; set; }
        public virtual DbSet<FurnitureItem> furniture_items { get; set; }
        public virtual DbSet<ManufacturerSellPosition> manufacturer_sell_positions { get; set; }
        public virtual DbSet<ManufacturerSell> manufacturer_sells { get; set; }
        public virtual DbSet<Material> materials { get; set; }
        public virtual DbSet<Owning> ownings { get; set; }
        public virtual DbSet<PartControllerEmbedRelativePosition> part_controllers_embed_relative_positions { get; set; }
        public virtual DbSet<Part> parts { get; set; }
        public virtual DbSet<PartsConnectionGlue> parts_connection_glues { get; set; }
        public virtual DbSet<TwoPartsConnection> two_parts_connection { get; set; }
        public virtual DbSet<TwoPartsConnectionGlue> two_parts_connection_glues { get; set; }
        public virtual DbSet<UserSellPosition> user_sell_positions { get; set; }
        public virtual DbSet<UserSell> user_sells { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.login)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.pwd)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.first_name)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.last_name)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.accounts_extensions)
                .WithRequired(e => e.accounts)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.ownings)
                .WithRequired(e => e.accounts)
                .HasForeignKey(e => e.account_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountExtension>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<AccountExtension>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<AccountExtension>()
                .HasMany(e => e.manufacturer_sells)
                .WithRequired(e => e.accounts_extensions)
                .HasForeignKey(e => e.accounts_extension_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountExtension>()
                .HasMany(e => e.user_sells)
                .WithRequired(e => e.accounts_extensions)
                .HasForeignKey(e => e.accounts_extension_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccountExtension>()
                .HasMany(e => e.user_sells1)
                .WithRequired(e => e.accounts_extensions1)
                .HasForeignKey(e => e.accounts_extension_other_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.red)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.green)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.blue)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Color>()
                .HasMany(e => e.concrete_parts)
                .WithRequired(e => e.colors)
                .HasForeignKey(e => e.color_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConcreteController>()
                .Property(e => e.mac)
                .IsUnicode(false);

            modelBuilder.Entity<ConcretePart>()
                .HasMany(e => e.concrete_controllers)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConcretePart>()
                .HasMany(e => e.manufacturer_sell_positions)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ConcretePart>()
                .HasMany(e => e.ownings)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id);

            modelBuilder.Entity<ConcretePart>()
                .HasMany(e => e.user_sell_positions)
                .WithRequired(e => e.concrete_parts)
                .HasForeignKey(e => e.concrete_part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FurnitureItemPartsConnection>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<FurnitureItemPartsConnection>()
                .HasMany(e => e.parts_connection_glues)
                .WithRequired(e => e.furniture_item_parts_connections)
                .HasForeignKey(e => e.connection_id);

            modelBuilder.Entity<FurnitureItemPartsConnection>()
                .HasMany(e => e.two_parts_connection)
                .WithRequired(e => e.furniture_item_parts_connections)
                .HasForeignKey(e => e.global_connection_id);

            modelBuilder.Entity<FurnitureItem>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<FurnitureItem>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<FurnitureItem>()
                .HasMany(e => e.furniture_item_parts_connections)
                .WithRequired(e => e.furniture_items)
                .HasForeignKey(e => e.furniture_item_id);

            modelBuilder.Entity<ManufacturerSell>()
                .HasMany(e => e.manufacturer_sell_positions)
                .WithRequired(e => e.manufacturer_sells)
                .HasForeignKey(e => e.manufacturer_sell_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.texture_url)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.concrete_parts)
                .WithRequired(e => e.materials)
                .HasForeignKey(e => e.material_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Material>()
                .HasMany(e => e.colors)
                .WithMany(e => e.materials)
                .Map(m => m.ToTable("materials_possible_colors").MapLeftKey("material_id").MapRightKey("color_id"));

            modelBuilder.Entity<PartControllerEmbedRelativePosition>()
                .HasMany(e => e.concrete_controllers)
                .WithRequired(e => e.part_controllers_embed_relative_positions)
                .HasForeignKey(e => e.embed_position_id);

            modelBuilder.Entity<PartControllerEmbedRelativePosition>()
                .HasMany(e => e.two_parts_connection)
                .WithRequired(e => e.part_controllers_embed_relative_positions)
                .HasForeignKey(e => e.part_controller_id);

            modelBuilder.Entity<PartControllerEmbedRelativePosition>()
                .HasMany(e => e.two_parts_connection1)
                .WithRequired(e => e.part_controllers_embed_relative_positions1)
                .HasForeignKey(e => e.part_controller_other_id);

            modelBuilder.Entity<Part>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Part>()
                .Property(e => e.model_url)
                .IsUnicode(false);

            modelBuilder.Entity<Part>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.concrete_parts)
                .WithRequired(e => e.parts)
                .HasForeignKey(e => e.part_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.part_controllers_embed_relative_positions)
                .WithRequired(e => e.parts)
                .HasForeignKey(e => e.part_id);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.parts_connection_glues)
                .WithOptional(e => e.parts)
                .HasForeignKey(e => e.glue_part_id);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.two_parts_connection_glues)
                .WithOptional(e => e.parts)
                .HasForeignKey(e => e.glue_part_id);

            modelBuilder.Entity<Part>()
                .HasMany(e => e.materials)
                .WithMany(e => e.parts)
                .Map(m => m.ToTable("parts_possible_materials").MapLeftKey("part_id").MapRightKey("material_id"));

            modelBuilder.Entity<PartsConnectionGlue>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<TwoPartsConnection>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<TwoPartsConnection>()
                .HasMany(e => e.two_parts_connection_glues)
                .WithRequired(e => e.two_parts_connection)
                .HasForeignKey(e => e.two_parts_connection_id);

            modelBuilder.Entity<TwoPartsConnectionGlue>()
                .Property(e => e.comment_text)
                .IsUnicode(false);

            modelBuilder.Entity<UserSell>()
                .HasMany(e => e.user_sell_positions)
                .WithRequired(e => e.user_sells)
                .HasForeignKey(e => e.user_sell_id)
                .WillCascadeOnDelete(false);
        }
    }
}
