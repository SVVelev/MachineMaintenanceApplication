namespace MachineMaintenanceApp.Services.Data.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Data.Comapnies;
    using MachineMaintenanceApp.Services.Mapping;
    using MachineMaintenanceApp.Web.ViewModels.Companies;
    using Microsoft.AspNetCore.Identity;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICompanyService companyService;

        public UserService(
            IDeletableEntityRepository<ApplicationUser> userRepository,
            UserManager<ApplicationUser> userManager,
            ICompanyService companyService)
        {
            this.userRepository = userRepository;
            this.userManager = userManager;
            this.companyService = companyService;
        }

        public async Task<string> EditAsync(string phoneNumber, string firstName, string lastName, string cardNumber, string imageUrl, PositionType position, string userId, string username)
        {
            var currentUser = this.userRepository
                .All()
                .Where(x => x.Id == userId)
                .FirstOrDefault();

            var userOldRole = currentUser.Position.ToString();

            currentUser.PhoneNumber = phoneNumber;
            currentUser.FirstName = firstName;
            currentUser.LastName = lastName;
            currentUser.CardNumber = cardNumber;
            currentUser.ImageUrl = imageUrl;
            currentUser.Position = position;
            currentUser.UserName = username;

            var userNewRole = position.ToString();
            await this.userManager.RemoveFromRolesAsync(currentUser, new List<string> { userOldRole });
            await this.userManager.AddToRoleAsync(currentUser, userNewRole);

            this.userRepository.Update(currentUser);
            await this.userRepository.SaveChangesAsync();

            return currentUser.Id;
        }

        public async Task<string> DeleteAsync(string id)
        {
            var user = this.userRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException($"User with {id} does not exist!");
            }

            this.userRepository.Delete(user);
            await this.userRepository.SaveChangesAsync();

            return user.CompanyId;
        }

        public async Task<string> UndeleteAsync(string id)
        {
            var user = this.userRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException($"User with {id} does not exist!");
            }

            this.userRepository.Undelete(user);
            await this.userRepository.SaveChangesAsync();

            return user.Id;
        }

        public async Task<string> ResetPassword(string id)
        {
            var user = this.userRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new ArgumentNullException($"User with {id} does not exist!");
            }

            string resetPassword = "00000000";
            user.PasswordHash = this.userManager.PasswordHasher.HashPassword(user, resetPassword);

            this.userRepository.Update(user);
            await this.userRepository.SaveChangesAsync();

            return user.Id;
        }

        public T GetById<T>(string id)
        {
            var user = this.userRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public string GetImageById(string id)
        {
            var userImage = this.userRepository
                .All()
                .Where(x => x.Id == id)
                .Select(x => x.ImageUrl)
                .FirstOrDefault();

            return userImage;
        }

        public IEnumerable<T> GetAllForCompany<T>(string companyId)
        {
                var query =
                this.userRepository
                .All()
                .Where(x => x.CompanyId == companyId);

                return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllForCompanyWithDeleted<T>(string companyId, int? take = null, int skip = 0)
        {
            var query =
            this.userRepository
            .AllWithDeleted()
            .OrderByDescending(x => x.CreatedOn)
            .Where(x => x.CompanyId == companyId)
            .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            IQueryable<ApplicationUser> query =
                this.userRepository
                .AllWithDeleted();

            return query.To<T>().ToList();
        }

        public T GetByIdWithDeleted<T>(string id)
        {
            var user = this.userRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public int GetCountAllForCompanyWithDeleted(string companyId)
        {
            return this.userRepository
            .AllWithDeleted()
            .Count(x => x.CompanyId == companyId);
        }

        private string Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
