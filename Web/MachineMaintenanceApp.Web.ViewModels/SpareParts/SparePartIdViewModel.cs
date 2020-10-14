
namespace MachineMaintenanceApp.Web.ViewModels.SpareParts
{
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class SparePartIdViewModel : IMapFrom<SparePart>
    {
        public string Id { get; set; }

        public string InventoryNumber { get; set; }
    }
}
