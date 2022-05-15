using Microsoft.EntityFrameworkCore;
using PSW.Model;
using System;

namespace PSW.DAL
{
    public class PSWStoreContext : DbContext
    {
        public PSWStoreContext() { } 
        public PSWStoreContext(DbContextOptions<PSWStoreContext> options) : base(options) { }

        public DbSet<User> User { get; set; }

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
