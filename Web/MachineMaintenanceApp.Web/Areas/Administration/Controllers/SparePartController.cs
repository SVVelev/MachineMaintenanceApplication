namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.SpareParts;
    using MachineMaintenanceApp.Web.ViewModels.Administration.SpareParts.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.SpareParts.Edit;
    using MachineMaintenanceApp.Web.ViewModels.Administration.SpareParts.SparePartsPage;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class SparePartController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly ISparePartsService sparePartService;
        private readonly UserManager<ApplicationUser> userManager;

        public SparePartController(ISparePartsService sparePartService, UserManager<ApplicationUser> userManager)
        {
            this.sparePartService = sparePartService;
            this.userManager = userManager;
        }

        public IActionResult Edit(string id, [FromQuery]string companyId)
        {
            var viewModel = this.sparePartService.GetById<AdminEditSparePartViewModel>(id);
            viewModel.CompanyId = companyId;
            return this.View(viewModel);
        }

        public IActionResult SparePartsPage(string id, int page = 1)
        {
            var count = this.sparePartService.GetCountForCompanyWithDeleted(id);

            var viewModel = new SparePartPageViewModel
            {
                SpareParts =
                    this.sparePartService.GetAllForCompany<AdminSparePartPageViewModel>(id, ItemsPerPage, (page - 1) * ItemsPerPage),
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
                var partCompanyId = await this.sparePartService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.SparePartsPage), new { id = partCompanyId });
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
                var partId = await this.sparePartService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id = partId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id, string companyId)
        {
            var viewModel = this.sparePartService.GetByIdWithDeleted<AdminSparePartDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            this.ViewBag.CompanyId = companyId;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminEditSparePartViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var machineId = await this.sparePartService.EditAsync(input.Type, input.SerialNumber, input.InventoryNumber, input.Manufacturer, input.Description, input.ImageUrl, input.Quantity, input.MachineInventoryNumber, input.Id, currentUser);
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
