
namespace MachineMaintenanceApp.Web.ViewModels.Companies
{
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class CompanyIdViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }
    }
}
