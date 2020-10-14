namespace MachineMaintenanceApp.Web.ViewModels.SpareParts.SparePartsHomePage
{

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class SparePartsHomePageViewModel : IMapFrom<SparePart>
    {
        public string Id { get; set; }

        public string InventoryNumber { get; set; }

        public SparePartType Type { get; set; }

        public string MachineInventoryNumber { get; set; }

        public int Quantity { get; set; }
    }
}
