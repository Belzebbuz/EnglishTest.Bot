using Client.BotStates;
using Client.Extensions;
using Infrastructure.Contracts;

namespace Client.Controllers;

public class WordMenuStateController : BotControllerState<WordMenuState>
{
	private readonly IUserRepository _userRepository;

	public WordMenuStateController(IUserRepository userRepository) => _userRepository = userRepository;

	public override async ValueTask OnEnter()
	{

		Word word = await _userRepository.GetWordByIdAsync(StateInstance.Id);
		if (word == null)
			await Send("Слово не найдено");
		RowButton("Удалить", Q(DeleteWordAsync));
		RowButton("Назад", Q(GoBackAsync));
		await Send($"{word.EnVersion} - {word.RuVersion}");
	}

	[Action]
	private async Task DeleteWordAsync()
	{
		await Client.TryDeleteMessageAsync(ChatId, Context.GetCallbackMessageId());
		await _userRepository.DeleteWordAsync(Context.UserId(), StateInstance.Id);
		await Send("Слово успешно удалено из словаря!");
		await GoBackAsync();
	}

	[Action(nameof(GoBackAsync))]
	public async Task GoBackAsync()
	{
		await GlobalState(null);
	}
}
