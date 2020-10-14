namespace MachineMaintenanceApp.Web.ViewModels.Administration.SpareParts.Details
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminSparePartDetailsViewModel : IMapFrom<SparePart>
    {
        [Display(Name = "Spare parts type")]
        public SparePartType Type { get; set; }

        [Display(Name = "Serial number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Inventory number")]
        public string InventoryNumber { get; set; }

        public string Manufacturer { get; set; }

        public string Description { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }

        [Display(Name = "Created by")]
        public string UserLastName { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        public string Id { get; set; }

        public string UserCompanyId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
