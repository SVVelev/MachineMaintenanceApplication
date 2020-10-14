namespace MachineMaintenanceApp.Web.ViewModels.Administration.DailyChecks.DailyCheckPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class AdminPageDailyChecksViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<AdminDailyCheckPageViewModel> DailyChecks { get; set; }
    }
}
