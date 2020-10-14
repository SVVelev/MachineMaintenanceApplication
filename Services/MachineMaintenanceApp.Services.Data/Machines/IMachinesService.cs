namespace MachineMaintenanceApp.Services.Data.Machines
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface IMachinesService
    {
        Task<string> CreateAsync(string inventoryNumber, string serialNumber, string model, string manufacturer, string manufactureYear, string description, string imageUrl, string userId);

        Task<string> EditAsync(string inventoryNumber, string serialNumber, string model, string manufacturer, string manufactureYear, string description, string imageUrl, string id, ApplicationUser user);

        T GetById<T>(string id);

        Task<string> DeleteAsync(string id, ApplicationUser user);

        IEnumerable<T> GetAll<T>(ApplicationUser user);

        IEnumerable<SelectListItem> GetInvenroryNumbers(ApplicationUser user, string companyId = null);

        T GetByInventoryNumber<T>(string inventoryNumber);

        bool CheckAccess(ApplicationUser user, string machineId);

        IEnumerable<T> GetAllWithDeleted<T>();

        T GetByIdWithDeleted<T>(string id);

        IEnumerable<T> GetAllForCompany<T>(string companyId, int? take = null, int skip = 0);

        Task UndeleteAsync(string id);

        int GetCount(string companyId);

        int GetCountWithDeleted(string companyId);
    }
}
