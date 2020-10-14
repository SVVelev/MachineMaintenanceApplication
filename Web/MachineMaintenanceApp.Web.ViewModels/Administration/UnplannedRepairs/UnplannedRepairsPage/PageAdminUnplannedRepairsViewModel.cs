namespace MachineMaintenanceApp.Web.ViewModels.Administration.UnplannedRepairs.UnplannedRepairsPage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class PageAdminUnplannedRepairsViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<AdminUnplannedRepairsPageViewModel> UnplannedRepairs { get; set; }
    }
}
