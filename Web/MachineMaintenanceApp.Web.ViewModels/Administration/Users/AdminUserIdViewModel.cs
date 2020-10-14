namespace MachineMaintenanceApp.Web.ViewModels.Administration.Users
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminUserIdViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
    }
}
