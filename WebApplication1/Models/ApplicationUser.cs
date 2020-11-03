using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayUserName { get; set; }

        public async Task CreateUserAsync(string password, UserManager<ApplicationUser> userManager)
        {
            var exists = await userManager.FindByEmailAsync(Email);
            if (exists == null)
                await userManager.CreateAsync(this, password);
        }
    }
}
