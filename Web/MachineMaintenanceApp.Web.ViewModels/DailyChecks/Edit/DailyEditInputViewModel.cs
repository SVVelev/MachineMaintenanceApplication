namespace MachineMaintenanceApp.Web.ViewModels.DailyChecks.Edit
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class DailyEditInputViewModel : IMapFrom<DailyCheck>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Daily check type")]
        public DailyCheckType Type { get; set; }

        public string Notes { get; set; }

        public string MachineId { get; set; }
    }
}
