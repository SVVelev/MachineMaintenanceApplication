namespace MachineMaintenanceApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Common.Models;

    public class Machine : BaseDeletableModel<string>
    {
        public Machine()
        {
            this.UnplannedRepairs = new HashSet<UnplannedRepair>();
            this.PlannedRepairs = new HashSet<PlannedRepair>();
            this.DailyChecks = new HashSet<DailyCheck>();
            this.WeeklyChecks = new HashSet<WeeklyCheck>();
        }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string InventoryNumber { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string SerialNumber { get; set; }

        public string Model { get; set; }

        [MaxLength(30)]
        public string Manufacturer { get; set; }

        [RegularExpression("[0-9]{4}")]
        public string ManufactureYear { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<UnplannedRepair> UnplannedRepairs { get; set; }

        public virtual ICollection<PlannedRepair> PlannedRepairs { get; set; }

        public virtual ICollection<DailyCheck> DailyChecks { get; set; }

        public virtual ICollection<WeeklyCheck> WeeklyChecks { get; set; }

        public virtual ICollection<SparePart> SpareParts { get; set; }
    }
}
