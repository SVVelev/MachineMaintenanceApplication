namespace MachineMaintenanceApp.Web.ViewModels.DailyChecks
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class DetailsDailyChecksViewModel : IMapFrom<DailyCheck>
    {
        public string Id { get; set; }

        [Display(Name = "Type")]
        public DailyCheckType Type { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }

        [Display(Name = "Created by")]
        public string UserLastName { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        public string MachineId { get; set; }

        public string UserId { get; set; }
    }
}
