namespace MachineMaintenanceApp.Web.ViewModels.Administration.PlannedRepairs.PlannedRepairsPage
{
    using System.Collections.Generic;

    public class PageAdminPlannedRepairsViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string MachineId { get; set; }

        public IEnumerable<AdminPlannedRepairsPageViewModel> PlannedRepairs { get; set; }
    }
}
