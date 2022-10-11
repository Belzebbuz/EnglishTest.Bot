using Domain.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
	public static class UserRepositoryExtensions
	{
		public static async Task<AppUser> GetUserByTgIdAsync(this ApplicationDbContext context, long userId)
		{
			var user = await context.Users
			.Include(x => x.Words)
			.Include(x => x.Tests)
			.ThenInclude(x => x.Questions)
			.FirstOrDefaultAsync(x => x.TgUserId == userId);
			if (user == null)
				throw new ArgumentNullException(nameof(user));
			return user;
		}
	}
}
