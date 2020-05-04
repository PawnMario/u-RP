using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

using uRP.Database.Model;

namespace uRP.Database
{
    class Context : DbContext, IDisposable
    {
        private string database;

        public Context() : this("server=localhost;database=uRP;user=root;password=")
        {

        }

        public Context(string v)
        {
            this.database = v;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;database=uRP;user=root;password=", 
                ob => ob.MigrationsAssembly(typeof(Context).GetTypeInfo().Assembly.GetName().Name));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Account> Players { get; set; }
        public DbSet<Character> Characters { get; set; }
    }
}
