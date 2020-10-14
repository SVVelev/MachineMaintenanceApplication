namespace MachineMaintenanceApp.Web.ViewModels.DailyChecks.DailyChecksPage
{
    using System;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class DailyChecksPageViewModel : IMapFrom<DailyCheck>
    {
        public string Id { get; set; }

        public DailyCheckType Type { get; set; }

        public string Notes { get; set; }

        public string MachineInventoryNumber { get; set; }

        public string UserLastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }
    }
}
