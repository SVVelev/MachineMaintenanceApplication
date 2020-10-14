namespace MachineMaintenanceApp.Web.ViewModels.PlannedRepairs.PlannedRepairsPage
{
    using System.Collections.Generic;

    public class PagePlannedRepairsViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<PlannedRepairsPageViewModel> PlannedRepairs { get; set; }
    }
}
