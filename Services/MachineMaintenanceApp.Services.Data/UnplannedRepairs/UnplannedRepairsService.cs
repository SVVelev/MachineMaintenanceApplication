namespace MachineMaintenanceApp.Services.Data.UnplannedRepairs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Services.Data.SpareParts;
    using MachineMaintenanceApp.Services.Mapping;
    using MachineMaintenanceApp.Web.ViewModels.Machines;
    using MachineMaintenanceApp.Web.ViewModels.SpareParts;
    using Microsoft.AspNetCore.Identity;

    public class UnplannedRepairsService : IUnplannedRepairsService
    {
        private readonly IDeletableEntityRepository<UnplannedRepair> unplannedRepairRepository;
        private readonly IMachinesService machineService;
        private readonly ISparePartsService sparePartsService;
        private readonly UserManager<ApplicationUser> userManager;

        public UnplannedRepairsService(
            IDeletableEntityRepository<UnplannedRepair> unplannedRepairRepository,
            IMachinesService machineService,
            ISparePartsService sparePartsService,
            UserManager<ApplicationUser> userManager)
        {
            this.unplannedRepairRepository = unplannedRepairRepository;
            this.machineService = machineService;
            this.sparePartsService = sparePartsService;
            this.userManager = userManager;
        }

        public async Task<string> CreateAsync(RepairType type, DateTime startTime, DateTime endTime, string description, string machineId, string userId, string sparePartInventoryNumber)
        {
            var machine = this.machineService.GetById<MachineIdViewModel>(machineId);

            if (machine == null)
            {
                throw new ArgumentNullException($"Machine with {machineId} does not exist!");
            }

            var sparePart = this.sparePartsService.GetByInventoryNumber<SparePartIdViewModel>(sparePartInventoryNumber);

            var unplannedRepair = new UnplannedRepair
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Description = description,
                StartTime = startTime,
                EndTime = endTime,
                MachineId = machine.Id,
                UserId = userId,
            };

            if (!string.IsNullOrEmpty(sparePartInventoryNumber))
            {
                unplannedRepair.SparePartId = sparePart.Id;
                unplannedRepair.PartNumber = sparePart.InventoryNumber;
                await this.sparePartsService.UseSparePart(sparePart.Id);
            }

            await this.unplannedRepairRepository.AddAsync(unplannedRepair);
            await this.unplannedRepairRepository.SaveChangesAsync();
            return unplannedRepair.Id;
        }

        public async Task<string> EditAsync(RepairType type, DateTime startTime, DateTime endTime, string description, string sparePartInventoryNumber, string id, ApplicationUser user)
        {
            var currentRepair = this.unplannedRepairRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (currentRepair == null)
            {
                throw new ArgumentNullException($"Repair with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentRepair.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            var sparePart = this.sparePartsService.GetByInventoryNumber<SparePartIdViewModel>(sparePartInventoryNumber);

            currentRepair.Type = type;
            currentRepair.StartTime = startTime;
            currentRepair.EndTime = endTime;
            currentRepair.Description = description;

            if (!string.IsNullOrEmpty(sparePartInventoryNumber))
            {
                if (currentRepair.SparePartId != null && currentRepair.SparePartId != sparePart.Id)
                {
                    await this.sparePartsService.ReturnSparePart(currentRepair.SparePartId);
                    await this.sparePartsService.UseSparePart(sparePart.Id);

                    currentRepair.SparePartId = sparePart.Id;
                    currentRepair.PartNumber = sparePart.InventoryNumber;
                }
                else if (currentRepair.SparePartId != null && currentRepair.SparePartId == sparePart.Id)
                {
                    currentRepair.SparePartId = sparePart.Id;
                    currentRepair.PartNumber = sparePart.InventoryNumber;
                }
                else
                {
                    await this.sparePartsService.UseSparePart(sparePart.Id);
                    currentRepair.SparePartId = sparePart.Id;
                    currentRepair.PartNumber = sparePart.InventoryNumber;
                }
            }
            else
            {
                if (currentRepair.SparePartId != null)
                {
                    await this.sparePartsService.ReturnSparePart(currentRepair.SparePartId);
                }

                currentRepair.SparePartId = null;
                currentRepair.PartNumber = null;
            }

            this.unplannedRepairRepository.Update(currentRepair);
            await this.unplannedRepairRepository.SaveChangesAsync();
            return currentRepair.Id;
        }

        public async Task<string> DeleteAsync(string id, ApplicationUser user)
        {
            var currentRepair = this.unplannedRepairRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (currentRepair == null)
            {
                throw new ArgumentNullException($"Repair with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentRepair.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            string currentRepairMachineId = currentRepair.MachineId;

            this.unplannedRepairRepository.Delete(currentRepair);
            await this.unplannedRepairRepository.SaveChangesAsync();
            return currentRepairMachineId;
        }

        public IEnumerable<T> GetAll<T>(string machineId, int? take = null, int skip = 0)
        {
            IQueryable<UnplannedRepair> query =
                 this.unplannedRepairRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn)
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
            var plannedRepair = this.unplannedRepairRepository
                .All()
                .Where(x => x.Id == id)
               .To<T>().FirstOrDefault();

            return plannedRepair;
        }

        public bool CheckAccess(ApplicationUser user, string repairId)
        {
            var currentRepair = this.unplannedRepairRepository
                .All()
                .FirstOrDefault(x => x.Id == repairId);

            return currentRepair.UserId == user.Id;
        }

        public IEnumerable<T> GetAllWithDeleted<T>(string machineId, int? take = null, int skip = 0)
        {
            IQueryable<UnplannedRepair> query =
                this.unplannedRepairRepository
                .AllWithDeleted()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.MachineId == machineId)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetByIdWithDeleted<T>(string id)
        {
            var user = this.unplannedRepairRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public async Task<string> UndeleteAsync(string id)
        {
            var currentRepair = this.unplannedRepairRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentRepair == null)
            {
                throw new ArgumentNullException($"Repair with {id} does not exist!");
            }

            this.unplannedRepairRepository.Undelete(currentRepair);
            await this.unplannedRepairRepository.SaveChangesAsync();

            return currentRepair.Id;
        }

        public int GetCount(string machineId)
        {
                return this.unplannedRepairRepository
                    .All()
                    .Count(x => x.MachineId == machineId);
        }

        public int GetCountWithDeleted(string machineId)
        {
            return this.unplannedRepairRepository
                .AllWithDeleted()
                .Count(x => x.MachineId == machineId);
        }
    }
}
