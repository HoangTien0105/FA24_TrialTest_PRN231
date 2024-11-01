using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repositories.Models;

public partial class ViroCureFal2024dbContext : DbContext
{
    public ViroCureFal2024dbContext()
    {
    }

    public ViroCureFal2024dbContext(DbContextOptions<ViroCureFal2024dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<PersonVirus> PersonViruses { get; set; }

    public virtual DbSet<ViroCureUser> ViroCureUsers { get; set; }

    public virtual DbSet<Virus> Viruses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(GetConnectionString("DB"));
    private string GetConnectionString(string name)
    {
        IConfigurationRoot config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", true, true)
            .Build();

        return config.GetConnectionString(name) ?? "";
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__543848DF8A893E01");

            entity.ToTable("Person");

            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasColumnName("person_id");
            entity.Property(e => e.BirthDay).HasColumnName("birth_day");
            entity.Property(e => e.Fullname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("fullname");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.People)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Person__user_id__286302EC");
        });

        modelBuilder.Entity<PersonVirus>(entity =>
        {
            entity.HasKey(e => new { e.PersonId, e.VirusId }).HasName("PK__person_v__7BFA2E3F417B9C45");

            entity.ToTable("person_virus");

            entity.Property(e => e.PersonId).HasColumnName("person_id");
            entity.Property(e => e.VirusId).HasColumnName("virus_id");
            entity.Property(e => e.ResistanceRate).HasColumnName("resistance_rate");

            entity.HasOne(d => d.Person).WithMany(p => p.PersonViruses)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__person_vi__perso__2D27B809");

            entity.HasOne(d => d.Virus).WithMany(p => p.PersonViruses)
                .HasForeignKey(d => d.VirusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__person_vi__virus__2E1BDC42");
        });

        modelBuilder.Entity<ViroCureUser>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__ViroCure__B9BE370FCDDE70F7");

            entity.ToTable("ViroCureUser");

            entity.HasIndex(e => e.Email, "UQ__ViroCure__AB6E6164141598A1").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasDefaultValue(2)
                .HasColumnName("role");
        });

        modelBuilder.Entity<Virus>(entity =>
        {
            entity.HasKey(e => e.VirusId).HasName("PK__Virus__FC266E03951BD959");

            entity.ToTable("Virus");

            entity.Property(e => e.VirusId)
                .ValueGeneratedNever()
                .HasColumnName("virus_id");
            entity.Property(e => e.Treatment)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("treatment");
            entity.Property(e => e.VirusName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("virus_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}