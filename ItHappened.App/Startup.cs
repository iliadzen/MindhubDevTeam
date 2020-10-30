using System.Text;
using ItHappened.App.Authentication;
using ItHappened.Application;
using ItHappened.Domain;
using ItHappened.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
            services.AddSingleton<IJwtIssuer, JwtIssuer>();
            
            services.AddSingleton<IRepository<User>, InMemoryRepository<User>>();
            services.AddSingleton<IRepository<Tracker>, InMemoryRepository<Tracker>>();
            services.AddSingleton<IRepository<Event>, InMemoryRepository<Event>>();

            services.AddSingleton<IHasher, Sha256Hasher>();
            
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ITrackerService, TrackerService>();
            
            // Controllers registration
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}