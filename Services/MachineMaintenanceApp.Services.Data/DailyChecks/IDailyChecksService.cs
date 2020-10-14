namespace MachineMaintenanceApp.Services.Data.DailyChecks
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;

    public interface IDailyChecksService
    {
        Task<string> CreateAsync(DailyCheckType type, string id, string notes, string userId);

        Task<string> EditAsync(DailyCheckType type, string notes, string id, string machineId, ApplicationUser user);

        Task<string> DeleteAsync(string id, ApplicationUser user);

        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>(string id, int? take = null, int skip = 0);

        bool CheckAccess(ApplicationUser user, string checkId);

        T GetByIdWithDeleted<T>(string id);

        Task UndeleteAsync(string id);

        IEnumerable<T> GetAllWithDeleted<T>(string machineId, int? take = null, int skip = 0);

        int GetCount(string machineId);

        int GetCountWithDeleted(string machineId);
    }
}
