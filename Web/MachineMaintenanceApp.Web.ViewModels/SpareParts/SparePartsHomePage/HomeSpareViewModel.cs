namespace MachineMaintenanceApp.Web.ViewModels.SpareParts.SparePartsHomePage
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class HomeSpareViewModel
    {
        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public IEnumerable<SparePartsHomePageViewModel> SpareParts { get; set; }
    }
}
