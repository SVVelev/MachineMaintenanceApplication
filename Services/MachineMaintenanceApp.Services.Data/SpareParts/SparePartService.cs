namespace MachineMaintenanceApp.Services.Data.SpareParts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Services.Mapping;
    using MachineMaintenanceApp.Web.ViewModels.Machines;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class SparePartService : ISparePartsService
    {
        private readonly IDeletableEntityRepository<SparePart> sparePartRepository;
        private readonly IMachinesService machineService;
        private readonly UserManager<ApplicationUser> userManager;

        public SparePartService(IDeletableEntityRepository<SparePart> sparePartRepository, IMachinesService machineService, UserManager<ApplicationUser> userManager)
        {
            this.sparePartRepository = sparePartRepository;
            this.machineService = machineService;
            this.userManager = userManager;
        }

        public async Task<string> CreateAsync(SparePartType type, string serialNumber, string inventoryNumber, string manufacturer, string description, string imageUrl, int quantity, string machineInventoryNumber, string userId)
        {
            var machine = this.machineService.GetByInventoryNumber<MachineIdViewModel>(machineInventoryNumber);

            var sparePart = new SparePart
            {
                Id = Guid.NewGuid().ToString(),
                InventoryNumber = inventoryNumber,
                SerialNumber = serialNumber,
                Type = type,
                Manufacturer = manufacturer,
                Description = description,
                ImageUrl = imageUrl,
                Quantity = quantity,
                UserId = userId,
            };

            if (!string.IsNullOrEmpty(machineInventoryNumber))
            {
                sparePart.MachineId = machine.Id;
            }

            await this.sparePartRepository.AddAsync(sparePart);
            await this.sparePartRepository.SaveChangesAsync();
            return sparePart.Id;
        }

        public async Task<string> EditAsync(SparePartType type, string serialNumber, string inventoryNumber, string manufacturer, string description, string imageUrl, int quantity, string machineInventoryNumber, string id, ApplicationUser user)
        {
            var machine = this.machineService.GetByInventoryNumber<MachineIdViewModel>(machineInventoryNumber);

            var currentPart = this.sparePartRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (currentPart == null)
            {
                throw new ArgumentNullException($"Spare part with {id} does not exist!");
            }

            if ((currentPart.UserId != user.Id) && (!await this.userManager.IsInRoleAsync(user, "Administrator")))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            currentPart.Type = type;
            currentPart.SerialNumber = serialNumber;
            currentPart.InventoryNumber = inventoryNumber;
            currentPart.Manufacturer = manufacturer;
            currentPart.Description = description;
            currentPart.ImageUrl = imageUrl;
            currentPart.Quantity = quantity;

            if (!string.IsNullOrEmpty(machineInventoryNumber))
            {
                currentPart.MachineId = machine.Id;
            }
            else
            {
                currentPart.MachineId = null;
            }

            this.sparePartRepository.Update(currentPart);
            await this.sparePartRepository.SaveChangesAsync();
            return currentPart.Id;
        }

        public async Task<string> DeleteAsync(string id, ApplicationUser user)
        {
            var currentPart = this.sparePartRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (currentPart == null)
            {
                throw new ArgumentNullException($"Spare part with {id} does not exist!");
            }

            if (currentPart.UserId != user.Id)
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            var currentPartCompanyId = currentPart.User.CompanyId;

            this.sparePartRepository.Delete(currentPart);
            await this.sparePartRepository.SaveChangesAsync();

            return currentPartCompanyId;
        }

        public IEnumerable<T> GetAllForCurrentMachine<T>(string machineId, int? take = null, int skip = 0)
        {
            IQueryable<SparePart> query = Enumerable.Empty<SparePart>().AsQueryable();

            query =
                 this.sparePartRepository
                 .All()
                 .Where(x => x.MachineId == machineId)
                 .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            var sparePart = this.sparePartRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return sparePart;
        }

        public IEnumerable<SelectListItem> GetInvenroryNumbers(string machineId)
        {
            List<SelectListItem> inventoryNumbers = new List<SelectListItem>();

            var currentMachine = this.machineService.GetById<MachineCompanyId>(machineId);

            if (currentMachine == null)
            {
                throw new ArgumentNullException($"Spare part with {machineId} does not exist!");
            }

            IQueryable<string> query =
                this.sparePartRepository
                .All()
                .Where(x => x.MachineId == machineId || (x.MachineId == null && x.User.CompanyId == currentMachine.UserCompanyId))
                .Select(x => x.InventoryNumber);

            foreach (var item in query)
            {
                inventoryNumbers.Add(new SelectListItem { Value = item, Text = item });
            }

            inventoryNumbers.Add(new SelectListItem { Value = string.Empty, Text = "Without Spare Part" });

            return inventoryNumbers;
        }

        public T GetByInventoryNumber<T>(string inventoryNumber)
        {
            var sparePart = this.sparePartRepository
                .All()
                .Where(x => x.InventoryNumber == inventoryNumber)
                .To<T>().FirstOrDefault();

            return sparePart;
        }

        public bool CheckSparePartQuantity(string sparePartId)
        {
            int quantity = this.sparePartRepository
                .All()
                .Where(x => x.Id == sparePartId)
                .Select(x => x.Quantity)
                .FirstOrDefault();

            return quantity > 0;
        }

        public async Task<string> UseSparePart(string sparePartId)
        {
            var sparePart = this.sparePartRepository
                .All()
                .Where(x => x.Id == sparePartId)
                .FirstOrDefault();

            if (this.CheckSparePartQuantity(sparePartId))
            {
                sparePart.Quantity = sparePart.Quantity - 1;
            }
            else
            {
                throw new ArgumentException("Not enough capacity!");
            }

            await this.sparePartRepository.SaveChangesAsync();

            return sparePart.Id;
        }

        public async Task<string> ReturnSparePart(string sparePartId)
        {
            var sparePart = this.sparePartRepository
                .All()
                .Where(x => x.Id == sparePartId)
                .FirstOrDefault();

            sparePart.Quantity = sparePart.Quantity + 1;
            await this.sparePartRepository.SaveChangesAsync();

            return sparePart.Id;
        }

        public async Task<string> IncreaseSparePartQuantity(string sparePartId, int quantity)
        {
            var sparePart = this.sparePartRepository
                .All()
                .Where(x => x.Id == sparePartId)
                .FirstOrDefault();

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }

            sparePart.Quantity = sparePart.Quantity + quantity;
            await this.sparePartRepository.SaveChangesAsync();

            return sparePart.Id;
        }

        public bool CheckAccess(ApplicationUser user, string partId)
        {
            var currentPart = this.sparePartRepository
                .All()
                .FirstOrDefault(x => x.Id == partId);

            return currentPart.UserId == user.Id;
        }

        public IEnumerable<T> GetAllWithDeleted<T>()
        {
            IQueryable<SparePart> query =
                this.sparePartRepository
                .AllWithDeleted();

            return query.To<T>().ToList();
        }

        public T GetByIdWithDeleted<T>(string id)
        {
            var user = this.sparePartRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public IEnumerable<T> GetAllForCompany<T>(string companyId, int? take = null, int skip = 0)
        {
            IQueryable<SparePart> query =
                this.sparePartRepository
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

        public async Task<string> UndeleteAsync(string id)
        {
            var currentPart = this.sparePartRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentPart == null)
            {
                throw new ArgumentNullException($"Spare part with {id} does not exist!");
            }

            this.sparePartRepository.Undelete(currentPart);
            await this.sparePartRepository.SaveChangesAsync();

            return currentPart.Id;
        }

        public IEnumerable<T> GetAll<T>(ApplicationUser user, int? take = null, int skip = 0)
        {
            IQueryable<SparePart> query = Enumerable.Empty<SparePart>().AsQueryable();

            query =
                this.sparePartRepository
                .All()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.Machine.User.CompanyId == user.CompanyId || (x.MachineId == null && x.User.CompanyId == user.CompanyId))
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public int GetCount(ApplicationUser user)
        {
            if (user != null)
            {
                return this.sparePartRepository.All()
                    .Count(x => x.Machine.User.CompanyId == user.CompanyId || (x.MachineId == null && x.User.CompanyId == user.CompanyId));
            }

            return 0;
        }

        public int GetCountForCompany(string companyId)
        {
            return this.sparePartRepository
            .All()
            .Count(x => x.User.CompanyId == companyId);
        }

        public int GetCountForCompanyWithDeleted(string companyId)
        {
            return this.sparePartRepository
            .AllWithDeleted()
            .Count(x => x.User.CompanyId == companyId);
        }
    }
}
