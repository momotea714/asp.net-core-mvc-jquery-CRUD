using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Repository.Interface;

namespace WebApplication1.Repository
{
    public class ApplicationUserRepositoryAsync : GenericRepositoryAsync<ApplicationUser>, IApplicationUserRepositoryAsync
    {
        private readonly DbSet<ApplicationUser> _applicationUser;
        public ApplicationUserRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _applicationUser = dbContext.Set<ApplicationUser>();
        }

        public Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            return _applicationUser.FirstAsync(x => x.UserName == userName);
        }
    }
}
