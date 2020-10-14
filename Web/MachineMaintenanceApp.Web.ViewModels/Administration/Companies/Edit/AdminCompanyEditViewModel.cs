namespace MachineMaintenanceApp.Web.ViewModels.Administration.Companies.Edit
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminCompanyEditViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }
    }
}
