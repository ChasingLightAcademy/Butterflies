using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NLog;
using Progynova.DbModels;

namespace Progynova
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
            Log.Info("Loading database.");
            
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

            services.AddDbContext<ProgynovaContext>(option =>
            {
                option.UseLazyLoadingProxies();
                option.UseMySql(mysqlConnectionString, ServerVersion.AutoDetect(mysqlConnectionString));
            });
            
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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Progynova", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Info("C10s tql!");
            
            app.Use(next =>
            {
                return async context =>
                {
                    context.Response.OnStarting(() =>
                    {
                        context.Response.Headers[$"X-{Program.Name}-Author"] = Program.Author;
                        context.Response.Headers[$"X-{Program.Name}-Version"] = Program.Version.ToString();

                        return Task.CompletedTask;
                    });

                    Log.Info($"Got {context.Request.Protocol} {context.Request.Method} " +
                             $"from {context.Connection.RemoteIpAddress?.MapToIPv4()}:{context.Connection.RemotePort} " +
                             $"to {context.Request.Path} with ID {context.Connection.Id}.");

                    await next(context);
                };
            });
            
            if (env.IsDevelopment())
            {
                app.UseStaticFiles();
                app.UseDefaultFiles();
                
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Progynova v1"));
            }

            app.UseForwardedHeaders();

            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetService<ProgynovaContext>();
            context?.Database.EnsureCreated();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}