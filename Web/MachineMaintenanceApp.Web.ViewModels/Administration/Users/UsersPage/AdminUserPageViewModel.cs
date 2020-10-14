namespace MachineMaintenanceApp.Web.ViewModels.Administration.Users.UsersPage
{
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Mapping;

    public class AdminUserPageViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public PositionType Position { get; set; }

        public bool IsDeleted { get; set; }
    }
}
