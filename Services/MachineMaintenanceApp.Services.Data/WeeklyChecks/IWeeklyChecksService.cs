namespace MachineMaintenanceApp.Services.Data.WeeklyChecks
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;

    public interface IWeeklyChecksService
    {
        Task<string> CreateAsync(WeeklyCheckType type, string id, string notes, string userId);

        Task<string> EditAsync(WeeklyCheckType type, string notes, string id, ApplicationUser user);

        Task<string> DeleteAsync(string id, ApplicationUser user);

        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>(string id, int? take = null, int skip = 0);

        bool CheckAccess(ApplicationUser user, string checkId);

        IEnumerable<T> GetAllWithDeleted<T>(string id, int? take = null, int skip = 0);

        T GetByIdWithDeleted<T>(string id);

        Task<string> UndeleteAsync(string id);

        int GetCount(string machineId);

        int GetCountWithDeleted(string machineId);
    }
}
