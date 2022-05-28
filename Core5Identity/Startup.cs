using Core5Identity.Infrastructers;
using Core5Identity.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core5Identity
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
            services.AddRazorPages();
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationIdentityDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //varsayýlan login sayfasýný deðiþtirebiliriz
            //services.ConfigureApplicationCookie(opt => opt.LoginPath = "/Home/Login");

            //custom password validate için
            services.AddTransient<IPasswordValidator<ApplicationUser>, CustomPasswordValidater>();

            //custom userName,email validate
            services.AddTransient<IUserValidator<ApplicationUser>, CustomUserValidater>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options => 
            {

                options.User.AllowedUserNameCharacters = "abcdefgklmn"; //username de istediðimiz karakterleri belirleyebirlirz
                options.User.RequireUniqueEmail = true; //tek bir email hakký 

                options.Password.RequiredLength = 6;    //minumum kac akrakter olsun
                options.Password.RequireLowercase = false;  //Küçük harf zorunluluðu
                options.Password.RequireUppercase=false;    //Büyük Harf zorunluluðu
                options.Password.RequireNonAlphanumeric = false;    //AlphaNumeric zorunluluðu olmasýn
                options.Password.RequireDigit = false;  //en az 1 rakam olsun mu olmasýn mo
            })
            .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
            .AddDefaultTokenProviders();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStatusCodePages();

            app.UseRouting();

            //Bu ikisinde sýra önemli
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"

                    );

            });
        }
    }
}
