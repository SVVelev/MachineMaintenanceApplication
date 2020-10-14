namespace MachineMaintenanceApp.Web.Controllers
{
    using System;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using MachineMaintenanceApp.Data.Common.Helpers;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Services.Mapping;
    using MachineMaintenanceApp.Web.ViewModels.Machines.Create;
    using MachineMaintenanceApp.Web.ViewModels.Machines.Details;
    using MachineMaintenanceApp.Web.ViewModels.Machines.Edit;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class MachineController : BaseController
    {
        private readonly IMachinesService machinesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly Cloudinary cloudinary;

        public MachineController(IMachinesService machinesService, UserManager<ApplicationUser> userManager, Cloudinary cloudinary)
        {
            this.machinesService = machinesService;
            this.userManager = userManager;
            this.cloudinary = cloudinary;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateMachineInputModel();

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = this.machinesService.GetById<EditMachineInputViewModel>(id);
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.machinesService.CheckAccess(currentUser, id))
            {
                return this.Redirect("/Identity/Account/AccessDenied");
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Manager, Operator, Technician")]
        public IActionResult Details(string id)
        {
            var viewModel = this.machinesService.GetById<MachineDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var currentUser = await this.userManager.GetUserAsync(this.User);

                await this.machinesService.DeleteAsync(id, currentUser);

                return this.RedirectToAction("Index", "Home");
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

        [HttpPost]
        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Create(CreateMachineInputModel input)
        {
            var machine = AutoMapperConfig.MapperInstance.Map<Machine>(input);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var currentUserId = this.userManager.GetUserId(this.User);

            var machineId = await this.machinesService.CreateAsync(input.InventoryNumber, input.SerialNumber, input.Model, input.Manufacturer, input.ManufactureYear, input.Description, input.ImageUrl, currentUserId);
            return this.RedirectToAction(nameof(this.Details), new { id = machineId });
        }

        [HttpPost]
        [Authorize(Roles = "Administrator, Engineer, Manager")]
        public async Task<IActionResult> Edit(EditMachineInputViewModel input)
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

        //[HttpPost]
        //public async Task<IActionResult> Upload(IFormFile file, string model, string manufacturer, string manufactureYear, string serialNumber, string inventoryNumber, string description)
        //{
        //    var result = await CloudinaryHelper.UploadAsync(this.cloudinary, file);

        //    this.ViewData["imageUrl"] = result;
        //    this.ViewData["model"] = model;
        //    this.ViewData["manufacturer"] = manufacturer;
        //    this.ViewData["manufactureYear"] = manufactureYear;
        //    this.ViewData["serialNumber"] = serialNumber;
        //    this.ViewData["inventoryNumber"] = inventoryNumber;
        //    this.ViewData["description"] = description;

        //    return this.RedirectToAction(nameof(this.Create));
        //}
    }
}
