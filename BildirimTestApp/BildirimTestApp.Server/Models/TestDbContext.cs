using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BildirimTestApp.Server.Models
{
    public partial class TestDbContext : DbContext
    {
        public virtual DbSet<Gorev> Gorevs { get; set; } = null!;
        public virtual DbSet<SisBildirim> SisBildirims { get; set; } = null!;
        public virtual DbSet<SisBildirimIcerik> SisBildirimIceriks { get; set; } = null!;
        public virtual DbSet<SisBildirimOutbox> SisBildirimOutboxes { get; set; } = null!;
        public virtual DbSet<SisKullanici> SisKullanicis { get; set; } = null!;

        public TestDbContext()
        {
        }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=PC_4179\\SQLEXPRESS;Initial Catalog=TestDb;Integrated Security=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gorev>(entity =>
            {
                entity.HasMany(d => d.SisKullanicisKullanicis)
                    .WithMany(p => p.GorevsGorevs)
                    .UsingEntity<Dictionary<string, object>>(
                        "SisKullaniciGorev",
                        l => l.HasOne<SisKullanici>().WithMany().HasForeignKey("SisKullanicisKullaniciId"),
                        r => r.HasOne<Gorev>().WithMany().HasForeignKey("GorevsGorevId"),
                        j =>
                        {
                            j.HasKey("GorevsGorevId", "SisKullanicisKullaniciId");

                            j.ToTable("SisKullaniciGorevs");

                            j.HasIndex(new[] { "SisKullanicisKullaniciId" }, "IX_SisKullaniciGorevs_SisKullanicisKullaniciId");
                        });
            });

            modelBuilder.Entity<SisBildirim>(entity =>
            {
                entity.HasKey(e => e.BildirimId);

                entity.ToTable("SisBildirim");

                entity.HasIndex(e => e.BildirimIcerikId, "IX_SisBildirim_BildirimIcerikId");

                entity.HasIndex(e => e.GonderilecekKullaniciId, "IX_SisBildirim_GonderilecekKullaniciId");

                entity.Property(e => e.GorulduDurumu)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.HasOne(d => d.BildirimIcerik)
                    .WithMany(p => p.SisBildirims)
                    .HasForeignKey(d => d.BildirimIcerikId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SisBildirim_SisBildirimIcerik");

                entity.HasOne(d => d.GonderilecekKullanici)
                    .WithMany(p => p.SisBildirims)
                    .HasForeignKey(d => d.GonderilecekKullaniciId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SisBildirim_SisKullanici");
            });

            modelBuilder.Entity<SisBildirimIcerik>(entity =>
            {
                entity.HasKey(e => e.BildirimIcerikId);

                entity.ToTable("SisBildirimIcerik");

                entity.Property(e => e.Aciklama)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.Json).IsUnicode(false);

                entity.Property(e => e.OlusturulanTarih).HasColumnType("datetime");
            });

            modelBuilder.Entity<SisBildirimOutbox>(entity =>
            {
                entity.HasKey(e => e.BildirimOutboxId);

                entity.ToTable("SisBildirimOutbox");

                entity.HasIndex(e => e.BildirimId, "IX_SisBildirimOutbox_BildirimId");

                entity.HasOne(d => d.Bildirim)
                    .WithMany(p => p.SisBildirimOutboxes)
                    .HasForeignKey(d => d.BildirimId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SisBildirimOutbox_SisBildirim");
            });

            modelBuilder.Entity<SisKullanici>(entity =>
            {
                entity.HasKey(e => e.KullaniciId);

                entity.ToTable("SisKullanici");

                entity.HasIndex(e => e.KullaniciAdi, "UK_SisKullanici")
                    .IsUnique();

                entity.Property(e => e.KullaniciAdi)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rol)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
