namespace MachineMaintenanceApp.Web.ViewModels.WeeklyChecks.WeeklyChecksPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PageWeeklyChecksViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<WeeklyChecksPageViewModel> WeeklyChecks { get; set; }
    }
}
