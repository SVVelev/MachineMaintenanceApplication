namespace MachineMaintenanceApp.Services.Data.Machines
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Services.Mapping;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class MachineService : IMachinesService
    {
        private readonly IDeletableEntityRepository<Machine> machineRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public MachineService(IDeletableEntityRepository<Machine> machineRepository, UserManager<ApplicationUser> userManager)
        {
            this.machineRepository = machineRepository;

            this.userManager = userManager;
        }

        public async Task<string> CreateAsync(string inventoryNumber, string serialNumber, string model, string manufacturer, string manufactureYear, string description, string imageUrl, string userId)
        {
            var machine = new Machine
            {
                Id = Guid.NewGuid().ToString(),
                InventoryNumber = inventoryNumber,
                SerialNumber = serialNumber,
                Model = model,
                Manufacturer = manufacturer,
                ManufactureYear = manufactureYear,
                Description = description,
                ImageUrl = imageUrl,
                UserId = userId,
            };

            await this.machineRepository.AddAsync(machine);
            await this.machineRepository.SaveChangesAsync();
            return machine.Id;
        }

        public async Task<string> EditAsync(string inventoryNumber, string serialNumber, string model, string manufacturer, string manufactureYear, string description, string imageUrl, string id, ApplicationUser user)
        {
            var currentMachine = this.machineRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (currentMachine == null)
            {
                throw new ArgumentNullException($"Machine with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentMachine.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            currentMachine.InventoryNumber = inventoryNumber;
            currentMachine.SerialNumber = serialNumber;
            currentMachine.Model = model;
            currentMachine.Manufacturer = manufacturer;
            currentMachine.ManufactureYear = manufactureYear;
            currentMachine.Description = description;
            currentMachine.ImageUrl = imageUrl;

            this.machineRepository.Update(currentMachine);
            await this.machineRepository.SaveChangesAsync();
            return currentMachine.Id;
        }

        public T GetById<T>(string id)
        {
            var machine = this.machineRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return machine;
        }

        public async Task<string> DeleteAsync(string id, ApplicationUser user)
        {
            var currentMachine = this.machineRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (currentMachine == null)
            {
                throw new ArgumentNullException($"Machine with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentMachine.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            this.machineRepository.Delete(currentMachine);
            await this.machineRepository.SaveChangesAsync();

            return currentMachine.User.CompanyId;
        }

        public IEnumerable<T> GetAll<T>(ApplicationUser user)
        {
            IQueryable<Machine> query = Enumerable.Empty<Machine>().AsQueryable();

            if (user != null)
            {
                query =
                this.machineRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.User.CompanyId == user.CompanyId);
            }

            return query.To<T>().ToList();
        }

        public IEnumerable<SelectListItem> GetInvenroryNumbers(ApplicationUser user, string companyId = null)
        {
            List<SelectListItem> inventoryNumbers = new List<SelectListItem>();
            IQueryable<string> query = Enumerable.Empty<string>().AsQueryable();

            if (companyId == null)
            {
                query =
                this.machineRepository
                .All()
                .Where(x => x.User.CompanyId == user.CompanyId)
                .Select(x => x.InventoryNumber);
            }
            else
            {
                query =
                this.machineRepository
                .All()
                .Where(x => x.User.CompanyId == companyId)
                .Select(x => x.InventoryNumber);
            }

            foreach (var item in query)
            {
                inventoryNumbers.Add(new SelectListItem { Value = item, Text = item });
            }

            inventoryNumbers.Add(new SelectListItem { Value = string.Empty, Text = "Without Machine" });

            return inventoryNumbers;
        }

        public T GetByInventoryNumber<T>(string inventoryNumber)
        {
            var machine = this.machineRepository
                .All().
                Where(x => x.InventoryNumber == inventoryNumber)
                .To<T>().FirstOrDefault();

            return machine;
        }

        public bool CheckAccess(ApplicationUser user, string machineId)
        {
            var currentCheck = this.machineRepository
                .All()
                .FirstOrDefault(x => x.Id == machineId);

            return currentCheck.UserId == user.Id;
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            IQueryable<Machine> query =
                this.machineRepository
                .AllWithDeleted();

            return query.To<T>().ToList();
        }

        public T GetByIdWithDeleted<T>(string id)
        {
            var user = this.machineRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public IEnumerable<T> GetAllForCompany<T>(string companyId, int? take = null, int skip = 0)
        {
            IQueryable<Machine> query =
                this.machineRepository
                .AllWithDeleted()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.User.CompanyId == companyId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public async Task UndeleteAsync(string id)
        {
            var currentMachine = this.machineRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentMachine == null)
            {
                throw new ArgumentNullException($"Machine with {id} does not exist!");
            }

            this.machineRepository.Undelete(currentMachine);
            await this.machineRepository.SaveChangesAsync();
        }

        public int GetCount(string companyId)
        {
            return this.machineRepository
            .All()
            .Count(x => x.User.CompanyId == companyId);
        }

        public int GetCountWithDeleted(string companyId)
        {
            return this.machineRepository
            .AllWithDeleted()
            .Count(x => x.User.CompanyId == companyId);
        }
    }
}
