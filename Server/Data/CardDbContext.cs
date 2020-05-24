﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Server.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Server.Models;

namespace Server.Data
{
    public partial class CardDbContext : DbContext
    {
        public CardDbContext()
        {
        }

        public CardDbContext(DbContextOptions<CardDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Card> Card { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CardGenDB;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Abilities).IsRequired();

                entity.Property(e => e.Quality).IsRequired();

                entity.Property(e => e.Rarity).IsRequired();

                entity.Property(e => e.Type).IsRequired();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}