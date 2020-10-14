namespace MachineMaintenanceApp.Web.ViewModels.Machines
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;

    public class MachineCompanyId : IMapFrom<Machine>
    {
        public string UserCompanyId { get; set; }
    }
}
