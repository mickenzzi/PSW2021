﻿using Microsoft.EntityFrameworkCore;
using PSW.Model;

namespace PSW.DAL
{
    public class PSWStoreContext : DbContext
    {
        public PSWStoreContext() { }
        public PSWStoreContext(DbContextOptions<PSWStoreContext> options) : base(options) { }

        public DbSet<User> User { get; set; }

        public DbSet<Comment> Comment { get; set; }

        public DbSet<Term> Term { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<Medicine> Medicine { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Server=localhost;Port=5432;Database=psw;User Id=postgres;Password=milorad;";
            optionsBuilder.UseNpgsql(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
