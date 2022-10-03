using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context;

public static class DbMigrator
{
	public static void InitDatabaseAsync<T>(this IApplicationBuilder app) where T : DbContext
	{
		using var scope = app.ApplicationServices.CreateScope();
		var context = scope.ServiceProvider.GetService<T>();
		context.Database.Migrate();
	}
}
