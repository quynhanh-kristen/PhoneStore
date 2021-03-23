using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PhoneStore.Models
{
    public partial class PhoneManagementContext : DbContext
    {
        public PhoneManagementContext()
        {
        }

        public PhoneManagementContext(DbContextOptions<PhoneManagementContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCategory> TblCategory { get; set; }
        public virtual DbSet<TblOrder> TblOrder { get; set; }
        public virtual DbSet<TblOrderDetail> TblOrderDetail { get; set; }
        public virtual DbSet<TblProduct> TblProduct { get; set; }
        public virtual DbSet<TblRole> TblRole { get; set; }
        public virtual DbSet<TblTempCart> TblTempCart { get; set; }
        public virtual DbSet<TblUser> TblUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.ToTable("tblCategory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(30);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<TblOrder>(entity =>
            {
                entity.ToTable("tblOrder");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("createdDate")
                    .HasColumnType("date");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.Payment)
                    .HasColumnName("payment")
                    .HasMaxLength(50);

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasMaxLength(10);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TblOrder)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__tblOrder__idUser__1BFD2C07");
            });

            modelBuilder.Entity<TblOrderDetail>(entity =>
            {
                entity.ToTable("tblOrderDetail");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.BoughtQuantity).HasColumnName("boughtQuantity");

                entity.Property(e => e.IdOrder).HasColumnName("idOrder");

                entity.Property(e => e.IdProduct).HasColumnName("idProduct");

                entity.Property(e => e.Tax).HasColumnName("tax");

                entity.HasOne(d => d.IdOrderNavigation)
                    .WithMany(p => p.TblOrderDetail)
                    .HasForeignKey(d => d.IdOrder)
                    .HasConstraintName("FK__tblOrderD__idOrd__20C1E124");

                entity.HasOne(d => d.IdProductNavigation)
                    .WithMany(p => p.TblOrderDetail)
                    .HasForeignKey(d => d.IdProduct)
                    .HasConstraintName("FK__tblOrderD__idPro__1FCDBCEB");
            });

            modelBuilder.Entity<TblProduct>(entity =>
            {
                entity.ToTable("tblProduct");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Configuration)
                    .HasColumnName("configuration")
                    .HasMaxLength(1000);

                entity.Property(e => e.Cost)
                    .HasColumnName("cost")
                    .HasColumnType("money");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(500);

                entity.Property(e => e.IdCtgPhone).HasColumnName("idCtgPhone");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100);

                entity.Property(e => e.Quantity).HasColumnName("quantity");

                entity.Property(e => e.Rating).HasColumnName("rating");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnName("updatedDate")
                    .HasColumnType("date");

                entity.Property(e => e.UserCreatedId).HasColumnName("userCreatedId");

                entity.HasOne(d => d.IdCtgPhoneNavigation)
                    .WithMany(p => p.TblProduct)
                    .HasForeignKey(d => d.IdCtgPhone)
                    .HasConstraintName("FK__tblProduc__idCtg__182C9B23");

                entity.HasOne(d => d.UserCreated)
                    .WithMany(p => p.TblProduct)
                    .HasForeignKey(d => d.UserCreatedId)
                    .HasConstraintName("FK__tblProduc__userC__1920BF5C");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.ToTable("tblRole");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(20);

                entity.Property(e => e.Status).HasColumnName("status");
            });

            modelBuilder.Entity<TblTempCart>(entity =>
            {
                entity.ToTable("tblTempCart");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IdProduct).HasColumnName("idProduct");

                entity.Property(e => e.IdUser).HasColumnName("idUser");

                entity.Property(e => e.TempQuantity).HasColumnName("tempQuantity");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.TblTempCart)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__tblTempCa__idUse__239E4DCF");
            });

            modelBuilder.Entity<TblUser>(entity =>
            {
                entity.ToTable("tblUser");

                entity.HasIndex(e => e.Phone)
                    .HasName("UQ__tblUser__B43B145F16B7DE27")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .HasColumnName("address")
                    .HasMaxLength(50);

                entity.Property(e => e.Fullname)
                    .HasColumnName("fullname")
                    .HasMaxLength(50);

                entity.Property(e => e.IdRole).HasColumnName("idRole");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.TblUser)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK__tblUser__idRole__1367E606");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
