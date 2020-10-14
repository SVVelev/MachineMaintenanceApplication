 namespace MachineMaintenanceApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Common.Models;
    using MachineMaintenanceApp.Data.Models.Enums;

    public class SparePart : BaseDeletableModel<string>
    {
        public SparePart()
        {
            this.PlannedRepairs = new HashSet<PlannedRepair>();
            this.UnplannedRepairs = new HashSet<UnplannedRepair>();
        }

        [Required]
        public SparePartType Type { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string SerialNumber { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string InventoryNumber { get; set; }

        [MaxLength(20)]
        public string Manufacturer { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Range(0, double.MaxValue)]
        public int Quantity { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string MachineId { get; set; }

        public virtual Machine Machine { get; set; }

        public virtual ICollection<PlannedRepair> PlannedRepairs { get; set; }

        public virtual ICollection<UnplannedRepair> UnplannedRepairs { get; set; }
    }
}
