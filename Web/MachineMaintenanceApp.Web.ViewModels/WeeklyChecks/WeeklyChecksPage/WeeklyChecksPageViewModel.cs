namespace MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.WeeklyChecksPage
{
    using System;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class WeeklyChecksPageViewModel : IMapFrom<WeeklyCheck>
    {
        public string Id { get; set; }

        public WeeklyCheckType Type { get; set; }

        public string Notes { get; set; }

        public string MachineInventoryNumber { get; set; }

        public string UserLastName { get; set; }

        public DateTime CreatedOn { get; set; }

        public string UserId { get; set; }
    }
}
