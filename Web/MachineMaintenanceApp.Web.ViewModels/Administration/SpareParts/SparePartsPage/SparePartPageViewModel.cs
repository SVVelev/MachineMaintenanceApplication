namespace MachineMaintenanceApp.Web.ViewModels.Administration.SpareParts.SparePartsPage
{
    using System.Collections.Generic;

    public class SparePartPageViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public string CompanyId { get; set; }

        public IEnumerable<AdminSparePartPageViewModel> SpareParts { get; set; }
    }
}
