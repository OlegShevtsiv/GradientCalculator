using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GradientCalculator.Attributes.Validation;
using GradientCalculator.Configs;
using GradientCalculator.Data.Sqlite;
using GradientCalculator.Middlewares;
using GradientCalculator.Middlewares.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Rezervist.Middlewares;

namespace GradientCalculator
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
            services.AddScoped<LanguageActionFilter>();

            services.AddSession(s => 
            {
                s.IdleTimeout = TimeSpan.FromHours(1);
            });

            services.AddSingleton<IValidationAttributeAdapterProvider, CustomValidationAttributeAdapterProvider>();

            services.AddDbContext<SqliteContext>(options =>
                options.UseSqlite($"Filename={SqliteContext.SliteFileName}"));

            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(co.DefaultLang_UA),
                    new CultureInfo(co.Lang_EN)
                };
                options.DefaultRequestCulture = new RequestCulture(co.DefaultLang_UA);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.AddControllersWithViews()
                .AddDataAnnotationsLocalization(options => {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                        factory.Create(typeof(CommonResource));
                })
                .AddViewLocalization();
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
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseCulture();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{culture=ua}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "CatchAll", 
                    pattern: "{*url}",
                    defaults: new { controller = "Home", action = "NotFoundPage" });
            });
        }
    }
}
