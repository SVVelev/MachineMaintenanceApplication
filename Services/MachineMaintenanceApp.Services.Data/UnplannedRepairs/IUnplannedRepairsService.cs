namespace MachineMaintenanceApp.Services.Data.UnplannedRepairs
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;

    public interface IUnplannedRepairsService
    {
        Task<string> CreateAsync(RepairType type, DateTime startTime, DateTime endTime, string description, string machineId, string userId, string sparePartInventoryNumber);

        Task<string> EditAsync(RepairType type, DateTime startTime, DateTime endTime, string description, string sparePartInventoryNumber, string id, ApplicationUser user);

        Task<string> DeleteAsync(string id, ApplicationUser user);

        T GetById<T>(string id);

        IEnumerable<T> GetAll<T>(string id, int? take = null, int skip = 0);

        bool CheckAccess(ApplicationUser user, string repairId);

        IEnumerable<T> GetAllWithDeleted<T>(string machineId, int? take = null, int skip = 0);

        T GetByIdWithDeleted<T>(string id);

        Task<string> UndeleteAsync(string id);

        int GetCount(string machineId);

        int GetCountWithDeleted(string machineId);
    }
}
