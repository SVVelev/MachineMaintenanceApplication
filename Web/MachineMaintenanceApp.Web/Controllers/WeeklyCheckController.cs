namespace MachineMaintenanceApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.WeeklyChecks;
    using MachineMaintenanceApp.Web.ViewModels.WeeklyChecks;
    using MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.Create;
    using MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.Edit;
    using MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.WeeklyChecksPage;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator, Engineer, Technician, Manager")]
    public class WeeklyCheckController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly IWeeklyChecksService weeklyChecksService;
        private readonly UserManager<ApplicationUser> userManager;

        public WeeklyCheckController(IWeeklyChecksService weeklyChecksService, UserManager<ApplicationUser> userManager)
        {
            this.weeklyChecksService = weeklyChecksService;
            this.userManager = userManager;
        }

        public IActionResult Create(string id)
        {
            var viewModel = new WeeklyCreateInputViewModel()
            {
                MachineId = id,
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.weeklyChecksService.GetById<WeeklyEditInputViewModel>(id);

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.weeklyChecksService.CheckAccess(currentUser, id))
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            return this.View(viewModel);
        }

        public IActionResult WeeklyChecksPage(string id, int page = 1)
        {
            var count = this.weeklyChecksService.GetCount(id);

            var viewModel = new PageWeeklyChecksViewModel
            {
                WeeklyChecks =
                    this.weeklyChecksService.GetAll<WeeklyChecksPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
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

                string machineId = await this.weeklyChecksService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.WeeklyChecksPage), new { id = machineId });
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

        public IActionResult Details(string id)
        {
            var viewModel = this.weeklyChecksService.GetById<DetailsWeeklyChecksViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(WeeklyCreateInputViewModel input, string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            input.MachineId = id;

            var currentUserId = this.userManager.GetUserId(this.User);

            try
            {
                var weeklyChecksId = await this.weeklyChecksService.CreateAsync(input.Type, input.MachineId, input.Notes, currentUserId);
                return this.RedirectToAction(nameof(this.WeeklyChecksPage), new { id });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WeeklyEditInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var id = await this.weeklyChecksService.EditAsync(input.Type, input.Notes, input.Id, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id });
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
