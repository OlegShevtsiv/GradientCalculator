using GradientCalculator.Data.Sqlite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GradientCalculator.Data.Sqlite
{
    public class SqliteContext : DbContext
    {
        public static readonly string SliteFileName = "GradientCalculatorStorage.db";

        public DbSet<ExceptionLog> ExceptionLogs { get; set; }

        public SqliteContext() : base()
        {
            Database.EnsureCreated();
        }

        public SqliteContext(DbContextOptions<SqliteContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={SqliteContext.SliteFileName}");
        }
    }
}
