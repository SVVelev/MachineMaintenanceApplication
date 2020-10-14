namespace MachineMaintenanceApp.Web.ViewModels.Administration.Companies.Create
{
    using System.ComponentModel.DataAnnotations;

    public class AdminCreateCompanyInputViewModel
    {
        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }
    }
}
