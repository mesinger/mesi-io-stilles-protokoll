using System.Data;
using Mesi.Io.SilentProtocol.Application;
using Mesi.Io.SilentProtocol.Domain;
using Mesi.Io.SilentProtocol.Infrastructure.Db;
using Mesi.Io.SilentProtocol.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

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
            services.Configure<DiscordOptions>(Configuration.GetSection("Discord"));

            services.AddTransient<IDbConnection>(db => new NpgsqlConnection(
                Configuration.GetConnectionString("SilentProtocolDb")));

            services.AddHttpClient();
            
            services.AddScoped<ISilentProtocolEntryRepository, DapperSilentProtocolEntryRepository>();
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