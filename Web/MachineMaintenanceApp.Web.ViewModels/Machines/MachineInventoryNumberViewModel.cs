namespace MachineMaintenanceApp.Web.ViewModels.Machines
{
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class MachineInventoryNumberViewModel : IMapFrom<Machine>
    {
        public string InventoryNumber { get; set; }
    }
}
