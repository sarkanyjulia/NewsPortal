using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NewsPortal.Persistence;
using NewsPortal.WebSite.Models;

namespace NewsPortal.WebSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            DbType dbType = Configuration.GetSection("CustomSettings").GetValue<DbType>("DbType");

            // Adatbázis kontextus függőségi befecskendezése
            switch (dbType)
            {
                case DbType.SqlServer:
                    services.AddDbContext<NewsPortalContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
                    break;
                case DbType.Sqlite:
                    services.AddDbContext<NewsPortalContext>(options =>
                        options.UseSqlite(Configuration.GetConnectionString("SqliteConnection")));
                    break;
            }

            services.AddIdentity<User, IdentityRole<int>>()
    .AddEntityFrameworkStores<NewsPortalContext>()
    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Jelszó komplexitására vonatkozó konfiguráció
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;


                // Hibás bejelentkezés esetén az (ideiglenes) kizárásra vonatkozó konfiguráció
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // Felhasználókezelésre vonatkozó konfiguráció
                //options.User.RequireUniqueEmail = true;
            });

            services.AddTransient<INewsPortalService, NewsPortalService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseBrowserLink();
            }
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            

            var dbContext = serviceProvider.GetRequiredService<NewsPortalContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            DbInitializer.Initialize(app.ApplicationServices.GetRequiredService<NewsPortalContext>(), userManager, roleManager, Configuration.GetValue<string>("ImageStore"));
        }
    }
}
