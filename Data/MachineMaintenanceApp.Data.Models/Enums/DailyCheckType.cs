namespace MachineMaintenanceApp.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum DailyCheckType
    {
        [Display(Name = "Dust cleaning")]
        DustCleaning = 1,

        [Display(Name = "Buttons check")]
        ButtonsCheck = 2,

        [Display(Name = "Cables check")]
        CablesCheck = 3,

        [Display(Name = "Air leaks check")]
        AirLeakscheck = 4,
    }
}
