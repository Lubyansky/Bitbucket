using System;
using System.Collections.Generic;
using Bitbucket.Models.Account;
using Bitbucket.Models.ShortUrl;
using Microsoft.EntityFrameworkCore;

namespace Bitbucket.Data;

public partial class BitbucketContext : DbContext
{
    public BitbucketContext()
    {
    }

    public BitbucketContext(DbContextOptions<BitbucketContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Url> Urls { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        optionsBuilder.UseNpgsql($"Host=localhost;Port=5432;Database=bitbucket;Username=postgres;Password={config["db_password"]}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasKey(e => e.UrlId).HasName("Urls_pkey");

            entity.Property(e => e.UrlId)
                .HasDefaultValueSql("nextval('\"Urls_id_seq\"'::regclass)")
                .HasColumnName("url_id");
            entity.Property(e => e.Clicks).HasColumnName("clicks");
            entity.Property(e => e.Original)
                .HasColumnType("character varying")
                .HasColumnName("original");
            entity.Property(e => e.Token)
                .HasColumnType("character varying")
                .HasColumnName("token");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Urls)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_user_id");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Users_pkey");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("nextval('\"Users_id_seq\"'::regclass)")
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasColumnType("character varying")
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasColumnType("character varying")
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
