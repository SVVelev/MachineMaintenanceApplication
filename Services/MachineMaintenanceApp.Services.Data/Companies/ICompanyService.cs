namespace MachineMaintenanceApp.Services.Data.Comapnies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;

    public interface ICompanyService
    {
        Task<string> CreateAsync(string name, string logo, string description);

        Task<string> EditAsync(string name, string logo, string description, string id);

        Task DeleteAsync(string id);

        Task<string> UndeleteAsync(string id);

        IEnumerable<SelectListItem> GetNames();

        T GetByName<T>(string name);

        IEnumerable<T> GetAll<T>();

        T GetById<T>(string id);

        IEnumerable<T> GetAllWithDeleted<T>();

        T GetByIdWithDeleted<T>(string id);
    }
}
