namespace MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.Edit
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class WeeklyEditInputViewModel : IMapFrom<WeeklyCheck>
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Weekly check type")]
        public WeeklyCheckType Type { get; set; }

        public string Notes { get; set; }

        public string MachineId { get; set; }

    }
}
