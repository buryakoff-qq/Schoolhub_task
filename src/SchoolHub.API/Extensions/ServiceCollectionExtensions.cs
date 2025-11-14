using Microsoft.EntityFrameworkCore;
using SchoolHub.Application.Interfaces;
using SchoolHub.Application.Services;
using SchoolHub.Infrastructure.Persistence;
using SchoolHub.Infrastructure.Repositories;

namespace SchoolHub.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSchoolHubServices(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseInMemoryDatabase("SchoolHubDb");
        });

        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ISchoolClassRepository, SchoolClassRepository>();

        services.AddScoped<StudentService>();
        services.AddScoped<SchoolClassService>();

        return services;
    }
}