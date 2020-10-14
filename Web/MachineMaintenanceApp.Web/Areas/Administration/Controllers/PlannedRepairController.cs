namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.PlannedRepairs;
    using MachineMaintenanceApp.Web.ViewModels.Administration.PlannedRepairs.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.PlannedRepairs.Edit;
    using MachineMaintenanceApp.Web.ViewModels.Administration.PlannedRepairs.PlannedRepairsPage;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class PlannedRepairController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly IPlannedRepairsService plannedRepairsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PlannedRepairController(IPlannedRepairsService plannedRepairsService, UserManager<ApplicationUser> userManager)
        {
            this.plannedRepairsService = plannedRepairsService;
            this.userManager = userManager;
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.plannedRepairsService.GetById<AdminPlannedRepairEditViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult PlannedRepairsPage(string id, int page = 1)
        {
            var count = this.plannedRepairsService.GetCountWithDeleted(id);

            var viewModel = new PageAdminPlannedRepairsViewModel
            {
                PlannedRepairs =
                    this.plannedRepairsService.GetAllWithDeleted<AdminPlannedRepairsPageViewModel>(id),
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
                var plannedRepairMachineId = await this.plannedRepairsService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.PlannedRepairsPage), new { id = plannedRepairMachineId });
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
                var plannedRepairId = await this.plannedRepairsService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id = plannedRepairId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.plannedRepairsService.GetByIdWithDeleted<AdminPlannedRepairsDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminPlannedRepairEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var plannedId = await this.plannedRepairsService.EditAsync(input.Type, input.StartTime, input.EndTime, input.Description, input.RepairsIntervalDays, input.PartNumber, input.Id, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id = plannedId });
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
