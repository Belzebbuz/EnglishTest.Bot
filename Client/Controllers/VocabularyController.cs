using Client.BotStates;
using Infrastructure.Contracts;
using Infrastructure.Wrappers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Client.Controllers;

public class VocabularyController : BotController
{
	private readonly IUserRepository _userRepository;
	private const int _pageSize = 10;
	public VocabularyController(IUserRepository userRepository) => _userRepository = userRepository;

	[Action("/vocabulary", "Словарь")]
	public async Task GetVocabularyAsync()
	{
		await SendWordsByPages();
	}

	[Action]
	private async Task SendWordsByPages(int page = 1)
	{
		AppUser user = await _userRepository.GetUserByTelegramId(Context.UserId());
		if (user == null)
			await GlobalState(new CreateUserState());

		var pagination = new Pagination(page, _pageSize);
		var words = await _userRepository.GetWordsAsync(Context.UserId(), pagination);
		foreach (var word in words)
		{
			PushL(word);
		}
		PushL($"\nСтраница {pagination.Page} из {pagination.TotalAmountOfPages}");

		if (pagination.Page != 1)
		{
			RowButton("<<<", Q(SendWordsByPages, pagination.PreviousPage));
		}
		if (pagination.Page != pagination.NextPage)
		{
			if (pagination.Page != 1)
			{
				Button(">>>", Q(SendWordsByPages, pagination.NextPage));
			}
			else
			{
				RowButton(">>>", Q(SendWordsByPages, pagination.NextPage));
			}
		}
	}
}
