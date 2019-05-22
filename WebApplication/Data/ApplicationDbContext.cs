using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Application.Entities;
using WebApplication.Models;

namespace Application
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<ActiveToken> ActiveTokens { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Application.Entities.Trip> Trip { get; set; }

        public DbSet<Application.Entities.Apartment> Apartment { get; set; }

        public DbSet<Application.Entities.ApartmentRoom> ApartmentRoom { get; set; }

    }
}
