using Client.BotStates;
using Infrastructure.Contracts;
using System;
using System.Threading.Tasks;

namespace Client.Controllers;

public class UsersResultStateController : BotControllerState<UsersResultState>
{
	private readonly IResultService _resultService;
	private readonly int[] resultsCount = new[] { 1, 5, 10 };
	public UsersResultStateController(IResultService resultService) => _resultService = resultService;

	public override async ValueTask OnEnter()
	{
		foreach (var count in resultsCount)
		{
			Button(count.ToString(), Q(ShowResults, count));
		}
		RowButton("20", Q(ShowResults, 20));
		await Send("Выберите количество последних ответов");
	}

	[Action]
	public async Task ShowResults(int count)
	{
		try
		{
			List<string> results =  await _resultService.GetUsersResultsAsync(StateInstance.Id, count);
			foreach (var result in results)
			{
				PushL(result);
				PushL($"{string.Join("",Enumerable.Range(1, result.Split("\n").First().Length).Select(x => "-"))}");
			}
			await Send();
		}
		finally
		{
			await GoBackAsync();
		}

	}

	[Action]
	private async Task GoBackAsync()
	{
		await GlobalState(null);
	}
}
