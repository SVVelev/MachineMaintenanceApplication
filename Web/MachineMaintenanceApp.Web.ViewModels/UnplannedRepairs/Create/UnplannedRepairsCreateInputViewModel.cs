namespace MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.Create
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models.Enums;

    public class UnplannedRepairsCreateInputViewModel
    {
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

        [Display(Name = "Spare Part")]
        public string PartNumber { get; set; }

        public string MachineId { get; set; }
    }
}
