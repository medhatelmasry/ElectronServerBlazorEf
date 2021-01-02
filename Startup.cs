using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;
using ElectronServerBlazorEf.Data;
using ElectronServerBlazorEf.NW;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ElectronServerBlazorEf {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices (IServiceCollection services) {
            services.AddRazorPages ();
            services.AddServerSideBlazor ();
            services.AddSingleton<WeatherForecastService> ();
            // Scoped creates an instance for each user
            services.AddScoped<NorthwindService> ();

            services.AddDbContext<NorthwindContext> (options => {
                options.UseSqlServer (Configuration.GetConnectionString ("NW"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseExceptionHandler ("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseHttpsRedirection ();
            app.UseStaticFiles ();

            app.UseRouting ();

            app.UseEndpoints (endpoints => {
                endpoints.MapBlazorHub ();
                endpoints.MapFallbackToPage ("/_Host");
            });

            if (HybridSupport.IsElectronActive) {
                ElectronBootstrap ();
            }
        }

        public async void ElectronBootstrap () {
            var browserWindow = await Electron.WindowManager.CreateWindowAsync (new BrowserWindowOptions {
                Width = 1152,
                    Height = 940,
                    Show = false
            });

            await browserWindow.WebContents.Session.ClearCacheAsync ();

            browserWindow.OnReadyToShow += () => browserWindow.Show ();
            browserWindow.SetTitle ("Electron.NET with Blazor!");
        }
    }
}