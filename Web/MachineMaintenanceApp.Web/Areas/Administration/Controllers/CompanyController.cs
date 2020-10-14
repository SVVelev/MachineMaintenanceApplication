namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Services.Data.Comapnies;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Companies.Create;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Companies.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Companies.Edit;
    using Microsoft.AspNetCore.Mvc;

    public class CompanyController : AdministrationController
    {
        private readonly ICompanyService companyService;

        public CompanyController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public IActionResult Create()
        {
            var viewModel = new AdminCreateCompanyInputViewModel();

            return this.View(viewModel);
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.companyService.GetById<AdminCompanyEditViewModel>(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await this.companyService.DeleteAsync(id);
                return this.RedirectToAction("Index", "Home");
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
                var companyId = await this.companyService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id = companyId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.companyService.GetByIdWithDeleted<AdminDetailsCompanyViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminCreateCompanyInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var companyId = await this.companyService.CreateAsync(input.Name, input.Logo, input.Description);
            return this.RedirectToAction(nameof(this.Details), new { id = companyId });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminCompanyEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                var companyId = await this.companyService.EditAsync(input.Name, input.Logo, input.Description, input.Id);
                return this.RedirectToAction(nameof(this.Details), new { id = companyId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }
    }
}
