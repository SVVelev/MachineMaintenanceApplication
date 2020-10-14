namespace MachineMaintenanceApp.Services.Data.PlannedRepairs
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

    public class PlannedRepairsService : IPlannedRepairsService
    {
        private readonly IDeletableEntityRepository<PlannedRepair> plannedRepairRepository;
        private readonly IMachinesService machineService;
        private readonly ISparePartsService sparePartsService;
        private readonly UserManager<ApplicationUser> userManager;

        public PlannedRepairsService(
            IDeletableEntityRepository<PlannedRepair> plannedRepairRepository,
            IMachinesService machineService,
            ISparePartsService sparePartsService,
            UserManager<ApplicationUser> userManager)
        {
            this.plannedRepairRepository = plannedRepairRepository;
            this.machineService = machineService;
            this.sparePartsService = sparePartsService;
            this.userManager = userManager;
        }

        public async Task<string> CreateAsync(RepairType type, DateTime startTime, DateTime endTime, string description, double repairInterval, string machineId, string userId, string sparePartInventoryNumber)
        {
            var machine = this.machineService.GetById<MachineIdViewModel>(machineId);

            if (machine == null)
            {
                throw new ArgumentNullException($"Machine with {machineId} does not exist!");
            }

            var sparePart = this.sparePartsService.GetByInventoryNumber<SparePartIdViewModel>(sparePartInventoryNumber);

            var plannedRepair = new PlannedRepair
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Description = description,
                StartTime = startTime,
                EndTime = endTime,
                RepairsIntervalDays = repairInterval,
                MachineId = machine.Id,
                UserId = userId,
            };

            if (!string.IsNullOrEmpty(sparePartInventoryNumber))
            {
                plannedRepair.SparePartId = sparePart.Id;
                plannedRepair.PartNumber = sparePart.InventoryNumber;

                await this.sparePartsService.UseSparePart(sparePart.Id);
            }

            await this.plannedRepairRepository.AddAsync(plannedRepair);
            await this.plannedRepairRepository.SaveChangesAsync();
            return plannedRepair.Id;
        }

        public async Task<string> EditAsync(RepairType type, DateTime startTime, DateTime endTime, string description, double repairInterval, string sparePartInventoryNumber, string id, ApplicationUser user)
        {
            var currentRepair = this.plannedRepairRepository
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
            currentRepair.RepairsIntervalDays = repairInterval;

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

            this.plannedRepairRepository.Update(currentRepair);
            await this.plannedRepairRepository.SaveChangesAsync();
            return currentRepair.Id;
        }

        public async Task<string> DeleteAsync(string id, ApplicationUser user)
        {
            var currentRepair = this.plannedRepairRepository
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

            this.plannedRepairRepository.Delete(currentRepair);
            await this.plannedRepairRepository.SaveChangesAsync();
            return currentRepairMachineId;
        }

        public IEnumerable<T> GetAll<T>(string machineId, int? take = null, int skip = 0)
        {
            IQueryable<PlannedRepair> query =
                 this.plannedRepairRepository
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
            var plannedRepair = this.plannedRepairRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return plannedRepair;
        }

        public bool CheckAccess(ApplicationUser user, string checkId)
        {
            var currentRepair = this.plannedRepairRepository
                .All()
                .FirstOrDefault(x => x.Id == checkId);

            return currentRepair.UserId == user.Id;
        }

        public IEnumerable<T> GetAllWithDeleted<T>(string machineId, int? take = null, int skip = 0)
        {
            IQueryable<PlannedRepair> query =
                this.plannedRepairRepository
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
            var user = this.plannedRepairRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return user;
        }

        public async Task<string> UndeleteAsync(string id)
        {
            var currentRepair = this.plannedRepairRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentRepair == null)
            {
                throw new ArgumentNullException($"Repair with {id} does not exist!");
            }

            this.plannedRepairRepository.Undelete(currentRepair);
            await this.plannedRepairRepository.SaveChangesAsync();

            return currentRepair.Id;
        }

        public int GetCount(string machineId)
        {
                return this.plannedRepairRepository
                .All()
                .Count(x => x.MachineId == machineId);
        }

        public int GetCountWithDeleted(string machineId)
        {
            return this.plannedRepairRepository
            .AllWithDeleted()
            .Count(x => x.MachineId == machineId);
        }
    }
}