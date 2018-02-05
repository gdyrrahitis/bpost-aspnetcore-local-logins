﻿namespace Authentication.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
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
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            var appSetting = Configuration.GetSection("Settings").Get<AppSettings>();
            var settings = new DatabaseCommanderSettings(Namespaces(appSetting), Connections(appSetting));
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
                });
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

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
