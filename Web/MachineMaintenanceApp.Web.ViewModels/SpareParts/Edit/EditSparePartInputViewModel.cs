namespace MachineMaintenanceApp.Web.ViewModels.SpareParts.Edit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class EditSparePartInputViewModel : IMapFrom<SparePart>
    {
        public string Id { get; set; }

        [Required]
        public SparePartType Type { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Serial number")]
        public string SerialNumber { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        [Display(Name = "Inventory number")]
        public string InventoryNumber { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Manufacturer { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        [Range(0, double.MaxValue)]
        public int Quantity { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }
    }
}
