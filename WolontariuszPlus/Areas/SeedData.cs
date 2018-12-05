using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WolontariuszPlus.Models;

namespace WolontariuszPlus.Areas
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            await EnsureRoleAsync(roleManager);
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            var volunteerAlreadyExists = await roleManager.RoleExistsAsync(Roles.VolunteerRole);
            var organizerAlreadyExists = await roleManager.RoleExistsAsync(Roles.OrganizerRole);

            if (!volunteerAlreadyExists)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.VolunteerRole));
            }

            if (!organizerAlreadyExists)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.OrganizerRole));
            }
        }
    }
}
