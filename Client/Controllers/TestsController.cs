using Client.BotStates;
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
	public async Task StartTestAsync()
	{
		AppUser user = await _userRepository.GetUserByTelegramId(Context.GetUserId());
		if (user == null)
		{
			await GlobalState(new CreateUserState());
		}
		TestDTO test = await _userRepository.CreateTestAsync(Context.UserId(), 10);
		await Send("Сейчас начнется тест");
		await _userRepository.MarkTestAsStartedAsync(Context.UserId(), test.Id);

		foreach (var question in test.Questions)
		{
			await Send($"Введи перевод слова <b>{question.Word.RuVersion}</b>");
			var text = await AwaitText();
			await _userRepository.SetQuestionAnswerAsync(Context.UserId(), test.Id, question.Id, text, text == question.Word.EnVersion);
		}
		await _userRepository.MarkTestAsDoneAsync(Context.UserId(), test.Id);
		string result = await _userRepository.GetTestResultAsync(Context.UserId(), test.Id);
		await Send(result);
	}
}
