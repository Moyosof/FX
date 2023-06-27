using FX.Domain.Entities.Auth;
using FX.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.Data.Context
{
    public class FXDbContext : DbContext
    {
        public FXDbContext(DbContextOptions<FXDbContext> options) : base(options)
        {

        }


        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
