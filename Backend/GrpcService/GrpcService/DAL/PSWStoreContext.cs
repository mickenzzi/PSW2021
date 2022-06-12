using Microsoft.EntityFrameworkCore;
using Pharmacy.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.DAL
{
    public class PSWStoreContext : DbContext
    {
        public PSWStoreContext() { }
        public PSWStoreContext(DbContextOptions<PSWStoreContext> options) : base(options) { }

        public DbSet<Medicine> Medicine { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "Server=localhost;Port=5432;Database=pharmacy;User Id=postgres;Password=milorad;";
            optionsBuilder.UseNpgsql(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
