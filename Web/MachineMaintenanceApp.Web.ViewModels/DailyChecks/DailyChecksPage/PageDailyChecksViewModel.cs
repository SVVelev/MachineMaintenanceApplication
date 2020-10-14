using System.Collections.Generic;

namespace MachineMaintenanceApp.Web.ViewModels.DailyChecks.DailyChecksPage
{
    public class PageDailyChecksViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<DailyChecksPageViewModel> DailyChecks { get; set; }
    }
}
