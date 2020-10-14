namespace MachineMaintenanceApp.Data.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum RepairType
    {
        [Display(Name = "Electrical repair")]
        ElectricalRepair = 1,

        [Display(Name = "Mechanical repair")]
        MechanicalRepair = 2,

        [Display(Name = "Pneumatical reapir")]
        PneumaticalRepair = 3,

        [Display(Name = "Hydraulic repair")]
        HydraulicRepair = 4,
    }
}
