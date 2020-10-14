namespace MachineMaintenanceApp.Web.ViewModels.Administration.SpareParts.SparePartsPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminSparePartPageViewModel : IMapFrom<SparePart>
    {
        public string Id { get; set; }

        public string InventoryNumber { get; set; }

        public SparePartType Type { get; set; }

        public string MachineInventoryNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}
