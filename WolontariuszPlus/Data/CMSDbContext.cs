using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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

            var splitStringConverter = new ValueConverter<ICollection<string>, string>(v => string.Join(";", v), v => v.Split(new[] { ';' }));
            modelBuilder.Entity<Event>().Property(nameof(Event.Tags)).HasConversion(splitStringConverter);
        }

        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Volunteer> Volunteers { get; set; }
        public virtual DbSet<Organizer> Organizers { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<VolunteerOnEvent> VolunteersOnEvent { get; set; }
    }
}
