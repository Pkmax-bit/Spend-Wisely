using System;
using System.Collections.Generic;
using Chitieu.Models;
using Microsoft.EntityFrameworkCore;

namespace Chitieu.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTieu> ChiTieus { get; set; }

    public virtual DbSet<NguoiChiTieu> NguoiChiTieus { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-0U55SOV\\SQLEXPRESS;Initial Catalog=SpendWisely;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTieu__3213E83F84731EA1");

            entity.ToTable("ChiTieu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(500)
                .HasColumnName("ghichu");
            entity.Property(e => e.GiaTien)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("gia_tien");
            entity.Property(e => e.IdTheloai).HasColumnName("id_theloai");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Ngaygio)
                .HasColumnType("datetime")
                .HasColumnName("ngaygio");

            entity.HasOne(d => d.IdTheloaiNavigation).WithMany(p => p.ChiTieus)
                .HasForeignKey(d => d.IdTheloai)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__ChiTieu__id_thel__5CD6CB2B");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.ChiTieus)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__ChiTieu__id_user__5BE2A6F2");
        });

        modelBuilder.Entity<NguoiChiTieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NguoiChi__3213E83F00307801");

            entity.ToTable("NguoiChiTieu");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(255)
                .HasColumnName("ghichu");
            entity.Property(e => e.Hinhanh)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("hinhanh");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sdt");
            entity.Property(e => e.Tennguoichitieu)
                .HasMaxLength(255)
                .HasColumnName("tennguoichitieu");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.NguoiChiTieus)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__NguoiChiT__id_us__59063A47");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TheLoai__3213E83F01AF6879");

            entity.ToTable("TheLoai");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Ghichu)
                .HasMaxLength(255)
                .HasColumnName("ghichu");
            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.Kichhoat)
                .HasDefaultValue(true)
                .HasColumnName("kichhoat");
            entity.Property(e => e.Tentheloai)
                .HasMaxLength(255)
                .HasColumnName("tentheloai");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.TheLoais)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK__TheLoai__id_user__5629CD9C");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F56A12B85");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E616493D3D655").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Hinhanh)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("hinhanh");
            entity.Property(e => e.Hoten)
                .HasMaxLength(255)
                .HasColumnName("hoten");
            entity.Property(e => e.Kichhoat)
                .HasDefaultValue(true)
                .HasColumnName("kichhoat");
            entity.Property(e => e.Lastlogin)
                .HasColumnType("datetime")
                .HasColumnName("lastlogin");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Random)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("random");
            entity.Property(e => e.Sdt)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("sdt");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
