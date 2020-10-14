namespace MachineMaintenanceApp.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Data.Models;

    public class CompanySeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Company.Any())
            {
                return;
            }

            var company1 = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = "AdminCompany",
                Logo = "https://cmkt-image-prd.freetls.fastly.net/0.1.0/ps/6713806/600/400/m2/fpnw/wm0/logos-21-77-.jpg?1563702063&s=3cbd11d46ab37b14314625d8db7d95bd",
            };

            var company2 = new Company
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Test",
                Logo = "https://dynamic.brandcrowd.com/asset/logo/f3500bdc-c0cb-448e-9e91-d380773b37b6/logo?v=4",
            };

            await dbContext.Company.AddAsync(company1);
            await dbContext.Company.AddAsync(company2);
        }
    }
}
