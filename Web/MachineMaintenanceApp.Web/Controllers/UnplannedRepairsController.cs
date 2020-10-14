namespace MachineMaintenanceApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.UnplannedRepairs;
    using MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.Create;
    using MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.Details;
    using MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.Edit;
    using MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.PlannedRepairsPage;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator, Engineer, Manager")]
    public class UnplannedRepairsController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IUnplannedRepairsService unplannedRepairsService;
        private readonly UserManager<ApplicationUser> userManager;

        public UnplannedRepairsController(
            IUnplannedRepairsService unplannedRepairsService,
            UserManager<ApplicationUser> userManager)
        {
            this.unplannedRepairsService = unplannedRepairsService;
            this.userManager = userManager;
        }

        public IActionResult Create(string id)
        {
            var viewModel = new UnplannedRepairsCreateInputViewModel()
            {
                MachineId = id,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.unplannedRepairsService.GetById<UnplannedRepairsEditInputModel>(id);

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.unplannedRepairsService.CheckAccess(currentUser, id))
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

                string machineId = await this.unplannedRepairsService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.UnplannedRepairsPage), new { id = machineId });
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

        public IActionResult UnplannedRepairsPage(string id, int page = 1)
        {
            var count = this.unplannedRepairsService.GetCount(id);

            var viewModel = new PageUnplannedRepairsViewModel
            {
                UnplannedRepairs =
                    this.unplannedRepairsService.GetAll<UnplannedRepairsPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
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
            var viewModel = this.unplannedRepairsService.GetById<UnplannedRepairsDetailsViewModel>(id);

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
        public async Task<IActionResult> Create(UnplannedRepairsCreateInputViewModel input, string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            input.MachineId = id;

            var currentUserId = this.userManager.GetUserId(this.User);

            try
            {
                var unplannedId = await this.unplannedRepairsService.CreateAsync(input.Type, input.StartTime, input.EndTime, input.Description, input.MachineId, currentUserId, input.PartNumber);
                return this.RedirectToAction(nameof(this.Details), new { id = unplannedId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UnplannedRepairsEditInputModel input)
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
