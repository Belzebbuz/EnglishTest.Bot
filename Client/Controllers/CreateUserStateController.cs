using Client.BotStates;
using Infrastructure.Contracts;

namespace Client.Controllers;

public class CreateUserStateController : BotControllerState<CreateUserState>
{
	private readonly IUserRepository _userRepository;

	public CreateUserStateController(IUserRepository userRepository) => _userRepository = userRepository;

	public override async ValueTask OnEnter()
	{
		try
		{
			await Send("Для начала работы нужно пройти регистрацию.");
			await Send("Введите имя:");
			var name = await AwaitText();
			await _userRepository.CreateUserAsync(Context.GetUsername(), name, Context.GetUserId(), ChatId);
			await Send("Ты успешно прошел регистрацию!\n/start - Узнать воможности");
		}
		finally
		{
			await GoBackAsync();
		}

	}

	[Action(nameof(GoBackAsync))]
	public async Task GoBackAsync()
	{
		await GlobalState(null);
	}
}
