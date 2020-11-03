using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repository.Interface
{
    public interface IApplicationUserRepositoryAsync : IGenericRepositoryAsync<ApplicationUser>
    {
        Task<ApplicationUser> GetByUserNameAsync(string userName);
    }
}
