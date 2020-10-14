namespace MachineMaintenanceApp.Services.Data.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        T GetById<T>(string id);

        string GetImageById(string id);

        Task<string> EditAsync(string phoneNumber, string firstName, string lastName, string cardNumber, string imageUrl, PositionType position, string userId, string username);

        Task<string> DeleteAsync(string id);

        Task<string> UndeleteAsync(string id);

        Task<string> ResetPassword(string id);

        IEnumerable<T> GetAllForCompany<T>(string companyId);

        IEnumerable<T> GetAllWithDeleted<T>();

        IEnumerable<T> GetAllForCompanyWithDeleted<T>(string companyId, int? take = null, int skip = 0);

        T GetByIdWithDeleted<T>(string id);

        int GetCountAllForCompanyWithDeleted(string companyId);
    }
}
