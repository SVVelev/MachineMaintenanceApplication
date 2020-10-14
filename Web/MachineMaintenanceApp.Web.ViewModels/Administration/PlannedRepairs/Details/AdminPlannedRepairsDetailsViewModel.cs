namespace MachineMaintenanceApp.Web.ViewModels.Administration.PlannedRepairs.Details
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminPlannedRepairsDetailsViewModel : IMapFrom<PlannedRepair>
    {
        public string Id { get; set; }

        [Display(Name = "Repair type")]
        public RepairType Type { get; set; }

        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End")]
        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        [Display(Name = "Interval between repairs in days")]
        public double RepairsIntervalDays { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }

        [Display(Name = "Created By")]
        public string UserLastName { get; set; }

        [Display(Name = "Spare Part")]
        public string PartNumber { get; set; }

        public string MachineId { get; set; }

        public string UserId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
