namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.WeeklyChecks;
    using MachineMaintenanceApp.Web.ViewModels.Administration.WeeklyChecks.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.WeeklyChecks.Edit;
    using MachineMaintenanceApp.Web.ViewModels.Administration.WeeklyChecks.WeeklyCheckPage;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class WeeklyCheckController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly IWeeklyChecksService weeklyChecksService;
        private readonly UserManager<ApplicationUser> userManager;

        public WeeklyCheckController(IWeeklyChecksService weeklyChecksService, UserManager<ApplicationUser> userManager)
        {
            this.weeklyChecksService = weeklyChecksService;
            this.userManager = userManager;
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.weeklyChecksService.GetById<AdminWeeklyCheckEditViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult WeeklyChecksPage(string id, int page = 1)
        {
            var count = this.weeklyChecksService.GetCountWithDeleted(id);

            var viewModel = new AdminPageWeeklyCheckViewModel
            {
                WeeklyChecks =
                    this.weeklyChecksService.GetAllWithDeleted<AdminWeeklyCheckPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
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
                var weeklyCheckMachineId = await this.weeklyChecksService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.WeeklyChecksPage), new { id = weeklyCheckMachineId });
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
                var weeklyCheckId = await this.weeklyChecksService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id = weeklyCheckId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.weeklyChecksService.GetByIdWithDeleted<AdminWeeklyDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminWeeklyCheckEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var weeklyyCheckId = await this.weeklyChecksService.EditAsync(input.Type, input.Notes, input.Id, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id = weeklyyCheckId });
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
