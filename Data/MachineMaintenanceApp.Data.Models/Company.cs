namespace MachineMaintenanceApp.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using MachineMaintenanceApp.Data.Common.Models;

    public class Company : BaseDeletableModel<string>
    {
        public Company()
        {
            this.Users = new HashSet<ApplicationUser>();
        }

        [Required]
        public string Name { get; set; }

        public string Logo { get; set; }

        public string Description { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
