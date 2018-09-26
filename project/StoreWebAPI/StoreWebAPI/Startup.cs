using System;
using System.Text;
using System.Threading.Tasks;
using ClothingStore.Repository.Context;
using ClothingStore.Repository.Interfaces;
using ClothingStore.Repository.Repository;
using ClothingStore.Service.Chat;
using ClothingStore.Service.Interfaces;
using ClothingStore.Service.Services;
using ClothingStore.Service.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace ClothingStore {
    public class Startup {
        public Startup(IHostingEnvironment env) {
            var builder = new ConfigurationBuilder()
                          .SetBasePath(env.ContentRootPath)
                          .AddJsonFile("appsettings.json", false, true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                          .AddEnvironmentVariables();
            this.Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddOptions();
            //DB
            services.AddDbContext<ApplicationContext>(
                options => options.UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection"))
            );
            //Services
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISecurityService, SecurityService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICouponService, CouponCodeService>();
            services.AddScoped<IFavoriteItemService, FavoriteItemService>();
            services.AddScoped<ICategoryService, CategoryService>();
            //Options
            services.Configure<AuthSettings>(this.Configuration.GetSection("AuthOptions"));

            services.Configure<BaseGeneratorSettings>("PassOptions",this.Configuration.GetSection("PassOptions"));
            services.Configure<BaseGeneratorSettings>("CodeOptions", this.Configuration.GetSection("CodeOptions"));

            //CORS
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    policy =>
                        policy.AllowAnyOrigin()
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials()
                              .Build());
            });
            //Auth
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options => {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters {

                            ValidateIssuer = true,

                            ValidIssuer = this.Configuration["AuthOptions:ISSUER"],

                            ValidateAudience = true,

                            ValidAudience = this.Configuration["AuthOptions:AUDIENCE"],

                            ValidateLifetime = true,

                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["AuthOptions:KEY"])),

                            ValidateIssuerSigningKey = true,
                            ClockSkew = TimeSpan.Zero
                        };
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var token = context.HttpContext.Request.Headers["Authorization"];
                                if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                                {
                                    context.Token = token[0].Substring("Token ".Length).Trim();
                                } else if(token.Count > 0 && token[0].StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase)) {
                                    context.Token = token[0].Substring("Bearer ".Length).Trim();
                                }
                                else context.Token = token;

                                return Task.CompletedTask;
                            }
                        };
                    });

            services.AddAuthorization(options => {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            });
            services.AddSignalR();
            services.AddResponseCompression(options => {
                
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Info { Title = "StoreWebAPI API", Version = "v1" });
            });

            services.AddLogging();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole(LogLevel.Debug);
            loggerFactory.AddDebug();

            if(env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else app.UseHsts();

            app.UseCors("CorsPolicy");
            app.UseStaticFiles();
            app.UseSignalR(routes => routes.MapHub<ChatHub>("/api/chat"));
            app.UseAuthentication();

            app.UseResponseCompression();

            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ClothingStoreWebAPI API");
            });
        }
    }
}
