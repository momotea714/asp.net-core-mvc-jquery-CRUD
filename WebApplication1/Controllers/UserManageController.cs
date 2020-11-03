using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using WebApplication1.Repository.Interface;
using WebApplication1.Service;

namespace WebApplication1.Controllers
{
    public class UserManageController : Controller
    {
        private readonly IApplicationUserRepositoryAsync _applicationUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRazorRenderService _renderService;
        private readonly ILogger<UserManageController> _logger;

        public UserManageController(ILogger<UserManageController> logger, IApplicationUserRepositoryAsync applicationUser, IUnitOfWork unitOfWork, IRazorRenderService renderService)
        {
            _logger = logger;
            _applicationUser = applicationUser;
            _unitOfWork = unitOfWork;
            _renderService = renderService;
        }
        public IEnumerable<ApplicationUser> applicationUsers { get; set; }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> OnGetViewAllPartial()
        {
            applicationUsers = await _applicationUser.GetAllAsync();
            return new PartialViewResult
            {
                ViewName = "_ViewAll",
                ViewData = new ViewDataDictionary<IEnumerable<ApplicationUser>>(ViewData, applicationUsers)
            };
        }

        public async Task<IActionResult> GetCreateOrEdit(string userName = null)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return PartialView("_CreateOrEdit", new ApplicationUser());
            }
            else
            {
                var u = await _applicationUser.GetByUserNameAsync(userName);
                return PartialView("_CreateOrEdit", u);
            }
        }
        public async Task<JsonResult> PostCreateOrEdit(string userName, ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                if (userName == null)
                {
                    await _applicationUser.AddAsync(applicationUser);
                    await _unitOfWork.Commit();
                }
                else
                {
                    await _applicationUser.UpdateAsync(applicationUser);
                    await _unitOfWork.Commit();
                }
                applicationUsers = await _applicationUser.GetAllAsync();
                return new JsonResult(new { isValid = true, html = PartialView("_ViewAll", applicationUsers) });
            }
            else
            {
                return new JsonResult(new { isValid = false, html = PartialView("_CreateOrEdit", applicationUser) });
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(string userName)
        {
            var applicationUser = await _applicationUser.GetByUserNameAsync(userName);
            await _applicationUser.DeleteAsync(applicationUser);
            await _unitOfWork.Commit();
            return await OnGetViewAllPartial();
        }
    }
}
