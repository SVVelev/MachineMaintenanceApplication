namespace MachineMaintenanceApp.Services.Data.WeeklyChecks
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
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;

    [Authorize]
    public class WeeklyChecksService : IWeeklyChecksService
    {
        private readonly IDeletableEntityRepository<WeeklyCheck> weeklyCheckRepository;
        private readonly IMachinesService machineService;
        private readonly UserManager<ApplicationUser> userManager;

        public WeeklyChecksService(
            IDeletableEntityRepository<WeeklyCheck> weeklyCheckRepository, IMachinesService machineService, UserManager<ApplicationUser> userManager)
        {
            this.weeklyCheckRepository = weeklyCheckRepository;
            this.machineService = machineService;
            this.userManager = userManager;
        }

        public async Task<string> CreateAsync(WeeklyCheckType type, string id, string notes, string userId)
        {
            var machine = this.machineService.GetById<MachineIdViewModel>(id);

            if (machine == null)
            {
                throw new ArgumentNullException($"Machine with {id} does not exist!");
            }

            var weeklyCheck = new WeeklyCheck
            {
                Id = Guid.NewGuid().ToString(),
                Type = type,
                Notes = notes,
                MachineId = machine.Id,
                UserId = userId,
            };

            await this.weeklyCheckRepository.AddAsync(weeklyCheck);
            await this.weeklyCheckRepository.SaveChangesAsync();
            return weeklyCheck.Id;
        }

        public async Task<string> DeleteAsync(string id, ApplicationUser user)
        {
            var currentWeeklyCheck = this.weeklyCheckRepository
                .All()
                .FirstOrDefault(x => x.Id == id);

            if (currentWeeklyCheck == null)
            {
                throw new ArgumentNullException($"Weekly check with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentWeeklyCheck.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            string currentWeeklyCheckMachineId = currentWeeklyCheck.MachineId;

            this.weeklyCheckRepository.Delete(currentWeeklyCheck);
            await this.weeklyCheckRepository.SaveChangesAsync();
            return currentWeeklyCheckMachineId;
        }

        public async Task<string> EditAsync(WeeklyCheckType type, string notes, string id, ApplicationUser user)
        {
            var currentWeeklyCheck = this.weeklyCheckRepository
                .All()
                .Where(x => x.Id == id)
                .FirstOrDefault();

            if (currentWeeklyCheck == null)
            {
                throw new ArgumentNullException($"Weekly check with {id} does not exist!");
            }

            if ((!await this.userManager.IsInRoleAsync(user, "Administrator")) && (currentWeeklyCheck.UserId != user.Id))
            {
                throw new UnauthorizedAccessException($"Access denied!");
            }

            currentWeeklyCheck.Type = type;
            currentWeeklyCheck.Notes = notes;

            this.weeklyCheckRepository.Update(currentWeeklyCheck);
            await this.weeklyCheckRepository.SaveChangesAsync();
            return currentWeeklyCheck.Id;
        }

        public IEnumerable<T> GetAll<T>(string id, int? take = null, int skip = 0)
        {
            IQueryable<WeeklyCheck> query =
                 this.weeklyCheckRepository
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
            var weeklyCheck = this.weeklyCheckRepository
                .All()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();

            return weeklyCheck;
        }

        public bool CheckAccess(ApplicationUser user, string checkId)
        {
            var currentCheck = this.weeklyCheckRepository
                .All()
                .FirstOrDefault(x => x.Id == checkId);

            return currentCheck.UserId == user.Id;
        }

        public IEnumerable<T> GetAllWithDeleted<T>(string id, int? take = null, int skip = 0)
        {
            IQueryable<WeeklyCheck> query =
                this.weeklyCheckRepository
                .AllWithDeleted()
                .OrderByDescending(x => x.CreatedOn)
                .Where(x => x.MachineId == id)
                .Skip(skip);

            if (take.HasValue)
            {
                query = query.Take(take.Value);
            }

            return query.To<T>().ToList();
        }

        public T GetByIdWithDeleted<T>(string id)
        {
            var user = this.weeklyCheckRepository
                .AllWithDeleted()
                .Where(x => x.Id == id)
                .To<T>().FirstOrDefault();
            return user;
        }

        public async Task<string> UndeleteAsync(string id)
        {
            var currentCheck = this.weeklyCheckRepository
                .AllWithDeleted()
                .FirstOrDefault(x => x.Id == id);

            if (currentCheck == null)
            {
                throw new ArgumentNullException($"Weekly check with {id} does not exist!");
            }

            this.weeklyCheckRepository.Undelete(currentCheck);
            await this.weeklyCheckRepository.SaveChangesAsync();

            return currentCheck.Id;
        }

        public int GetCount(string machineId)
        {
            return this.weeklyCheckRepository
            .All()
            .Count(x => x.MachineId == machineId);
        }

        public int GetCountWithDeleted(string machineId)
        {
            return this.weeklyCheckRepository
            .AllWithDeleted()
            .Count(x => x.MachineId == machineId);
        }
    }
}
