using Domain.Models;
using Infrastructure.Context;
using Infrastructure.Contracts;
using Infrastructure.DTOs;
using Infrastructure.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
	private readonly ApplicationDbContext _context;

	public UserRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task AddNewWordToVocabulary(long userId, string enWord, string ruVersion)
	{
		var user = await _context.Users.Include(x => x.Words).FirstOrDefaultAsync(x => x.TgUserId == userId);
		if (user == null)
			throw new ArgumentNullException(nameof(user));
		if(user.Words.Any(x => x.EnVersion == enWord))
			throw new ArgumentException($"У пользователя уже имеется слово {enWord} в словаре");

		user.AddNewWord(enWord, ruVersion);
		await _context.SaveChangesAsync();
	}


	public async Task CreateUserAsync(string? userName, string name, long userId, long chatId)
	{
		var exists = await _context.Users.AnyAsync(x => x.TgUserId == userId);
		if (exists)
			throw new ArgumentException($"Пользователь с telegramId: {userId} уже зарегистрирован!");
		var user = new AppUser(userName, name, userId, chatId);
		user.Role = Role.basic;
		await _context.AddAsync(user);
		await _context.SaveChangesAsync();
	}

	public async Task DeleteWordAsync(long userId, Guid id)
	{
		var user = await _context.Users.Include(x => x.Words).FirstOrDefaultAsync(x => x.TgUserId == userId);
		if (user == null)
			throw new ArgumentNullException(nameof(user));
		user.RemoveWord(id);
		await _context.SaveChangesAsync();	
	}

	public async Task<string> GetTestResultAsync(long userId, Guid id)
	{
		AppUser? user = await GetUserByTgIdAsync(userId);
		return user.GetTestResult(id);
	}

	public async Task<AppUser?> GetUserByTelegramId(long userId)
	{
		return await _context.Users.Include(x => x.Words).FirstOrDefaultAsync(x => x.TgUserId == userId);
	}

	public async Task<Word?> GetWordByEnVersionAsync(long userId, string enWord)
	{
		AppUser? user = await GetUserByTgIdAsync(userId);
		return user.Words.FirstOrDefault(x => x.EnVersion == enWord);
	}

	private async Task<AppUser> GetUserByTgIdAsync(long userId)
	{
		var user = await _context.Users
			.Include(x => x.Words)
			.Include(x => x.Tests)
			.ThenInclude(x => x.Questions)
			.FirstOrDefaultAsync(x => x.TgUserId == userId);
		if (user == null)
			throw new ArgumentNullException(nameof(user));
		return user;
	}

	public async Task<Word?> GetWordByIdAsync(Guid id)
	{
		return await _context.Words.FirstOrDefaultAsync(x => x.Id == id);
	}

	public async Task MarkTestAsDoneAsync(long userId, Guid id)
	{
		AppUser? user = await GetUserByTgIdAsync(userId);
		user.MarkTestAsDone(id);
		await _context.SaveChangesAsync();
	}

	public async Task MarkTestAsStartedAsync(long userId, Guid id)
	{
		AppUser? user = await GetUserByTgIdAsync(userId);
		user.MarkTestAsStarted(id);
		await _context.SaveChangesAsync();
	}

	public async Task SetQuestionAnswerAsync(long userId,Guid testId, Guid questionId, string text, bool isCorrect)
	{
		AppUser? user = await GetUserByTgIdAsync(userId);
		user.SetTestQuestionAnswer(testId, questionId, text, isCorrect);
		await _context.SaveChangesAsync();
	}

	public async Task<TestDTO> CreateTestAsync(long userId, int questionCount)
	{
		AppUser? user = await GetUserByTgIdAsync(userId);
		var result = user.GenerateRandomTest(questionCount);
		var questionDTO = new List<QuestionDTO>();
		await _context.SaveChangesAsync();
		result.Questions.ForEach(x => questionDTO.Add(new() { Id = x.Id, Word = new(x.Word.Id, x.Word.EnVersion, x.Word.RuVersion) }));
		return new()
		{
			Id = result.Id,
			Questions = questionDTO
		};
	}
}
