 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using BR.Utils.Notification;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using System.Security.Claims;
//using Microsoft.AspNetCore.SignalR.Redis;

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

        private RedisOptions GetRedisOptions()
        {
            var options = new RedisOptions();
            var section = Configuration.GetSection("Redis");
            //var secretUrl = section["ConnectionStringSecret"];
           // var secret = _keyVaultClient.GetSecretAsync(secretUrl).Result;


            options.ConnectionString = section["ConnectionString"];
            options.InstanceName = section["InstanceName"];
            return options;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthOptions>(Configuration);
            services.Configure<AuthOptions>(options =>
            {
                Configuration.GetSection("AuthOptions").Bind(options);
            });
            services.Configure<AzureStorageAccountOptions>(Configuration.GetSection("AzureStorageAccountOptions"));

            services.AddDbContext<BRDbContext>(
                options =>
                {
                    string connStrAzure = Configuration.GetConnectionString("AzureDbConnectionString");
                    string connSql = Configuration.GetConnectionString("SQLConnectionString");
                   // string connStrPostgre = Configuration.GetConnectionString("PostgreSQLConnectionString");
                   // options.UseNpgsql(connStrPostgre);
                    options.UseSqlServer(connStrAzure);
                    options.UseLazyLoadingProxies();
                });

            services.AddScoped<BRDbContext>();
            services.AddScoped<IAsyncRepository, EFAsyncRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IWaiterService, WaiterService>();
            services.AddScoped<IAdminAccountService, AdminAccountService>();
            services.AddScoped<IClientAccountService, ClientAccountService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IWaiterAccountService, WaiterAccountService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IOwnerRequestService, OwnerRequestService>();
            services.AddScoped<IParameterService, ParameterService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ISchemaService, SchemeService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IPushNotificationService, PushNotificationService>();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Info
                {
                    Title = "MyApi",
                    Version = "v1"
                });
                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                    In = "header",
                    Name = "Authorization",
                    Type = "apiKey"
                });
            });

            services.AddIdentityCore<IdentityUser>().AddSignInManager<SignInManager<IdentityUser>>()
                .AddUserManager<UserManager<IdentityUser>>()
                .AddRoles<IdentityRole>()
              // .AddRoleValidator<RoleValidator<IdentityRole>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<BRDbContext>();
            services.AddIdentityCore<IdentityUser>().AddDefaultTokenProviders();

            services.AddScoped<RoleManager<IdentityRole>>();

            // services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>().AddRoleManager<RoleManager<IdentityRole>>();

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

            services.AddAuthorization(opts => {
                opts.AddPolicy("Headwaiter", policy =>
                {
                    policy.RequireClaim(ClaimTypes.NameIdentifier, "Headwaiter");
                });
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

            // Add Quartz services
            services.AddSingleton<IJobFactory, JobFactory>();
            services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
            services.AddSingleton<QuartzJobRunner>();

            // Add our job
            //services.AddSingleton<ReservationNotificationJob>();
            services.AddScoped<DailyReservationReminderJob>();
            //services.AddSingleton(new JobSchedule(
            //    jobType: typeof(DailyReservationReminderJob),
            //    cronExpression: "0/5 * * * * ?"));


            // run every 5 seconds
            // "0 0 20 * * ?" => every day at 20:00

            services.AddHostedService<QuartzHostedService>();
            
            services.Configure<NotificationHubConfiguration>(Configuration.GetSection("NotificationHubConfiguration"));

            services.AddDistributedRedisCache(opt => {
                var options = GetRedisOptions();
                opt.Configuration = options.ConnectionString;
                opt.InstanceName = options.InstanceName;
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.Use(async (context, next) =>
            //{
            //    var req = context.Request;
            //    var reader = new StreamReader(req.Body);
            //    var data = reader.ReadLine();
            //    await next();
            //});
            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(o => o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseSwagger();
            app.UseSwaggerUI(setup => {
                setup.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "My API v1"
                );
            });

            app.UseMvc();
        }
    }
}

//hello