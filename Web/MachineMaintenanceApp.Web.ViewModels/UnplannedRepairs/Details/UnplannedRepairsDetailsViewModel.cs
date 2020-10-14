namespace MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.Details
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class UnplannedRepairsDetailsViewModel : IMapFrom<UnplannedRepair>
    {
        public string Id { get; set; }

        [Display(Name = "Repair type")]
        public RepairType Type { get; set; }

        [Display(Name = "Start")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End")]
        public DateTime EndTime { get; set; }

        public string Description { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }

        [Display(Name = "Created By")]
        public string UserLastName { get; set; }

        [Display(Name = "Spare Part")]
        public string PartNumber { get; set; }

        public string MachineId { get; set; }

        public string UserId { get; set; }
    }
}
