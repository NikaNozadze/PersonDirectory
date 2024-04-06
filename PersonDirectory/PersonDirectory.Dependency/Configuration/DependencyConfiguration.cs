using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PersonDirectory.Database;
using PersonDirectory.Interfaces.DbContext;
using PersonDirectory.Interfaces.Repositories;
using PersonDirectory.Interfaces.Services;
using PersonDirectory.Repository;
using PersonDirectory.Service;
using PersonDirectory.Sevice;

namespace PersonDirectory.Dependency.Configuration;

public static class DependencyConfiguration
{
    public static void ConfigureDependency(this WebApplicationBuilder builder)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IUnitOfWork>().CityRepository);
        builder.Services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IUnitOfWork>().PersonRepository);
        builder.Services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IUnitOfWork>().PhoneNumberRepository);
        builder.Services.AddScoped(serviceProvider => serviceProvider.GetRequiredService<IUnitOfWork>().RelatedPersonRepository);

        builder.Services.AddScoped<ICityService, CityService>();
        builder.Services.AddScoped<IPersonService, PersonService>();
        builder.Services.AddScoped<IPhoneNumberService, PhoneNumberService>();
        builder.Services.AddScoped<IRelatedPersonService, RelatedPersonService>();
        builder.Services.AddScoped<IImageService, ImageService>();
        builder.Services.AddScoped<IPersonDetailsService, PersonDetailsService>();
        builder.Services.AddScoped<ISearchService, SearchService>();


        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        builder.Services.AddScoped<IPersonDirectoryDbContext, PersonDirectoryDbContext>();
        builder.Services.AddDbContext<PersonDirectoryDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("PersonDirectory.Database")));
    }
}