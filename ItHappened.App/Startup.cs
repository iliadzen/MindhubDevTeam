using System.Text;
using ItHappened.App.Authentication;
using ItHappened.App.Filters;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Domain.Customizations;
using ItHappened.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ItHappened.App
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            // Jwt configure
            var jwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
            services.AddSingleton(jwtConfig);
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                        ValidateLifetime = true
                    };
                });
            
            // Services registration
            services.AddDbContext<CommonDbContext>(
                builder => builder.UseSqlServer(
                    Configuration
                        .GetSection("Database")
                        .GetValue<string>("ConnectionString")
                    )
                );
            // Repositories
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<License>, LicenseRepository>();
            services.AddScoped<IRepository<Tracker>, TrackerRepository>();
            services.AddScoped<IRepository<Event>, EventRepository>();
            
            services.AddScoped<IRepository<Comment>, CommentRepository>();
            services.AddScoped<IRepository<Rating>, RatingRepository>();
            services.AddScoped<IRepository<Scale>, ScaleRepository>();
            services.AddScoped<IRepository<Geotag>, GeotagRepository>();
            services.AddScoped<IRepository<Photo>, PhotoRepository>();

            // Utils
            services.AddScoped<IJwtIssuer, JwtIssuer>();
            services.AddScoped<IHasher, Sha256Hasher>();
            
            // Filters
            services.AddScoped<SaveChangesFilter>();

            // Services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITrackerService, TrackerService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ICustomizationService, CustomizationService>();
            
            // Controllers
            services.AddControllers(options =>
            {
                options.Filters.AddService<SaveChangesFilter>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors(
                builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}