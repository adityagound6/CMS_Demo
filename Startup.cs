using CMS_Demo.Controllers;
using CMS_Demo.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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
                       .AddCookie(options => options.LoginPath = new PathString("/Admin/Login"));
            services.AddDbContextPool<AppDbContext>(option =>
                        option.UseSqlServer(_config.GetConnectionString("DBConnection")));
            //services.AddTransient<IClaimsTransformation, MyClaimsTransformation>();
            //services.AddDistributedMemoryCache();
            
            services.AddSession();
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
                /*var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
                options.Filters.Add(new AuthorizeFilter(policy));*/
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            //app.UseAuthorization();
            app.UseSession();
            app.UseMvcWithDefaultRoute();
           // app.UseCookiePolicy(cookiePolicyOptions);
            app.UseMvc(/*route => {
                route.MapRoute("default", "{controller=Home}/{action=index}/{id?}/{id1?}");

            }*/);
        }
    }
}
