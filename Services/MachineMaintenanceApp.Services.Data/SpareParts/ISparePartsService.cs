namespace MachineMaintenanceApp.Services.Data.SpareParts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ISparePartsService
    {
        Task<string> CreateAsync(SparePartType type, string serialNumber, string inventoryNumber, string manufacturer, string description, string imageUrl, int quantity, string machineInventoryNumber, string userId);

        Task<string> EditAsync(SparePartType type, string serialNumber, string inventoryNumber, string manufacturer, string description, string imageUrl, int quantity, string machineInventoryNumber, string id, ApplicationUser user);

        T GetById<T>(string id);

        Task<string> DeleteAsync(string id, ApplicationUser user);

        // IEnumerable<T> GetAll<T>(ApplicationUser user);

        IEnumerable<T> GetAllForCurrentMachine<T>(string id, int? take = null, int skip = 0);

        IEnumerable<SelectListItem> GetInvenroryNumbers(string machineId);

        T GetByInventoryNumber<T>(string inventoryNumber);

        bool CheckSparePartQuantity(string sparePartId);

        Task<string> UseSparePart(string sparePartId);

        Task<string> ReturnSparePart(string sparePartId);

        Task<string> IncreaseSparePartQuantity(string sparePartId, int quantity);

        bool CheckAccess(ApplicationUser user, string partId);

        IEnumerable<T> GetAllWithDeleted<T>();

        T GetByIdWithDeleted<T>(string id);

        IEnumerable<T> GetAllForCompany<T>(string companyId, int? take = null, int skip = 0);

        Task<string> UndeleteAsync(string id);

        IEnumerable<T> GetAll<T>(ApplicationUser user, int? take = null, int skip = 0);

        int GetCount(ApplicationUser user);

        int GetCountForCompany(string companyId);

        int GetCountForCompanyWithDeleted(string companyId);
    }
}
