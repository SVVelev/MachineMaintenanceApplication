namespace MachineMaintenanceApp.Web.ViewModels.Administration.Users.Details
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminUserDetailViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Card number")]
        public string CardNumber { get; set; }

        [Display(Name = "Profil picture")]
        public string ImageUrl { get; set; }

        public PositionType Position { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Deleted ?")]
        public bool IsDeleted { get; set; }

        public string CompanyId { get; set; }

        [Display(Name = "Created on")]
        public DateTime CreatedOn { get; set; }
    }
}
