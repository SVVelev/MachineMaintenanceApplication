namespace MachineMaintenanceApp.Web.ViewModels.Administration.WeeklyChecks.Details
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminWeeklyDetailsViewModel : IMapFrom<WeeklyCheck>
    {
        public string Id { get; set; }

        [Display(Name = "Type")]
        public WeeklyCheckType Type { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Machine")]
        public string MachineInventoryNumber { get; set; }

        [Display(Name = "Created by")]
        public string UserLastName { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }

        public bool IsDeleted { get; set; }

        public string MachineId { get; set; }
    }
}
