namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.DailyChecks;
    using MachineMaintenanceApp.Web.ViewModels.Administration.DailyChecks.DailyCheckPage;
    using MachineMaintenanceApp.Web.ViewModels.Administration.DailyChecks.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.DailyChecks.Edit;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class DailyCheckController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly IDailyChecksService dailyChecksService;
        private readonly UserManager<ApplicationUser> userManager;

        public DailyCheckController(IDailyChecksService dailyChecksService, UserManager<ApplicationUser> userManager)
        {
            this.dailyChecksService = dailyChecksService;
            this.userManager = userManager;
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.dailyChecksService.GetById<AdminDailyCheckEditViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult DailyChecksPage(string id, int page = 1)
        {
            var count = this.dailyChecksService.GetCountWithDeleted(id);

            var viewModel = new AdminPageDailyChecksViewModel
            {
                DailyChecks =
                    this.dailyChecksService.GetAllWithDeleted<AdminDailyCheckPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
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

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);
                var machineId = await this.dailyChecksService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.Details), new { id = machineId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public async Task<IActionResult> Undelete(string id)
        {
            try
            {
                await this.dailyChecksService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.dailyChecksService.GetByIdWithDeleted<AdminDailyDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminDailyCheckEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var dailyCheckId = await this.dailyChecksService.EditAsync(input.Type, input.Notes, input.Id, input.MachineId, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id = dailyCheckId });
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
