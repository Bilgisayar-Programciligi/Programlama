using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using ETicaret.Data;
using Microsoft.AspNetCore.Identity;
using ETicaret.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Eticaret.Data;

namespace ETicaret
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
            services.AddControllersWithViews();

            services.AddDbContext<ETicaretContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("ETicaretContext")));

            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ETicaretContext>().AddDefaultTokenProviders();
            // services.Configure<IdentityOptions>(options =>
            // {
            //     // Default Password settings.
            //     options.Password.RequireDigit = false;
            //     options.Password.RequireLowercase = false;
            //     options.Password.RequireNonAlphanumeric = false;
            //     options.Password.RequireUppercase = false;
            //     options.Password.RequiredLength = 3;
            //     options.Password.RequiredUniqueChars = 0;
            // });

            services.AddSingleton<IEmailSender, EmailSender>(); //Uygulamanın başından sonuna kadar 
            //services.AddScoped  request boyunca
            //services.AddTransient nesneyi her istediğimizde

            services.AddControllersWithViews();
            services.AddRazorPages();
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

            System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("tr-TR");
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = customCulture;
            System.Globalization.CultureInfo.DefaultThreadCurrentUICulture = customCulture;

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
