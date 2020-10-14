namespace MachineMaintenanceApp.Web.ViewModels.Machines.Details
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class MachineDetailsViewModel : IMapFrom<Machine>
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

        [Display(Name = "Created by")]
        public string UserLastName { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Display(Name = "Unplanned reapirs count")]
        public int UnplannedRepairsCount { get; set; }

        [Display(Name = "Planned reapairs count")]
        public int PlannedRepairsCount { get; set; }

        [Display(Name = "Daily checks count")]
        public int DailyChecksCount { get; set; }

        [Display(Name = "Weekly checks count")]
        public int WeeklyChecksCount { get; set; }

        [Display(Name = "Spare parts count")]
        public int SparePartsCount { get; set; }

        public string UserId { get; set; }
    }
}
