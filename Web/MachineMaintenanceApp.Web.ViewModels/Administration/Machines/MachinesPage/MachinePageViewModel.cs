namespace MachineMaintenanceApp.Web.ViewModels.Administration.Machines.MachinesPage
{
    using System.Collections.Generic;

    public class MachinePageViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string CompanyId { get; set; }

        public IEnumerable<AdminMachinePageViewModel> Machines { get; set; }
    }
}
