using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.ReCaptcha;
using Butterflies.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;

namespace Butterflies
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public ILogger Log { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Log = LogManager.GetCurrentClassLogger();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Info("Choosing raw materials.");
            
            if (string.IsNullOrWhiteSpace(Configuration["Database:Host"]) ||
                string.IsNullOrWhiteSpace(Configuration["Database:Port"]) || 
                string.IsNullOrWhiteSpace(Configuration["Database:User"]) || 
                string.IsNullOrWhiteSpace(Configuration["Database:Pass"]) || 
                string.IsNullOrWhiteSpace(Configuration["Database:Name"]))
            {
                throw new InvalidDataException($"Cannot find mysql database settings, please check your appsettings.json file.");
            }
            
            var mysqlConnectionString = $"Server={Configuration["Database:Host"]};" +
                                        $"Port={Configuration["Database:Port"]};" + 
                                        $"Uid={Configuration["Database:User"]};" + 
                                        $"Pwd={Configuration["Database:Pass"]};" + 
                                        $"DataBase={Configuration["Database:Name"]};";
            services.AddDbContext<ButterfliesContext>(option =>
            {
                option.UseLazyLoadingProxies();
                option.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString));
            });
            
            Log.Info("Compositing.");
            services.AddReCaptcha(settings =>
            {
                settings.SiteKey = Configuration["Security:Recaptcha:SiteKey"];
                settings.SecretKey = Configuration["Security:Recaptcha:SecretKey"];
                settings.UseRecaptchaNet = true;
                settings.Version = ReCaptchaVersion.V3;
            });
            
            Log.Info("Diluting.");
            
            services.AddControllers()
                .AddNewtonsoftJson(json =>
                {
                    json.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    json.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);
            
            Log.Info("Tabletting.");

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}