namespace MachineMaintenanceApp.Web.ViewModels.Administration.Machines.Edit
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminMachineEditViewModel : IMapFrom<Machine>
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

    }
}
