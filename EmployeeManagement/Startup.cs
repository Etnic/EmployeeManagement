using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Filter;
using EmployeeManagement.Filters;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement
{
    public class Startup
    {
        private IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContextPool<AppDbContext>(
            //    options => options.UseSqlServer(this.configuration.GetConnectionString("EmployeeDBConnection")));
            //for sql connection string
            //          "ConnectionStrings": {
            //              "EmployeeDBConnection": "server=localhost;database=EmployeeDB,Trusted_Connection=true"
            //},

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
            });

            //services.AddMvc(options => {
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireAuthenticatedUser()
            //        .Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));
            //}).AddXmlSerializerFormatters();

            //services.AddMvc(options =>
            //{
            //    options.Filters.Add(new AddHeaderAttribute("GlobalAddHeader",
            //        "Result filter added to MvcOptions.Filters"));         // An instance
            //    options.Filters.Add(typeof(ActionFilter));         // By type
            //   // options.Filters.Add(new ShortCircuitingResourceFilterAttribute());       // An instance
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

             services.AddMvc();

            services.AddEntityFrameworkNpgsql().AddDbContext<AppDbContext>(opt =>
            opt.UseNpgsql(this.configuration.GetConnectionString("MyWebApiConection")));

            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
            services.AddScoped<AddHeaderResultServiceFilter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
            });

        }
    }
}
