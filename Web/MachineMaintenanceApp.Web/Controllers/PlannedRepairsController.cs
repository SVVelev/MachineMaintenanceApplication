namespace MachineMaintenanceApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.PlannedRepairs;
    using MachineMaintenanceApp.Web.ViewModels.PlannedRepairs.Create;
    using MachineMaintenanceApp.Web.ViewModels.PlannedRepairs.Details;
    using MachineMaintenanceApp.Web.ViewModels.PlannedRepairs.Edit;
    using MachineMaintenanceApp.Web.ViewModels.PlannedRepairs.PlannedRepairsPage;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator, Engineer, Manager")]
    public class PlannedRepairsController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IPlannedRepairsService plannedRepairsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PlannedRepairsController(
            IPlannedRepairsService plannedRepairsService,
            UserManager<ApplicationUser> userManager)
        {
            this.plannedRepairsService = plannedRepairsService;
            this.userManager = userManager;
        }

        public IActionResult Create(string id)
        {
            var viewModel = new PlannedRepairsCreateInputViewModel()
            {
                MachineId = id,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.plannedRepairsService.GetById<PlannedRepairsEditInputViewModel>(id);

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.plannedRepairsService.CheckAccess(currentUser, id))
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            if (viewModel.PartNumber == null)
            {
                viewModel.PartNumber = string.Empty;
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);

                string machineId = await this.plannedRepairsService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.PlannedRepairsPage), new { id = machineId });
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

        public IActionResult PlannedRepairsPage(string id, int page = 1)
        {
            var count = this.plannedRepairsService.GetCount(id);

            var viewModel = new PagePlannedRepairsViewModel
            {
                PlannedRepairs =
                    this.plannedRepairsService.GetAll<PlannedRepairsPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
                MachineId = id,
                PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage),
                CurrentPage = page,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            return this.View(viewModel);
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.plannedRepairsService.GetById<PlannedRepairsDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }


            if (viewModel.PartNumber == null)
            {
                viewModel.PartNumber = "Without Spare Part";
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlannedRepairsCreateInputViewModel input, string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            input.MachineId = id;

            var currentUserId = this.userManager.GetUserId(this.User);

            try
            {
                var plannedId = await this.plannedRepairsService.CreateAsync(input.Type, input.StartTime, input.EndTime, input.Description, input.RepairsIntervalDays, input.MachineId, currentUserId, input.PartNumber);
                return this.RedirectToAction(nameof(this.Details), new { id = plannedId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PlannedRepairsEditInputViewModel input)
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