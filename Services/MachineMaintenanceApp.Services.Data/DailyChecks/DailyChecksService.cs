namespace MachineMaintenanceApp.Services.Data.DailyChecks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Services.Mapping;
    using MachineMaintenanceApp.Web.ViewModels.Machines;
    using Microsoft.AspNetCore.Identity;

    public class DailyChecksService : IDailyChecksService
    {
        private readonly IDeletableEntityRepository<DailyCheck> dailyCheckRepository;
        private readonly IMachinesService machineService;
        private readonly UserManager<ApplicationUser> userManager;

        public DailyChecksService(
            IDeletableEntityRepository<DailyCheck> dailyCheckRepository, IMachinesService machineService, UserManager<ApplicationUser> userManager)
        {
            this.dailyCheckRepository = dailyCheckRepository;
            this.machineService = machineService;
            this.userManager = userManager;
        }

        public async Task<string> CreateAsync(DailyCheckType type, string id, string notes, string userId)
        {
            var machine = this.machineService.GetById<MachineIdViewModel>(id);

            if (machine == null)
            {
                throw new ArgumentNullException($"Daily check with {id} does not exist!");
            }

            var dailyCheck = new DailyCheck
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Notes = notes,
                MachineId = machine.Id,
                UserId = userId,
            };

            await this.dailyCheckRepository.AddAsync(dailyCheck);
            await this.dailyCheckRepository.SaveChangesAsync();
            return dailyCheck.Id;
        }

        public async Task<string> EditAsync(DailyCheckType type, string notes, string id, string machineId, ApplicationUser user)
        {
            var currentDailyCheck = this.dailyCheckRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (currentDailyCheck == null)
            {
                throw new ArgumentNullException($"Daily check with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentDailyCheck.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            currentDailyCheck.Type = type;
            currentDailyCheck.Notes = notes;

            this.dailyCheckRepository.Update(currentDailyCheck);
            await this.dailyCheckRepository.SaveChangesAsync();
            return currentDailyCheck.MachineId;
        }

        public async Task<string> DeleteAsync(string id, ApplicationUser user)
        {
            var currentDailyCheck = this.dailyCheckRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (currentDailyCheck == null)
            {
                throw new ArgumentNullException($"Daily check with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentDailyCheck.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            string currentDailyCheckMachineId = currentDailyCheck.MachineId;

            this.dailyCheckRepository.Delete(currentDailyCheck);
            await this.dailyCheckRepository.SaveChangesAsync();
            return currentDailyCheckMachineId;
        }

        public IEnumerable<T> GetAll<T>(string id, int? take = null, int skip = 0)
        {
            IQueryable<DailyCheck> query =
                 this.dailyCheckRepository
                 .All()
                 .OrderByDescending(x => x.CreatedOn)
                 .Where(x => x.MachineId == id)
                 .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetById<T>(string id)
        {
            var dailyCheck = this.dailyCheckRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>()
                .FirstOrDefault();

            return dailyCheck;
        }

        public bool CheckAccess(ApplicationUser user, string checkId)
        {
            var currentCheck = this.dailyCheckRepository
                .All()
                .FirstOrDefault(x => x.Id == checkId);

            return currentCheck.UserId == user.Id;
        }

        public IEnumerable<T> GetAllWithDeleted<T>(string machineId, int? take = null, int skip = 0)
        {
            IQueryable<DailyCheck> query =
                this.dailyCheckRepository
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
            var user = this.dailyCheckRepository
                .AllWithDeleted()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public async Task UndeleteAsync(string id)
        {
            var currentCheck = this.dailyCheckRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentCheck == null)
            {
                throw new ArgumentNullException($"Daily check with {id} does not exist!");
            }

            this.dailyCheckRepository.Undelete(currentCheck);
            await this.dailyCheckRepository.SaveChangesAsync();
        }

        public int GetCount(string machineId)
        {
            return this.dailyCheckRepository
            .All()
            .Count(x => x.MachineId == machineId);
        }

        public int GetCountWithDeleted(string machineId)
        {
            return this.dailyCheckRepository
            .AllWithDeleted()
            .Count(x => x.MachineId == machineId);
        }
    }
}
