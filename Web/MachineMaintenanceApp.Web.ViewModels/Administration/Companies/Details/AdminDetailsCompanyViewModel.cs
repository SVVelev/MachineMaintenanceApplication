namespace MachineMaintenanceApp.Web.ViewModels.Administration.Companies.Details
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminDetailsCompanyViewModel : IMapFrom<Company>
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }

        [Display(Name = "Users")]
        public string UsersCount { get; set; }

        public bool IsDeleted { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
    }
}
