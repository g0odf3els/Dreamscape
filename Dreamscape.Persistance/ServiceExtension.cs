using Dreamscape.Application.Repositories;
using Dreamscape.Application.Services;
using Dreamscape.Domain.Entities;
using Dreamscape.Persistance.Context;
using Dreamscape.Persistance.Repositories;
using Dreamscape.Persistance.Services.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WallpaperPortal.Services;
namespace Dreamscape.Persistance
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration["PostgresSQLConnection:ConnectionString"];

            services.AddDbContext<DataContext>(options =>
                    options.UseNpgsql(connectionString, o => o.UseVector()));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IResolutionRepository, ResolutionRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IColorRepository, ColorRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();

            services.AddScoped<IModelPredictionService, ModelPredictionService>();

            var emailConfig = configuration.GetSection("EmailConfiguration")
              .Get<EmailConfiguration>();

            services.AddSingleton(emailConfig);
            services.AddScoped<IEmailService, EmailService>();

            services.AddIdentity<User, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredLength = 1;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
                opt.TokenLifespan = TimeSpan.FromHours(2));
        }
    }
}

