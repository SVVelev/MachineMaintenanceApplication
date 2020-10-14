namespace MachineMaintenanceApp.Web.ViewModels.Machines.Create
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class CreateMachineInputModel : IMapTo<Machine>
    {
        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string InventoryNumber { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string SerialNumber { get; set; }

        public string Model { get; set; }

        [StringLength(30, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Manufacturer { get; set; }

        public string ManufactureYear { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }
    }
}
