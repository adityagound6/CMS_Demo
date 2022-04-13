using CMS_Demo.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS_Demo
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration _config)
        {
            this._config = _config;
        }
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(x => x.LoginPath = "/Login");
                    
            services.AddAuthorization();
            services.AddDbContextPool<AppDbContext>(option =>
                    option.UseSqlServer(_config.GetConnectionString("DBConnection")));
            services.AddDistributedMemoryCache();
            services.AddSession();
            /*services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Admin/Login";
            });*/
            services.AddMvc(options => options.EnableEndpointRouting = false);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRouting();
           // app.UseCookiePolicy(cookiePolicyOptions);
            app.UseMvc(route => {
                route.MapRoute("default", "{controller=Home}/{action=index}/{id?}");

            });
        }
    }
}
