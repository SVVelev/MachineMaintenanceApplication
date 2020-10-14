namespace MachineMaintenanceApp.Web.ViewModels.Administration.Machines.MachinesPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminMachinePageViewModel : IMapFrom<Machine>
    {
        public string Id { get; set; }

        public string InventoryNumber { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public bool IsDeleted { get; set; }
    }
}
