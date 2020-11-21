using BatchDataAccessLibrary.FileReader;
using BatchDataAccessLibrary.Helpers;
using BatchDataAccessLibrary.Interfaces;
using BatchDataAccessLibrary.Repositories;
using BatchDataAccessLibrary.Repositories.PcsCompliance.DemoMocks;
using BatchDataAccessLibrary.Repositories.ShiftLog;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RosemountDiagnosticsV2.Hubs;
using RosemountDiagnosticsV2.Interfaces;
using RosemountDiagnosticsV2.Models;
using RosemountDiagnosticsV2.Models.ApplicationData;

namespace RosemountDiagnosticsV2
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<BatchContext>(options =>
            //{
            //    options.UseSqlServer(Configuration.GetConnectionString("BatchData"));
            //});
            //services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            //    {
            //        option.Password.RequiredLength = 6;
            //        option.Password.RequiredUniqueChars = 3;
            //    })
            //    .AddEntityFrameworkStores<BatchContext>();




            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // *********************************************************************

            // THIS REPOSITORY SECTION IS TO SWITCH BETWEEN REAL PRODUCTION MODE AND DEMO MODE
            // I HAVE REMOVED THE REAL REPOSITORIES FOR COMPANY SENSITIVITY REASONS
            // I HAVE LEFT THE FUNCTIONS HERE TO SHOW HOW I SWITCHED BETWEEN MODES

            //RegisterMockRepositories(services);  
            RegisterDemoRepositories(services);
            //RegisterRepositories(services);

            // **********************************************************************

            services.AddMvc(options =>
            {
                //var policy = new AuthorizationPolicyBuilder()
                //.RequireAuthenticatedUser()
                //.Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR();
        }

        //private void RegisterRepositories(IServiceCollection services)
        //{
        //    services.AddScoped<IBatchDataFileManager, BatchDataFileManager>();
        //    services.AddScoped<IBatchRepository, BatchReportRepository>();
        //    services.AddScoped<IMaterialDetailsRepository, MaterialDetailsRepository>();
        //    services.AddScoped<IGapInTimeReasons, GapInTimeReasonsRepository>();
        //    services.AddScoped<IRecipeLimitRepository, RecipeLimitRepository>();
        //    services.AddScoped<IDateSelector, DateSelectorModal>();
        //    services.AddSingleton<IApplicationData, ApplicationData>();
        //    services.AddScoped<IApiBatchRepository, ApiBatchRepository>();
        //    services.AddScoped<IPcsWeightParameterRepository, PcsParametersRepository>();
        //    services.AddScoped<IPcsReworkParameters, PcsReworkRepository>();
        //    services.AddScoped<IPcsActiveTempParameters, PcsActiveTempParametersRepository>();
        //    services.AddScoped<IPcsScoringRepository, PcsScoringRepository>();
        //    services.AddScoped<IPcsToleranceParameterRepository, PcsToleranceParameterRepository>();
        //    services.AddScoped<IShiftLogRepository, ShiftLogRepository>();
        //    services.AddScoped<IHelperMethods, BatchHelperMethods>();
        //}

        //private void RegisterMockRepositories(IServiceCollection services)
        //{
        //    services.AddScoped<IBatchDataFileManager, BatchDataFileManager>();
        //    services.AddSingleton<IBatchRepository, MockBatchReportRepository>();
        //    services.AddSingleton<IMaterialDetailsRepository, MockMaterialDetailsRepository>();
        //    services.AddSingleton<IGapInTimeReasons, MockGapInTimeReasons>();
        //    services.AddSingleton<IRecipeLimitRepository, MockRecipeLimitRepository>();
        //    services.AddScoped<IDateSelector, DateSelectorModal>();
        //    services.AddSingleton<IApiBatchRepository, ApiBatchRepository>();
        //    services.AddSingleton<IApplicationData, ApplicationDataMock>();
        //    services.AddSingleton<IPcsWeightParameterRepository, MockPcsParametersRepository>();
        //    services.AddSingleton<IPcsReworkParameters, MockPcsReworkRepository>();
        //    services.AddSingleton<IPcsActiveTempParameters, MockPcsActiveTempParametersRepository>();
        //    services.AddSingleton<IPcsScoringRepository, MockPcsScoringRepository>();
        //    services.AddSingleton<IPcsToleranceParameterRepository, MockToleranceParameterRepository>();
        //    services.AddScoped<IShiftLogRepository, ShiftLogRepository>();
        //    services.AddScoped<IHelperMethods, BatchHelperMethods>();
        //}

        private void RegisterDemoRepositories(IServiceCollection services)
        {
            services.AddSingleton<IBatchDataFileManager, BatchFileDataManagerForDemo>();
            services.AddSingleton<IBatchRepository, DemoMockBatchReportRepository>();
            services.AddSingleton<IMaterialDetailsRepository, DemoMockMaterialDetailsRepository>();
            services.AddSingleton<IGapInTimeReasons, MockGapInTimeReasons>();
            services.AddSingleton<IRecipeLimitRepository, MockRecipeLimitRepository>();
            services.AddScoped<IDateSelector, DateSelectorModal>();
            services.AddSingleton<IApiBatchRepository, ApiBatchRepository>();
            services.AddSingleton<IApplicationData, ApplicationDataDemo>();
            services.AddSingleton<IPcsWeightParameterRepository, DemoMockPcsParameterRepository>();
            services.AddSingleton<IPcsReworkParameters, DemoMockPcsReworkRepository>();
            services.AddSingleton<IPcsActiveTempParameters, DemoMockActiveTempsRepository>();
            services.AddSingleton<IPcsScoringRepository, MockPcsScoringRepository>();
            services.AddSingleton<IPcsToleranceParameterRepository, MockToleranceParameterRepository>();
            services.AddScoped<IShiftLogRepository, ShiftLogRepository>();
            services.AddScoped<IHelperMethods, DemoBatchHelperMethods>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSignalR(route =>
            {
                route.MapHub<BatchCompletedHub>("/batchhub");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
