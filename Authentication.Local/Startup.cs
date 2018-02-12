namespace Authentication.Local
{
    using System.Collections.Generic;
    using System.Linq;
    using Events;
    using Infrastructure.Logging;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;
    using Services;
    using Syrx;
    using Syrx.Commanders.Databases;
    using Syrx.Connectors.Databases;
    using Syrx.Connectors.Databases.SqlServer;
    using Syrx.Readers.Databases;
    using Syrx.Settings.Databases;

    public class Startup
    {
        private static readonly List<string> PermittedCategories = new List<string>
        {
            typeof(CookieEvents).FullName
        };

        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var settings = GetDatabaseCommanderSettings();
            services.AddSingleton<IDatabaseCommanderSettings>(settings);
            services.AddTransient<IDatabaseConnector, SqlServerDatabaseConnector>();
            services.AddTransient<IDatabaseCommandReader, DatabaseCommandReader>();
            services.AddTransient(typeof(ICommander<>), typeof(DatabaseCommander<>));

            services.AddSingleton(Configuration);
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserClaimsRepository, UserClaimsRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserClaimsService, UserClaimsService>();
            services.AddTransient<IClaimsTransformation, ProfileClaimsTransformationService>();
            services.AddTransient<CookieEvents>();
            services.AddMvc();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login";
                    options.LogoutPath = "/auth/logout";
                    options.EventsType = typeof(CookieEvents);
                });
        }

        private DatabaseCommanderSettings GetDatabaseCommanderSettings()
        {
            var appSetting = Configuration.GetSection("Settings").Get<AppSettings>();
            return new DatabaseCommanderSettings(Namespaces(appSetting), Connections(appSetting));
        }

        private static IEnumerable<ConnectionStringSetting> Connections(AppSettings appSetting)
        {
            return appSetting.Connections.Select(connection =>
                new ConnectionStringSetting(connection.Alias, connection.ConnectionString));
        }

        private static IEnumerable<DatabaseCommandNamespaceSetting> Namespaces(AppSettings appSetting)
        {
            return appSetting.Namespaces.Select(namespaceSetting =>
                new DatabaseCommandNamespaceSetting(
                    namespaceSetting.Namespace,
                    namespaceSetting.Types.Select(type =>
                        new DatabaseCommandTypeSetting(type.Name,
                            type.Commands.ToDictionary(k => k.Key,
                                v => new DatabaseCommandSetting(v.Value.ConnectionAlias,
                                    v.Value.CommandText,
                                    v.Value.CommandType,
                                    v.Value.CommandTimeout,
                                    v.Value.Split,
                                    v.Value.Flags,
                                    v.Value.IsolationLevel))))));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            var commander = app.ApplicationServices.GetService<ICommander<DatabaseLogger>>();
            loggerFactory.AddProvider(new DatabaseLoggerProvider(commander, category => 
                PermittedCategories.Contains(category)));

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
