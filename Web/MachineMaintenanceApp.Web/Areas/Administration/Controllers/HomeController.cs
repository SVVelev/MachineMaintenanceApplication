namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using MachineMaintenanceApp.Services.Data.Comapnies;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Home;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : AdministrationController
    {
        private readonly ICompanyService companyService;

        public HomeController(ICompanyService companyService)
        {
            this.companyService = companyService;
        }

        public IActionResult Index()
        {
            var viewModel = new AdministrationIndexViewModel
            {
                Companies =
                     this.companyService.GetAllWithDeleted<AdministrationIndexCompanyViewModel>(),
            };

            return this.View(viewModel);
        }
    }
}
