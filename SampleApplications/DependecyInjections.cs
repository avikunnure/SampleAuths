using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SampleApplications.Database;

namespace SampleApplications
{
    public static class DependecyInjections 
    {
        public static IServiceCollection AddRequiredServices(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<SampleApplicationDBContext>(options =>
               options.UseSqlServer(configuration.GetConnectionString("Default"),b => b.MigrationsAssembly("SampleApplications")));

            services.AddIdentityCore<ApplicationUser>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<SampleApplicationDBContext>();
            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                
            }).AddCookie();
            services.AddSession();
            return services;
        }
    }
}
