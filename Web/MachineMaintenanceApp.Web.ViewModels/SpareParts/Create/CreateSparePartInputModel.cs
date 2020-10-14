namespace MachineMaintenanceApp.Web.ViewModels.SpareParts.Create
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models.Enums;

    public class CreateSparePartInputModel
    {
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
