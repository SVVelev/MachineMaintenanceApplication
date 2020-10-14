namespace MachineMaintenanceApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Common.Models;
    using MachineMaintenanceApp.Data.Models.Enums;

    public class UnplannedRepair : BaseDeletableModel<string>
    {
        public UnplannedRepair()
        {
        }

        [Required]
        public RepairType Type { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Description { get; set; }

        public string PartNumber { get; set; }

        [Required]
        public string MachineId { get; set; }

        public virtual Machine Machine { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string SparePartId { get; set; }

        public virtual SparePart SparePart { get; set; }
    }
}
