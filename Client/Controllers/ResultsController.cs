using Client.BotStates;
using Client.Extensions;
using Deployf.Botf;
using Infrastructure.Contracts;
using Infrastructure.DTOs;

namespace Client.Controllers;

public class ResultsController : BotController
{
	private readonly IUserRepository _userRepository;

	public ResultsController(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	[Action("/getallresults")]
	[Authorize("admin")]
	public async Task GetAllUserResult()
	{
		List<AppUserDTO> users = await _userRepository.GetAllUsersAsync();
		foreach (var user in users)
		{
			RowButton(user.FullName, Q(GetUsersResult, user.TgUserId));
		}
		await Send("Пользователи: ");
	}

	[Action("/results", "Получить результаты тестов")]
	public async Task GetResults()
	{
		AppUser appUser = await _userRepository.GetUserByTelegramId(Context.GetUserId());
		if (appUser == null)
			await GlobalState(new CreateUserState());

		await GetUsersResult(Context.UserId());
	}

	[Action]
	public async Task GetUsersResult(long id)
	{
		await Client.TryDeleteMessageAsync(ChatId, Context.Update?.CallbackQuery?.Message?.MessageId);
		await GlobalState(new UsersResultState(id));
	}
}
