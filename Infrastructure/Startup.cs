using Infrastructure.Context;
using Infrastructure.Contracts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlite("Data Source=mainbot.db", b => b.MigrationsAssembly("Migrators.Sqlite"));
		});
		services.AddTransient<IUserRepository, UserRepository>();
		services.AddTransient<IResultService, ResultService>();
		return services;
    }
}
