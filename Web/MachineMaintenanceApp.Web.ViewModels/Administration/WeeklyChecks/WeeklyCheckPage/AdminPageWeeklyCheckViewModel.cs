namespace MachineMaintenanceApp.Web.ViewModels.Administration.WeeklyChecks.WeeklyCheckPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AdminPageWeeklyCheckViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<AdminWeeklyCheckPageViewModel> WeeklyChecks { get; set; }
    }
}
