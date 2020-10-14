namespace MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.PlannedRepairsPage
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class UnplannedRepairsPageViewModel : IMapFrom<UnplannedRepair>
    {
        public string Id { get; set; }

        public RepairType Type { get; set; }

        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }

        [Display(Name = "Created By")]
        public string UserLastName { get; set; }
    }
}
