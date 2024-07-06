using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bibliotecaModels.Entity;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaDataAccess
{
    public class AplicationDBContext : DbContext
    {
         public AplicationDBContext(DbContextOptions options) : base(options)        
        {
        }

        public DbSet<books> book { get; set; }

        public DbSet<users> usuario { get; set; }

        public DbSet<loan> loans { get; set; }
    }
}