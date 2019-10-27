using Autofac;
using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Reflection;
using TIKSN.Data.Mongo;
using TIKSN.DependencyInjection;
using TIKSN.Habitica;
using TIKSN.Lionize.HabiticaTaskProviderService.Business;
using TIKSN.Lionize.HabiticaTaskProviderService.Business.IdentityGenerator;
using TIKSN.Lionize.HabiticaTaskProviderService.Data;
using TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.BackgroundServices;
using TIKSN.Lionize.HabiticaTaskProviderService.WebAPI.Options;
using TIKSN.Lionize.Messaging;
using TIKSN.Lionize.Messaging.Options;

namespace TIKSN.Lionize.HabiticaTaskProviderService.WebAPI
{
    public class Startup
    {
        private readonly string AllowSpecificCorsOrigins = "_AllowSpecificCorsOrigins_";

        public Startup(IConfiguration configuration)
        {
            Configuration = (IConfigurationRoot)configuration;
        }

        public IConfigurationRoot Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production
                // scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0/swagger.json", "API 1.0");
            });

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(AllowSpecificCorsOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(opt =>
            {
                opt.MapControllers();
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new CoreModule());
            builder.RegisterModule(new BusinessAutofacModule());
            builder.RegisterModule(new DataAutofacModule());
            builder.RegisterModule(new MessagingAutofacModule());

            builder.RegisterType<DatabaseProvider>().As<IMongoDatabaseProvider>().SingleInstance();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = false;
                o.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = false;
                o.SubstituteApiVersionInUrl = true;
            });

            var servicesConfigurationSection = Configuration.GetSection("Services");
            services.Configure<ServiceDiscoveryOptions>(servicesConfigurationSection);

            var webApiResourceOptions = new WebApiResourceOptions();
            Configuration.GetSection("ApiResource").Bind(webApiResourceOptions);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
            .AddIdentityServerAuthentication(options =>
            {
                var serviceDiscoveryOptions = new ServiceDiscoveryOptions();
                servicesConfigurationSection.Bind(serviceDiscoveryOptions);

                options.Authority = $"{serviceDiscoveryOptions.Identity.BaseAddress}";
                options.RequireHttpsMetadata = false;

                options.ApiName = webApiResourceOptions.ApiName;
                options.ApiSecret = webApiResourceOptions.ApiSecret;
            });

            services.AddAuthorization(options =>
            {
            });

            services.Configure<UnsignedBigIntegerIdentityGeneratorOptions>(options =>
            {
                options.ByteLength = 16;
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("1.0", new OpenApiInfo { Title = "Lionize / Habitica Task Provider Service", Version = "1.0" });

                var def = new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", def);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {def, new List<string>()}
                });
            });

            services.AddDataProtection()
                .PersistKeysToRedis(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

            services.AddAutoMapper((provider, exp) =>
            {
                exp.AddProfile(new BusinessMappingProfile(provider.GetRequiredService<IDataProtectionProvider>()));
            }, typeof(WebApiMappingProfile));

            services.AddCors(options =>
            {
                options.AddPolicy(AllowSpecificCorsOrigins,
                cpbuilder =>
                {
                    var origins = Configuration.GetSection("Cors").GetSection("Origins").Get<string[]>();

                    if (origins != null)
                    {
                        cpbuilder.AllowAnyMethod();
                        cpbuilder.AllowAnyHeader();
                        cpbuilder.WithOrigins(origins);
                    }
                });
            });

            services.Configure<ApplicationOptions>(opt =>
            {
                opt.ApplictionId = "HabiticaTaskProviderService";
                opt.ApplictionQueuePart = "habitica";
            });

            services.AddSingleton(Configuration);

            services.AddDistributedRedisCache(opt =>
            {
                opt.Configuration = Configuration.GetConnectionString("Redis");
                opt.InstanceName = "HabiticaTaskProviderService";
            });

            services.AddHostedService<PullTodosBackgroundService>();

            services.AddMediatR(typeof(BusinessAutofacModule).GetTypeInfo().Assembly);

            services.AddFrameworkPlatform();
            services.AddHabitica();
        }
    }
}