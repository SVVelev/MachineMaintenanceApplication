namespace MachineMaintenanceApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Common.Models;
    using MachineMaintenanceApp.Data.Models.Enums;

    public class DailyCheck : BaseDeletableModel<string>
    {
        [Required]
        public DailyCheckType Type { get; set; }

        public string Notes { get; set; }

        [Required]
        public string MachineId { get; set; }

        public virtual Machine Machine { get; set; }

        [Required]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
