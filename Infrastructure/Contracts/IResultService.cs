using Infrastructure.DTOs;

namespace Infrastructure.Contracts;

public interface IResultService
{
	Task<List<string>> GetAllUsersResultsAsync(long id);
	Task<List<string>> GetUsersResultsAsync(long id, int count);
}
