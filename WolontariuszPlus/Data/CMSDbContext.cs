using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Data
{
    public partial class CMSDbContext : IdentityDbContext<IdentityUser>
    {
        public CMSDbContext(DbContextOptions<CMSDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Test> Tests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
