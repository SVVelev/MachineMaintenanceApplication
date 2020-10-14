namespace MachineMaintenanceApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Services.Data.SpareParts;
    using MachineMaintenanceApp.Web.ViewModels.Machines;
    using MachineMaintenanceApp.Web.ViewModels.SpareParts.Create;
    using MachineMaintenanceApp.Web.ViewModels.SpareParts.Details;
    using MachineMaintenanceApp.Web.ViewModels.SpareParts.Edit;
    using MachineMaintenanceApp.Web.ViewModels.SpareParts.SparePartsHomePage;
    using MachineMaintenanceApp.Web.ViewModels.SpareParts.SparePartsHomePageMachine;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Administrator, Engineer, Manager")]
    public class SparePartController : BaseController
    {
        private const int ItemsPerPage = 6;

        private readonly ISparePartsService sparePartsService;
        private readonly IMachinesService machineService;
        private readonly UserManager<ApplicationUser> userManager;

        public SparePartController(ISparePartsService sparePartsService, IMachinesService machineService, UserManager<ApplicationUser> userManager)
        {
            this.sparePartsService = sparePartsService;
            this.machineService = machineService;
            this.userManager = userManager;
        }

        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public IActionResult Create()
        {
            var viewModel = new CreateSparePartInputModel();

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.sparePartsService.GetById<EditSparePartInputViewModel>(id);

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.sparePartsService.CheckAccess(currentUser, id))
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            if (viewModel.MachineInventoryNumber == null)
            {
                viewModel.MachineInventoryNumber = string.Empty;
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);

                await this.sparePartsService.DeleteAsync(id, currentUser);

                return this.RedirectToAction(nameof(this.SparePartHome));
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

        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public IActionResult CreateForCurrentMachine(string id)
        {
            var currentMachine = this.machineService.GetById<MachineInventoryNumberViewModel>(id);

            var viewModel = new CreateSparePartInputModel()
            {
                MachineInventoryNumber = currentMachine.InventoryNumber,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Operator, Technician, Manager")]
        public async Task<IActionResult> SparePartHome(int page = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var count = this.sparePartsService.GetCount(currentUser);

            var viewModel = new HomeSpareViewModel
            {
                SpareParts =
                    this.sparePartsService.GetAll<SparePartsHomePageViewModel>(currentUser, ItemsPerPage, (page - 1) * ItemsPerPage),
                PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage),
                CurrentPage = page,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Operator, Technician, Manager")]
        public async Task<IActionResult> SparePartHomeMachine(string id, int page = 1)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var count = this.sparePartsService.GetCount(currentUser);

            var viewModel = new HomeSpareMachineViewModel
            {
                SpareParts =
                    this.sparePartsService.GetAllForCurrentMachine<SparePartsHomePageMachineViewModel>(id),
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

        [Authorize(Roles = "Administrator, Engineer, Operator, Technician, Manager")]
        public IActionResult Details(string id)
        {
            var viewModel = this.sparePartsService.GetById<DetailsSparePartsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            if (viewModel.MachineInventoryNumber == null)
            {
                viewModel.MachineInventoryNumber = "Without Machine";
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> IncreaseQuantity(int quantity, string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View("Details");
            }

            var sparePartId = await this.sparePartsService.IncreaseSparePartQuantity(id, quantity);

            return this.RedirectToAction(nameof(this.Details), new { id = sparePartId });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Create(CreateSparePartInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUserId = this.userManager.GetUserId(this.User);

            var sparePartId = await this.sparePartsService.CreateAsync(input.Type, input.SerialNumber, input.InventoryNumber, input.Manufacturer, input.Description, input.ImageUrl, input.Quantity, input.MachineInventoryNumber, currentUserId);
            return this.RedirectToAction(nameof(this.Details), new { id = sparePartId });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> CreateForCurrentMachine(CreateSparePartInputModel input, string id)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentMachine = this.machineService.GetById<MachineInventoryNumberViewModel>(id);

            var currentUserId = this.userManager.GetUserId(this.User);

            var sparePartId = await this.sparePartsService.CreateAsync(input.Type, input.SerialNumber, input.InventoryNumber, input.Manufacturer, input.Description, input.ImageUrl, input.Quantity, currentMachine.InventoryNumber, currentUserId);
            return this.RedirectToAction(nameof(this.Details), new { id = sparePartId });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Edit(EditSparePartInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUser = await this.userManager.GetUserAsync(this.User);

            try
            {
                var sparePartId = await this.sparePartsService.EditAsync(input.Type, input.SerialNumber, input.InventoryNumber, input.Manufacturer, input.Description, input.ImageUrl, input.Quantity, input.MachineInventoryNumber, input.Id, currentUser);
                return this.RedirectToAction(nameof(this.Details), new { id = sparePartId });
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
