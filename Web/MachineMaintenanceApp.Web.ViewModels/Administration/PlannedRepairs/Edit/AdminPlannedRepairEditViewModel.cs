namespace MachineMaintenanceApp.Web.ViewModels.Administration.PlannedRepairs.Edit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminPlannedRepairEditViewModel : IMapFrom<PlannedRepair>
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Repair Type")]
        public RepairType Type { get; set; }

        [Required]
        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End")]
        public DateTime EndTime { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Interval between repairs")]
        public double RepairsIntervalDays { get; set; }

        [Display(Name = "Spare Part")]
        public string PartNumber { get; set; }

        public string MachineId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
