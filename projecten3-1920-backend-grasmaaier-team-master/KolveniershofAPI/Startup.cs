using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using KolveniershofAPI.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using KolveniershofAPI.Data.Seeding;
using KolveniershofAPI.Model.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NSwag.SwaggerGeneration.Processors.Security;
using NSwag;
using KolveniershofAPI.Data.Repositories;
using System.Security.Claims;

namespace KolveniershofAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            #region MVC
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #endregion

            #region Configure DB-Context
            services.AddDbContext<ApplicationDBContext>(options =>
            {
                //mac connectionstring
                //options.UseSqlServer(Configuration.GetConnectionString("MacConnection"));
                //Docker connectionstring
                //options.UseSqlServer(Configuration.GetConnectionString("DockerConnection"));
                //Windows connectionstring
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Api configuration
            //Used for swagger and documentation
            services.AddOpenApiDocument(o =>
            {
                o.Title = "Kolveniershof API";
                o.Version = "alpha";
                o.DocumentName = "Kolveniershof";
                o.Description = "The api powering an Angular and Android application";
                o.DocumentProcessors.Add(new SecurityDefinitionAppender("JWT Token", new SwaggerSecurityScheme
                {
                    Type = SwaggerSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = SwaggerSecurityApiKeyLocation.Header,
                    Description = "Copy 'Bearer' + valid JWT token into field"
                }));
                o.OperationProcessors.Add(new OperationSecurityScopeProcessor("JWT Token"));
            });
            #endregion

            #region Identity + Configuration
            services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDBContext>();

            services.Configure<IdentityOptions>(options => {
                //Password configuration
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;

                //Disable lockout
                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = int.MaxValue;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.Zero;

                // Email login
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });
            #endregion

            #region Authentication Configuration
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme =JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Tokens:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //Ensure token hasn't expired
                    RequireExpirationTime = true
                };
            }); 
            #endregion

            #region Cors
            /*Allows for more relaxed subdomaining, ports, ... 
            /without the browser picking it up as something malicious*/
            services.AddCors(options =>
                options.AddPolicy("AllowAllOrigins", builder =>
                    builder.AllowAnyOrigin()
                )
            );
            #endregion

            #region Dependency Injection
            services
                .AddScoped<IClientRepository, ClientRepository>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IAtelierRepository,AtelierRepository>()
                .AddScoped<ITemplateRepository,TemplateRepository>()
                .AddScoped<ISfeergroepRepository,SfeergroepRepository>()
                .AddScoped<IBegeleiderRepository,BegeleiderRepository>()
                .AddScoped<IAtelierDagRepository,AtelierDagRepository>()
				.AddScoped<IDagRepository, DagRepository>()
                .AddScoped<IDagMenuRepository, DagMenuRepository>()
                .AddScoped<IBusRepository, BusRepository>();
            #endregion

            #region Authorization
            services.AddAuthorization(options =>{
                    options.AddPolicy("Client", policy => policy.RequireClaim(ClaimTypes.Role, "Client"));
                    options.AddPolicy("Begeleider", policy => policy.RequireClaim(ClaimTypes.Role, "Begeleider"));
                    options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"));
                }); 
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDBContext context,
            UserManager<IdentityUser> um){
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Make sure database is created and model creating is called.
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //Seed Data
            new DataInit(context, um).SeedData();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();

            app.UseSwaggerUi3();
            app.UseSwagger();

            app.UseCors("AllowAllOrigins");
        }
    }
}
