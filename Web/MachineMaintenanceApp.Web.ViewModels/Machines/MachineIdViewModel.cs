namespace MachineMaintenanceApp.Web.ViewModels.Machines
{
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class MachineIdViewModel : IMapFrom<Machine>
    {
        public string Id { get; set; }
    }
}
