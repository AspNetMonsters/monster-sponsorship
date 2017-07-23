using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MonsterSponsorship.Models;

namespace MonsterSponsorship
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            if(env.IsDevelopment())
                builder.AddUserSecrets();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddOptions();
            services.Configure<StripeOptions>(Configuration.GetSection("Stripe"));
            services.Configure<BusinessOptions>(Configuration.GetSection("Business"));

            services.Configure<UserGroupOptions>(Configuration.GetSection("UserGroups"));

            var groups = new UserGroupOptions();

            for (int i = 0; i < 20; i++)
            {
                var name = Configuration.GetValue<string>($"UserGroups:{i}:GroupName");
                var code = Configuration.GetValue<string>($"UserGroups:{i}:GroupCode");
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(code))
                    groups.UserGroups.Add(new UserGroup { GroupName = name, GroupCode = code });
            }

            services.AddSingleton<UserGroupOptions>(groups);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Sponsorship}/{action=Index}/{id?}");
            });
        }
    }
}
