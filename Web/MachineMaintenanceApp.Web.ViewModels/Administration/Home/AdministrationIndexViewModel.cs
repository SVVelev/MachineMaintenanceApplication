namespace MachineMaintenanceApp.Web.ViewModels.Administration.Home
{
    using System.Collections.Generic;

    public class AdministrationIndexViewModel
    {
        public IEnumerable<AdministrationIndexCompanyViewModel> Companies { get; set; }
    }
}
