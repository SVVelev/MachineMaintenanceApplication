namespace MachineMaintenanceApp.Web.ViewModels.DailyChecks.Create
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models.Enums;

    public class DailyCreateInputViewModel
    {
        [Required]
        [Display(Name = "Weekly check type")]
        public DailyCheckType Type { get; set; }

        public string Notes { get; set; }

        public string MachineId { get; set; }
    }
}
