namespace MachineMaintenanceApp.Services.Data.Comapnies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Data.Users;
    using MachineMaintenanceApp.Services.Mapping;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class CompanyService : ICompanyService
    {
        private readonly IDeletableEntityRepository<Company> companyRepository;
        private readonly IDeletableEntityRepository<ApplicationUser> userRepository;

        public CompanyService(IDeletableEntityRepository<Company> companyRepository, IDeletableEntityRepository<ApplicationUser> userRepository)
        {
            this.companyRepository = companyRepository;
            this.userRepository = userRepository;
        }

        public async Task<string> CreateAsync(string name, string logo, string description)
        {
            var company = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Logo = logo,
                Description = description,
            };

            await this.companyRepository.AddAsync(company);
            await this.companyRepository.SaveChangesAsync();
            return company.Id;
        }

        public async Task<string> EditAsync(string name, string logo, string description, string id)
        {
            var currentCompany = this.companyRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (currentCompany == null)
            {
                throw new ArgumentNullException($"Company with {id} does not exist!");
            }

            currentCompany.Name = name;
            currentCompany.Logo = logo;
            currentCompany.Description = description;

            this.companyRepository.Update(currentCompany);
            await this.companyRepository.SaveChangesAsync();
            return currentCompany.Id;
        }

        public async Task DeleteAsync(string id)
        {
            var currentCompany = this.companyRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (currentCompany == null)
            {
                throw new ArgumentNullException($"Company with {id} does not exist!");
            }

            foreach (var user in currentCompany.Users)
            {
                this.userRepository.Delete(user);
            }

            await this.userRepository.SaveChangesAsync();
            this.companyRepository.Delete(currentCompany);
            await this.companyRepository.SaveChangesAsync();
        }

        public async Task<string> UndeleteAsync(string id)
        {
            var currentCompany = this.companyRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentCompany == null)
            {
                throw new ArgumentNullException($"Company with {id} does not exist!");
            }

            foreach (var user in currentCompany.Users)
            {
                this.userRepository.Undelete(user);
            }

            await this.userRepository.SaveChangesAsync();
            this.companyRepository.Undelete(currentCompany);
            await this.companyRepository.SaveChangesAsync();

            return currentCompany.Id;
        }

        public IEnumerable<SelectListItem> GetNames()
        {
            List<SelectListItem> companyName = new List<SelectListItem>();

            IQueryable<string> query =
                this.companyRepository
                .All()
                .Select(x => x.Name);

            foreach (var item in query)
            {
                if (item == "AdminCompany")
                {
                    continue;
                }

                companyName.Add(new SelectListItem { Value = item, Text = item });
            }

            return companyName;
        }

        public T GetByName<T>(string name)
        {
            var company = this.companyRepository
                .All()
                .Where(x => x.Name == name)
                .To<T>().FirstOrDefault();

            return company;
        }

        public T GetById<T>(string id)
        {
            var company = this.companyRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return company;
        }

        public IEnumerable<T> GetAll<T>()
        {
            IQueryable<Company> query = this.companyRepository
                .All()
                .OrderBy(x => x.Name);
            return query.To<T>().ToList();
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            IQueryable<Company> query = this.companyRepository
                .AllWithDeleted()
                .OrderBy(x => x.Name);

            return query.To<T>().ToList();
        }

        public T GetByIdWithDeleted<T>(string id)
        {
            var user = this.companyRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }
    }
}
