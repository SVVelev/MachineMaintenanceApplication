namespace MachineMaintenanceApp.Web
{
    using System.Reflection;
    using CloudinaryDotNet;
    using MachineMaintenanceApp.Data;
    using MachineMaintenanceApp.Data.Common;
    using MachineMaintenanceApp.Data.Common.Repositories;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Repositories;
    using MachineMaintenanceApp.Data.Seeding;
    using MachineMaintenanceApp.Services.Data;
    using MachineMaintenanceApp.Services.Data.Comapnies;
    using MachineMaintenanceApp.Services.Data.DailyChecks;
    using MachineMaintenanceApp.Services.Data.Machines;
    using MachineMaintenanceApp.Services.Data.PlannedRepairs;
    using MachineMaintenanceApp.Services.Data.SpareParts;
    using MachineMaintenanceApp.Services.Data.UnplannedRepairs;
    using MachineMaintenanceApp.Services.Data.Users;
    using MachineMaintenanceApp.Services.Data.WeeklyChecks;
    using MachineMaintenanceApp.Services.Mapping;
    using MachineMaintenanceApp.Services.Messaging;
    using MachineMaintenanceApp.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(this.configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddMvc(options => options.
            Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

            services.Configure<CookiePolicyOptions>(
                options =>
                    {
                        options.CheckConsentNeeded = context => true;
                        options.MinimumSameSitePolicy = SameSiteMode.None;
                    });

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddSingleton(this.configuration);

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<ISettingsService, SettingsService>();
            services.AddTransient<IMachinesService, MachineService>();
            services.AddTransient<ISparePartsService, SparePartService>();
            services.AddTransient<IDailyChecksService, DailyChecksService>();
            services.AddTransient<IWeeklyChecksService, WeeklyChecksService>();
            services.AddTransient<IPlannedRepairsService, PlannedRepairsService>();
            services.AddTransient<IUnplannedRepairsService, UnplannedRepairsService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ICompanyService, CompanyService>();

            Account account = new Account(
                    this.configuration["Cloudinary:CloudName"],
                    this.configuration["Cloudinary:ApiKey"],
                    this.configuration["Cloudinary:ApiSecret"]);

            Cloudinary cloudinary = new Cloudinary(account);

            services.AddSingleton(cloudinary);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            // Seed data on application startup
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                if (env.IsDevelopment())
                {
                    dbContext.Database.Migrate();
                }

                new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // app.UseStatusCodePagesWithRedirects("/Home/HttpError?statusCode={0}");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                    {
                        endpoints.MapControllerRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                        endpoints.MapRazorPages();
                    });
        }
    }
}
