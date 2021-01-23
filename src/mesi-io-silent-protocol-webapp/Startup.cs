using Mesi.Io.SilentProtocol.Application;
using Mesi.Io.SilentProtocol.Domain;
using Mesi.Io.SilentProtocol.Infrastructure.Db;
using Mesi.Io.SilentProtocol.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mesi.Io.SilentProtocol.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Index");
                options.Conventions.AuthorizePage("/New");
            });
            
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
                {
                    config.LoginPath = "/login";
                });

            services.Configure<SilentProtocolOptions>(Configuration.GetSection("SilentProtocol"));

            services.AddDbContext<SilentProtocolDbContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("SilentProtocolDb")));

            services.AddScoped<ISilentProtocolEntryRepository, SilentProtocolEntryRepository>();
            services.AddScoped<ISilentProtocolEntryFactory, SilentProtocolEntryFactory>();

            services.AddScoped<IGetSilentProtocolEntriesPaged, SilentProtocolApplicationService>();
            services.AddScoped<IAddSilentProtocolEntry, SilentProtocolApplicationService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}