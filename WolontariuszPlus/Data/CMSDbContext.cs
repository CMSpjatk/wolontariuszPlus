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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<Organizer> Organizers { get; set; }
    }
}
