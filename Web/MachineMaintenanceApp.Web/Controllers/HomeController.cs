namespace MachineMaintenanceApp.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Web.ViewModels;
    using MachineMaintenanceApp.Web.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseController
    {
        private readonly IMachinesService machinesService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(IMachinesService machinesService, UserManager<ApplicationUser> userManager)
        {
            this.machinesService = machinesService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var viewModel = new IndexViewModel
            {
                Machines =
                    this.machinesService.GetAll<IndexMachineViewModel>(currentUser),
            };

            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }

        public IActionResult NotFound()
        {
            return this.View();
        }

        public IActionResult HttpError(int statusCode)
        {
            if (statusCode == 404)
            {
               return this.RedirectToAction(nameof(this.NotFound));
            }
            else
            {
               return this.RedirectToAction(nameof(this.Error));
            }
        }
    }
}
