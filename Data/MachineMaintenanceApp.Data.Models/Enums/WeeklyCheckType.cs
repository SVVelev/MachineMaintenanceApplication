namespace MachineMaintenanceApp.Data.Models.Enums
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public enum WeeklyCheckType
    {
        [Display(Name = "Emergency button check")]
        EmergencyButtonCheck = 1,

        [Display(Name = "Mechanical connections check")]
        MechanicalConnectionsCheck = 2,

        [Display(Name = "Machine settings check")]
        MachineSettingsCheck = 3,
    }
}
