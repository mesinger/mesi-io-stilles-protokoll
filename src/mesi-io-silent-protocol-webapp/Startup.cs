using System.Data;
using System.Net.Http;
using Mesi.Io.SilentProtocol.Application;
using Mesi.Io.SilentProtocol.Domain;
using Mesi.Io.SilentProtocol.Infrastructure.Db;
using Mesi.Io.SilentProtocol.Options;
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
            services.AddControllers();
            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Index");
                options.Conventions.AuthorizePage("/New");
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = Configuration.GetValue<string>("Oidc:Authority");
            
                    options.ClientId = Configuration.GetValue<string>("Oidc:ClientId");
                    options.ClientSecret = Configuration.GetValue<string>("Oidc:Secret");
                    options.ResponseType = "code";
            
                    options.SaveTokens = true;
            
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.GetClaimsFromUserInfoEndpoint = true;
                    
                    var handler = new HttpClientHandler();
                    handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                    options.BackchannelHttpHandler = handler;
                });

            services.Configure<SilentProtocolOptions>(Configuration.GetSection("SilentProtocol"));
            services.Configure<DiscordOptions>(Configuration.GetSection("Discord"));

            services.AddTransient<IDbConnection>(db => new NpgsqlConnection(
                Configuration.GetConnectionString("SilentProtocolDb")));

            services.AddHttpClient();
            services.AddHttpContextAccessor();
            
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
                app.UseHttpsRedirection();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}