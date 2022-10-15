using Infrastructure.Contracts;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Passport;

namespace Client.Controllers;

public class AdminController : BotController
{
	private readonly IUserRepository _userRepository;

	public AdminController(IUserRepository userRepository) => _userRepository = userRepository;

	[Authorize]
	[Action("/makemeadmin")]
	public async Task MakeAdminAsync()
	{
		await Send("Введите пароль:");
		var password = await AwaitText();
		if (password == "密码")
		{
			await _userRepository.MakeUserAdminAsync(Context.UserId());
			await Send("Вы админ");
		}
		else
		{
			await Send("Неверно");
		}
	}

	[Authorize("admin")]
	[Action("/sendnotification")]
	public async Task SendUsersNotification()
	{
		foreach (var user in await _userRepository.GetAllUsersAsync())
		{
			await Client.SendTextMessageAsync(user.ChatId, "/tests - нужно пройти тест на 10 случайных слов!");
		}
	}
}
