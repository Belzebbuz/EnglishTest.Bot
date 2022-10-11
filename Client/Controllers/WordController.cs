using Client.BotStates;
using Client.Extensions;
using Infrastructure.Contracts;
using System;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Client.Controllers;

public class WordController : BotController
{
	private readonly IUserRepository _userRepository;
	public WordController(IUserRepository userRepository) => _userRepository = userRepository;


	[Action("/wordmenu", "Управление словом")]
	public async Task AddNewWordAsync()
	{
		AppUser user = await _userRepository.GetUserByTelegramId(Context.GetUserId());
		if (user == null)
		{
			await GlobalState(new CreateUserState());
		}
		await Send("Введи слово на английском");
		var enWord = await AwaitText();
		var wordEntity = await _userRepository.GetWordByEnVersionAsync(Context.UserId(), enWord.ToUpper());
		if(wordEntity == null)
		{
			RowButton("Добавить", Q(AddNewWord, enWord));
			RowButton("Нет", Q(DeleteMessage));
			await Send("Такое слово не найдено!\n Добавить?");
		}
		else
		{
			await GlobalState(new WordMenuState(wordEntity.Id));
		}
	}

	[Action]
	private async Task AddNewWord(string enWord)
	{
		await Client.TryDeleteMessageAsync(ChatId, (int)Context.GetCallbackMessageId());
		await Send("Введи перевод:");
		var ruVersion = await AwaitText();
		await _userRepository.AddNewWordToVocabulary(Context.UserId(), enWord.ToUpper().Trim(), ruVersion.ToUpper().Trim());
		await Send("Слово успешно добавлено в словарь!");
	}

	[Action]
	private async Task DeleteMessage()
	{
		await Client.TryDeleteMessageAsync(ChatId, (int)Context.GetCallbackMessageId());
	}
}
