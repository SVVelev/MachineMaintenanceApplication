namespace MachineMaintenanceApp.Web.ViewModels.Administration.Machines.Details
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminMachinesDetailViewModel : IMapFrom<Machine>
    {
        public string Id { get; set; }

        [Display(Name = "Inventory number")]
        public string InventoryNumber { get; set; }

        [Display(Name = "Serial number")]
        public string SerialNumber { get; set; }

        public string Model { get; set; }

        public string Manufacturer { get; set; }

        [Display(Name = "Year of manufacture")]
        public string ManufactureYear { get; set; }

        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public string UserCompanyId { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Created by")]
        public string UserLastName { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
    }
}
