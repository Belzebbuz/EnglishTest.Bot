using Domain.Models;
using Infrastructure.Context;
using Infrastructure.Contracts;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class ResultService : IResultService
{
	private readonly ApplicationDbContext _context;

	public ResultService(ApplicationDbContext applicationDbContext) => _context = applicationDbContext;

	public async Task<List<string>> GetAllUsersResultsAsync(long id)
	{
		var user = await _context.GetUserByTgIdAsync(id);
		return user.GetAllResults();
	}

	public async Task<List<string>> GetUsersResultsAsync(long id, int count)
	{
		var user = await _context.GetUserByTgIdAsync(id);
		return user.GetLastResults(count);
	}
}
