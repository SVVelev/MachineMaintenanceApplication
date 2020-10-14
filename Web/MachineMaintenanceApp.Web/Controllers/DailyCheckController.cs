namespace MachineMaintenanceApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using MachineMaintenanceApp.Data.Common.Helpers;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.DailyChecks;
    using MachineMaintenanceApp.Web.ViewModels.DailyChecks;
    using MachineMaintenanceApp.Web.ViewModels.DailyChecks.Create;
    using MachineMaintenanceApp.Web.ViewModels.DailyChecks.DailyChecksPage;
    using MachineMaintenanceApp.Web.ViewModels.DailyChecks.Edit;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator, Engineer, Operator, Technician, Manager")]
    public class DailyCheckController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IDailyChecksService dailyChecksService;
        private readonly UserManager<ApplicationUser> userManager;

        public DailyCheckController(IDailyChecksService dailyChecksService, UserManager<ApplicationUser> userManager)
        {
            this.dailyChecksService = dailyChecksService;
            this.userManager = userManager;
        }

        public IActionResult Create(string id)
        {
            var viewModel = new DailyCreateInputViewModel()
            {
                MachineId = id,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
           var viewModel = this.dailyChecksService.GetById<DailyEditInputViewModel>(id);
           var currentUser = await this.userManager.GetUserAsync(this.User);

           if (!this.dailyChecksService.CheckAccess(currentUser, id))
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

           return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);

                string machineId = await this.dailyChecksService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.DailyChecksPage), new { id = machineId });
            }
            catch (UnauthorizedAccessException)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult DailyChecksPage(string id, int page = 1)
        {
            var count = this.dailyChecksService.GetCount(id);

            var viewModel = new PageDailyChecksViewModel
            {
                DailyChecks =
                    this.dailyChecksService.GetAll<DailyChecksPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
                PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage),
                CurrentPage = page,
                MachineId = id,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            return this.View(viewModel);
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.dailyChecksService.GetById<DetailsDailyChecksViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(DailyCreateInputViewModel input, string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            input.MachineId = id;

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var dailyCheckId = await this.dailyChecksService.CreateAsync(input.Type, input.MachineId, input.Notes, currentUser.Id);
                return this.RedirectToAction(nameof(this.DailyChecksPage), new { id });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(DailyEditInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var dailyCheckId = await this.dailyChecksService.EditAsync(input.Type, input.Notes, input.Id, input.MachineId, currentUser);
                return this.RedirectToAction(nameof(this.DailyChecksPage), new { id = dailyCheckId });
            }
            catch (UnauthorizedAccessException)
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }
    }
}
