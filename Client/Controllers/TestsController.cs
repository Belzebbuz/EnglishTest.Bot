using Client.BotStates;
using Client.Extensions;
using Infrastructure.Contracts;
using Infrastructure.DTOs;
using System;
using System.Threading.Tasks;

namespace Client.Controllers;

public class TestsController : BotController
{
	private readonly IUserRepository _userRepository;
	
	public TestsController(IUserRepository userRepository) => _userRepository = userRepository;

	[Action("/tests", "Тесты")]
	public async Task SelectTestAsync()
	{
		AppUser user = await _userRepository.GetUserByTelegramId(Context.GetUserId());
		if (user == null)
		{
			await GlobalState(new CreateUserState());
		}
		RowButton("10 Случайных слов",Q(StartRandomTestAsync));
		RowButton("Последние слова",Q(GenerateTestAsync));
		await Send("Выберите режим тестирования");
	}
	[Action]
	private async Task GenerateTestAsync()
	{
		await Client.TryDeleteMessageAsync(ChatId, Context.GetCallbackMessageId());
		await Send("Сколько последних добавленных слов использовать в тесте?");
		var countText = await AwaitText();
		if(int.TryParse(countText, out int count))
		{
			TestDTO test = await _userRepository.CreateTestAsync(Context.UserId(), count);
			await StartTestAsync(test);
		}
		else
		{
			await Send("Нужно указать число!");
		}
	}

	[Action]
	private async Task StartRandomTestAsync()
	{
		await Client.TryDeleteMessageAsync(ChatId, Context.GetCallbackMessageId());
		TestDTO test = await _userRepository.CreateRandomTestAsync(Context.UserId(), 10);
		await StartTestAsync(test);
	}

	private async Task StartTestAsync(TestDTO test)
	{
		await Send("Сейчас начнется тест");
		await _userRepository.MarkTestAsStartedAsync(Context.UserId(), test.Id);

		foreach (var question in test.Questions)
		{
			await Send($"Введи перевод слова <b>{question.Word.RuVersion}</b>");
			var text = await AwaitText();
			await _userRepository.SetQuestionAnswerAsync(Context.UserId(), test.Id, question.Id, text, text.ToUpper() == question.Word.EnVersion.ToUpper());
		}
		await _userRepository.MarkTestAsDoneAsync(Context.UserId(), test.Id);
		string result = await _userRepository.GetTestResultAsync(Context.UserId(), test.Id);
		await Send(result);
	}
}
