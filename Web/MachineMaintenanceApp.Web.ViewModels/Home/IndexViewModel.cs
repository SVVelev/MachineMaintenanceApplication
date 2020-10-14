namespace MachineMaintenanceApp.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class IndexViewModel
    {
        public IEnumerable<IndexMachineViewModel> Machines { get; set; }
    }
}
