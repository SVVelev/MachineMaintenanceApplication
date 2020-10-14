namespace MachineMaintenanceApp.Web.ViewModels.UnplannedRepairs.PlannedRepairsPage
{
    using System.Collections.Generic;

    public class PageUnplannedRepairsViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<UnplannedRepairsPageViewModel> UnplannedRepairs { get; set; }
    }
}
