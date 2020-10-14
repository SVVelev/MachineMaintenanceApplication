namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Machines.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Machines.Edit;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Machines.MachinesPage;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class MachineController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly IMachinesService machinesService;
        private readonly UserManager<ApplicationUser> userManager;

        public MachineController(IMachinesService machinesService, UserManager<ApplicationUser> userManager)
        {
            this.machinesService = machinesService;
            this.userManager = userManager;
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.machinesService.GetById<AdminMachineEditViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult MachinesPage(string id, int page = 1)
        {
            var count = this.machinesService.GetCountWithDeleted(id);

            var viewModel = new MachinePageViewModel
            {
                Machines =
                    this.machinesService.GetAllForCompany<AdminMachinePageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
                CompanyId = id,
                PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage),
                CurrentPage = page,
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
                var machineCompanyId = await this.machinesService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.MachinesPage), new { id = machineCompanyId });
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
                await this.machinesService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.machinesService.GetByIdWithDeleted<AdminMachinesDetailViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminMachineEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var machineId = await this.machinesService.EditAsync(input.InventoryNumber, input.SerialNumber, input.Model, input.Manufacturer, input.ManufactureYear, input.Description, input.ImageUrl, input.Id, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id = machineId });
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
