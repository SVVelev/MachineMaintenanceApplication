namespace MachineMaintenanceApp.Web.ViewModels.SpareParts.SparePartsHomePageMachine
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HomeSpareMachineViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<SparePartsHomePageMachineViewModel> SpareParts { get; set; }
    }
}
