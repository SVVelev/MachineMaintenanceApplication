namespace MachineMaintenanceApp.Web.ViewModels.Home
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class IndexMachineViewModel : IMapFrom<Machine>
    {
        public string Id { get; set; }

        public string InventoryNumber { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        public string ImageUrl { get; set; }

        public virtual ICollection<PlannedRepair> MachinePlannedRepairs { get; set; }


    }
}
