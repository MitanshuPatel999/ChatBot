using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Models;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Interpretation> Interpretations { get; set; }

    public virtual DbSet<Rule> Rules { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRulesFavorite> UserRulesFavorites { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=RuleDB;Integrated Security=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Interpretation>(entity =>
        {
            entity.HasKey(e => e.InterpretationId).HasName("PK__Interpre__77E108EB76F7B5C9");

            entity.Property(e => e.Author).HasMaxLength(100);

            entity.HasOne(d => d.Rule).WithMany(p => p.Interpretations)
                .HasForeignKey(d => d.RuleId)
                .HasConstraintName("FK__Interpret__RuleI__4BAC3F29");
        });

        modelBuilder.Entity<Rule>(entity =>
        {
            entity.HasKey(e => e.RuleId).HasName("PK__Rules__110458E22A48DC10");

            entity.HasIndex(e => e.Title, "IX_Rules_Title");

            entity.Property(e => e.Category).HasMaxLength(50);
            entity.Property(e => e.PublicationDate).HasColumnType("date");
            entity.Property(e => e.Source).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C46B89A35");

            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRulesFavorite>(entity =>
        {
            entity.HasKey(e => e.FavoriteId).HasName("PK__UserRule__CE74FAD50F0F7101");

            entity.HasOne(d => d.Rule).WithMany(p => p.UserRulesFavorites)
                .HasForeignKey(d => d.RuleId)
                .HasConstraintName("FK__UserRules__RuleI__5165187F");

            entity.HasOne(d => d.User).WithMany(p => p.UserRulesFavorites)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__UserRules__UserI__5070F446");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

// dotnet ef dbcontext scaffold "Server=localhost\SQLEXPRESS;Database=RuleDB;Integrated Security=true;" Microsoft.EntityFrameworkCore.SqlServer -c MyDbContext -o Models
