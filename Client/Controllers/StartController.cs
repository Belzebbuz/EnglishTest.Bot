using Client.BotStates;
using Client.Extensions;
using Infrastructure.Contracts;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

namespace Client.Controllers;

public class StartController : BotController
{
    private readonly IUserRepository _userRepository;

    public StartController(IUserRepository userRepository) => _userRepository = userRepository;

	[Action("/start", "Начало работы")]
    public async Task StartAsync()
    {
        AppUser user = await _userRepository.GetUserByTelegramId(Context.GetUserId());
		if (user == null)
        {
            await GlobalState(new CreateUserState());
        }
        else
        {
            PushL("/wordmenu - Управление словарем");
            PushL("/tests - режим тестирования");
            PushL("/results - Получить результаты тестов");
        }
    }

    [On(Handle.ClearState)]
    public async Task EmptyState()
    {
		await Client.TryDeleteMessageAsync(ChatId, Context.Update?.CallbackQuery?.Message?.MessageId);
	}

	[On(Handle.Unknown)]
	public async Task Unknown()
	{
        await GlobalState(null);
	}
	[On(Handle.Exception)]
	public async Task Ex(Exception e)
	{
		Console.WriteLine(e.GetBaseException().Message);
		await Send(e.Message);
	}
}
