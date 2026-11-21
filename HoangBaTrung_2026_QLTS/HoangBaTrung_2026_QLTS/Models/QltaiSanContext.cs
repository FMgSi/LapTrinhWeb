using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HoangBaTrung_2026_QLTS.Models;

public partial class QltaiSanContext : DbContext
{
    public QltaiSanContext()
    {
    }

    public QltaiSanContext(DbContextOptions<QltaiSanContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LoaiTaiSan> LoaiTaiSans { get; set; }

    public virtual DbSet<TaiSan> TaiSans { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Name=ConnectionStrings:QLTaiSanContext");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<LoaiTaiSan>(entity =>
        {
            entity.HasKey(e => e.MaLoai);

            entity.ToTable("LoaiTaiSan");

            entity.Property(e => e.TenLoai).HasMaxLength(50);
        });

        modelBuilder.Entity<TaiSan>(entity =>
        {
            entity.HasKey(e => e.MaTS);

            entity.ToTable("TaiSan");

            entity.Property(e => e.Anh).HasMaxLength(100);
            entity.Property(e => e.NguyenGia).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.TenTS).HasMaxLength(100);

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.TaiSans)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiSan_LoaiTaiSan");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
