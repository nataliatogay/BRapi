using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.EF;
using BR.Models;
using BR.Services;
using BR.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BR
{
    public class Startup
    {

        private AuthOptions _authOptions;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _authOptions = Configuration.GetSection(nameof(AuthOptions)).Get<AuthOptions>();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration);
            services.Configure<AuthOptions>(options =>
            {
                Configuration.GetSection("AuthOptions").Bind(options);
            });


            services.AddDbContext<BRDbContext>(
                options =>
                {
                    string connstr = Configuration.GetConnectionString("DefaultConnection");
                    //string connstr2 = Configuration.GetConnectionString("DefaultConnection2");
                    options.UseNpgsql(connstr);
                    //options.UseSqlServer(connstr2);
                    //options.UseLazyLoadingProxies();
                });

            services.AddScoped<BRDbContext>();
            services.AddScoped<IAsyncRepository, EFAsyncRepository>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IWaiterService, WaiterService>();
            services.AddScoped<IAdminAccountService, AdminAccountService>();
            services.AddScoped<IClientAccountService, ClientAccountService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            //services.AddScoped<IAdminMailService, AdminMailService>();
            services.AddScoped<IClientRequestService, ClientRequestService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddIdentityCore<IdentityUser>().AddSignInManager<SignInManager<IdentityUser>>()
                .AddUserManager<UserManager<IdentityUser>>()
                .AddEntityFrameworkStores<BRDbContext>();
            services.AddIdentityCore<IdentityUser>().AddDefaultTokenProviders();
            
            //services.AddIdentityCore<IdentityUser>().

            //services.AddIdentity<IdentityUser, IdentityRole>(s =>
            //{
            //}).AddEntityFrameworkStores<BRDbContext>();
            
            //services.AddAuthorization(s =>
            //{
                
            //});
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = _authOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _authOptions.Audience,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = _authOptions.GetSymmetricSecurityKey(),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            services.Configure<IdentityOptions>(
                options => 
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 5;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                });
            services.AddSingleton<IEmailConfiguration>(Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
            services.AddSingleton<ISMSConfiguration>(Configuration.GetSection("TwilioConfiguration").Get<TwilioConfiguration>());
            services.AddTransient<IEmailService, EmailService>();
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseMvc();
        }
    }
}
