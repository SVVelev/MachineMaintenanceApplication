// ReSharper disable VirtualMemberCallInConstructor
namespace MachineMaintenanceApp.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Common.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            this.PlannedRepairs = new HashSet<PlannedRepair>();
            this.UnplannedRepairs = new HashSet<UnplannedRepair>();
            this.WeeklyChecks = new HashSet<WeeklyCheck>();
            this.DailyChecks = new HashSet<DailyCheck>();
            this.Machines = new HashSet<Machine>();
            this.Parts = new HashSet<SparePart>();
        }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(20)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression("[0-9]{6}")]
        public string CardNumber { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public PositionType Position { get; set; }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        [Required]
        public string CompanyId { get; set; }

        public virtual Company Company { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<PlannedRepair> PlannedRepairs { get; set; }

        public virtual ICollection<UnplannedRepair> UnplannedRepairs { get; set; }

        public virtual ICollection<WeeklyCheck> WeeklyChecks { get; set; }

        public virtual ICollection<DailyCheck> DailyChecks { get; set; }

        public virtual ICollection<Machine> Machines { get; set; }

        public virtual ICollection<SparePart> Parts { get; set; }
    }
}
