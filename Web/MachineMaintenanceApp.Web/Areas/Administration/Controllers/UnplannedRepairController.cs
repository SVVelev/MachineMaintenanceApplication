namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.UnplannedRepairs;
    using MachineMaintenanceApp.Web.ViewModels.Administration.UnplannedRepairs.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.UnplannedRepairs.Edit;
    using MachineMaintenanceApp.Web.ViewModels.Administration.UnplannedRepairs.UnplannedRepairsPage;
    using MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.PlannedRepairsPage;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UnplannedRepairController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly IUnplannedRepairsService unplannedRepairsService;
        private readonly UserManager<ApplicationUser> userManager;

        public UnplannedRepairController(IUnplannedRepairsService unplannedRepairsService, UserManager<ApplicationUser> userManager)
        {
            this.unplannedRepairsService = unplannedRepairsService;
            this.userManager = userManager;
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.unplannedRepairsService.GetById<AdminUnplannedRepairsEditViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult UnplannedRepairsPage(string id, int page = 1)
        {
            var count = this.unplannedRepairsService.GetCountWithDeleted(id);

            var viewModel = new PageAdminUnplannedRepairsViewModel
            {
                UnplannedRepairs =
                    this.unplannedRepairsService.GetAllWithDeleted<AdminUnplannedRepairsPageViewModel>(id),
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
                var unplannedMachineId = await this.unplannedRepairsService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.UnplannedRepairsPage), new { id = unplannedMachineId });
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
                var unplannedId = await this.unplannedRepairsService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id = unplannedId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.unplannedRepairsService.GetByIdWithDeleted<AdminUnplannedRepairsDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUnplannedRepairsEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var unplannedId = await this.unplannedRepairsService.EditAsync(input.Type, input.StartTime, input.EndTime, input.Description, input.PartNumber, input.Id, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id = unplannedId });
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
