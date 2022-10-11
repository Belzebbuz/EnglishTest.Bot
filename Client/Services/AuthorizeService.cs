using Infrastructure.Context;

namespace Client.Services;

public class AuthorizeService : IBotUserService
{
	private readonly ApplicationDbContext _context;
	static Role[] _roles = Enum.GetValues<Role>();
	public AuthorizeService(ApplicationDbContext context)
	{
		_context = context;
	}
	public ValueTask<(string id, string[] roles)> GetUserIdWithRoles(long tgUserId)
	{
		var user = _context.Users.FirstOrDefault(x => x.TgUserId == tgUserId);
		if (user == null)
		{
			return new((null, null));
		}

		var id = user.Id.ToString();
		var roles = GetRoles(user.Role);
		return new((id, roles));
	}

	private string[] GetRoles(Role roles)
	{

		return _roles.Where(c => ((int)c & (int)roles) == (int)c)
			.Select(c => c.ToString())
			.ToArray();
	}
}
