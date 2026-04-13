using Microsoft.EntityFrameworkCore;
using Testing.Domain.Entities;

namespace Testing.Repositories;

public partial class DbPrintedBoardsContext : DbContext
{
    public DbPrintedBoardsContext()
    {
    }

    public DbPrintedBoardsContext(DbContextOptions<DbPrintedBoardsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ConfirmationCode> ConfirmationCodes { get; set; }

    public virtual DbSet<Country> Countrys { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<PcbSpecification> PcbSpecifications { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserSession> UserSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.Property(e => e.IdCategory)
                .ValueGeneratedNever()
                .HasColumnName("id_category");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<ConfirmationCode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Con_code");

            entity.ToTable("ConfirmationCode");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(6)
                .HasColumnName("code");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.ExpiresIn)
                .HasColumnType("datetime")
                .HasColumnName("expires_in");
            entity.Property(e => e.IdUser).HasColumnName("id_user");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ConfirmationCodes)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_ConfirmationCode_Users");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.IdCountry);

            entity.Property(e => e.IdCountry)
                .ValueGeneratedNever()
                .HasColumnName("id_country");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder);

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("order_number");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_Orders_Products");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_Orders_Users");
        });

        modelBuilder.Entity<PcbSpecification>(entity =>
        {
            entity.HasKey(e => e.IdSpec);

            entity.ToTable("PCB_Specifications");

            entity.Property(e => e.IdSpec).HasColumnName("id_spec");
            entity.Property(e => e.CopperThickness)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("copper_thickness");
            entity.Property(e => e.Height)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("height");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.Layers).HasColumnName("layers");
            entity.Property(e => e.Material)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("material");
            entity.Property(e => e.SolderMaskColor)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("solder_mask_color");
            entity.Property(e => e.SurfaceFinish)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("surface_finish");
            entity.Property(e => e.Thickness)
                .HasColumnType("decimal(4, 2)")
                .HasColumnName("thickness");
            entity.Property(e => e.Width)
                .HasColumnType("decimal(8, 2)")
                .HasColumnName("width");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.PcbSpecifications)
                .HasForeignKey(d => d.IdProduct)
                .HasConstraintName("FK_PCB_Specifications_Products");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct);

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("image_url");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("datetime")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.IdCategory)
                .HasConstraintName("FK_Products_Categories");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.IdImage);

            entity.Property(e => e.IdImage).HasColumnName("id_image");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole);

            entity.Property(e => e.IdRole)
                .ValueGeneratedNever()
                .HasColumnName("id_role");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Email)
                .HasMaxLength(80)
                .HasColumnName("email");
            entity.Property(e => e.IdCountry).HasColumnName("id_country");
            entity.Property(e => e.IdRole).HasColumnName("id_role");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(70)
                .HasColumnName("surname");

            entity.HasOne(d => d.IdCountryNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdCountry)
                .HasConstraintName("FK_Users_Countrys");

            entity.HasOne(d => d.IdRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.IdRole)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<UserSession>(entity =>
        {
            entity.HasKey(e => e.IdSession);

            entity.ToTable("UserSession");

            entity.Property(e => e.IdSession).HasColumnName("id_session");
            entity.Property(e => e.CreatedAt)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Created_At");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .HasColumnName("Refresh_Token");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserSessions)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_UserSession_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
