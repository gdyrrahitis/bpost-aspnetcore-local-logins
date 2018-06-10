namespace Authentication.Local
{
    using System.Collections.Generic;
    using Authentication.Local.Infrastructure.Constants;
    using Authentication.Local.Models;
    using Infrastructure.Security;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Repositories;
    using Services;
    using Syrx.AspNetCore.Configuration.SqlServer;

    public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var authSettings = Configuration.GetSection("AuthSettings");
            var appSetting = Configuration.GetSection("Settings");
            services.AddSyrxSqlServer(appSetting);

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserClaimsRepository, UserClaimsRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserClaimsService, UserClaimsService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IClaimsTransformation, ProfileClaimsTransformationService>();

            services.AddTransient<IAuthorizationHandler, BlogAdminHandler>();
            services.AddTransient<IAuthorizationHandler, BlogAuthorHandler>();
            services.AddTransient<IAuthorizationHandler, BlogModeratorHandler>();
            services.AddTransient<IAuthorizationHandler, BlogFreezeHandler>();

            services.Configure<Roles>(options => Configuration.GetSection("Roles").Bind(options));
            services.Configure<LogoutPrompt>(options => Configuration.GetSection("Account").Bind(options));
            services.Configure<AuthSettings>(options => authSettings.Bind(options));

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
                var domains = authSettings.GetSection("Domains").Get<List<string>>();
                var minimumAge = authSettings.GetSection("Age").Get<int>();

                options.DefaultPolicy =
                    new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build();

                options.AddPolicy(Policies.AgeRestriction, policy =>
                    policy.AddRequirements(new AgeRestrictionRequirement(minimumAge)));

                options.AddPolicy(Policies.DomainRestriction, policy =>
                    policy.AddRequirements(new DomainRestrictionRequirement(domains)));

                options.AddPolicy(Policies.BlogAccessRestriction, policy =>
                    policy
                    .RequireAuthenticatedUser()
                    .AddRequirements(new BlogAccessRequirement()));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseBrowserLink();
            app.UseDeveloperExceptionPage();
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
