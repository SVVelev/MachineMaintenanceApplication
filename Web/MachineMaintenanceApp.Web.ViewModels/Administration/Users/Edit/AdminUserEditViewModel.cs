namespace MachineMaintenanceApp.Web.ViewModels.Administration.Users.Edit
{
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminUserEditViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "First Name")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [Required]
        public string LastName { get; set; }

        [Display(Name = "Card Number")]
        [RegularExpression("[0-9]{6}")]
        [Required]
        public string CardNumber { get; set; }

        [Display(Name = "Profil Image")]
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public PositionType Position { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
