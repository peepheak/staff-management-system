using System.Reflection;
using API.Common;
using API.Context;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class ServiceCollectionExtensions
{
    private const string CorsPolicy = nameof(CorsPolicy);

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(
                configuration.GetConnectionString("StaffConnection"),
                npgsqlOptions => { npgsqlOptions.EnableRetryOnFailure(); });
        });

        return services;
    }

    public static IServiceCollection AddServiceRegister(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromApplicationDependencies()
            .AddClasses(c => c.AssignableTo<IScopedService>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            .AddClasses(c => c.AssignableTo<ISingletonService>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime()
            .AddClasses(c => c.AssignableTo<ITransientService>())
            .AsImplementedInterfaces()
            .WithTransientLifetime()
        );

        return services;
    }

    public static void AddMappings(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AllowNullCollections = true;
            mc.ShouldMapMethod = _ => false;
            mc.AddMaps(Assembly.GetExecutingAssembly());
        }, new LoggerFactory());

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddAutoMapper(cfg =>
        {
            cfg.AllowNullCollections = true;
            cfg.ShouldMapMethod = _ => false;
        }, Assembly.GetExecutingAssembly());
    }

    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        return services.AddCors(opt =>
            opt.AddPolicy(CorsPolicy, policy =>
                policy.AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowAnyMethod()
                    .AllowCredentials()
            ));
    }

    public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
        app.UseCors(CorsPolicy);
}