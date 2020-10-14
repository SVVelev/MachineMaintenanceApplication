namespace MachineMaintenanceApp.Web.ViewModels.Administration.Home
{
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdministrationIndexCompanyViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public bool IsDeleted { get; set; }
    }
}
