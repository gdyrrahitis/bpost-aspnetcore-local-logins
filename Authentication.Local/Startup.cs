namespace Authentication.Local
{
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Constants;
    using Infrastructure.Security;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
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
            services.AddScoped<IDatabaseConnector, SqlServerDatabaseConnector>();
            services.AddScoped<IDatabaseCommandReader, DatabaseCommandReader>();
            services.AddScoped(typeof(ICommander<>), typeof(DatabaseCommander<>));

            services.AddSingleton(Configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserClaimsRepository, UserClaimsRepository>();
            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IMeetupRepository, MeetupRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserClaimsService, UserClaimsService>();
            services.AddScoped<IAttendeeService, AttendeeService>();
            services.AddScoped<IMeetupService, MeetupService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IClaimsTransformation, ProfileClaimsTransformationService>();

            services.AddTransient<IAuthorizationHandler, MeetupCoFounderHandler>();
            services.AddTransient<IAuthorizationHandler, MeetupFounderHandler>();
            services.AddTransient<IAuthorizationHandler, MeetupMemberHandler>();
            services.AddTransient<IAuthorizationHandler, MeetupRsvpHandler>();

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
                    options.AccessDeniedPath = "/denied";
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy =
                    new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();

                var minimumAge = Configuration.GetSection("Policies:Age").Get<int>();
                options.AddPolicy(Policies.AgeRestriction, policy => 
                    policy.AddRequirements(new AgeRestrictionRequirement(minimumAge)));

                var domains = Configuration.GetSection("Policies:Domains").Get<List<string>>();
                options.AddPolicy(Policies.DomainRestriction, policy => 
                    policy.AddRequirements(new DomainRestrictionRequirement(domains)));

                options.AddPolicy(Policies.MeetupRestriction, policy => 
                    policy.AddRequirements(new MeetupAccessRequirement()));
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
