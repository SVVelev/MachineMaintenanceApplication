namespace MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.Create
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models.Enums;

    public class WeeklyCreateInputViewModel
    {
        [Required]
        [Display(Name = "Weekly check type")]
        public WeeklyCheckType Type { get; set; }

        public string Notes { get; set; }

        public string MachineId { get; set; }

    }
}
