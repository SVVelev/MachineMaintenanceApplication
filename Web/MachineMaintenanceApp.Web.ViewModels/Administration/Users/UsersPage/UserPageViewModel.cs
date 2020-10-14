namespace MachineMaintenanceApp.Web.ViewModels.Administration.Users.UsersPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class UserPageViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string CompanyId { get; set; }

        public IEnumerable<AdminUserPageViewModel> Users { get; set; }
    }
}
