using Domain.Models;
using Infrastructure.DTOs;
using Infrastructure.Wrappers;

namespace Infrastructure.Contracts;

public interface IUserRepository
{
	Task AddNewWordToVocabulary(long userId, string enWord, string ruVersion);
	Task<TestDTO> CreateTestAsync(long v1, int v2);
	Task CreateUserAsync(string? userName, string name, long userId, long chatId);
	Task DeleteWordAsync(long userId, Guid id);
	Task<string> GetTestResultAsync(long userId, Guid id);
	Task<AppUser?> GetUserByTelegramId(long userId);
	Task<Word?> GetWordByEnVersionAsync(long userId, string enWord);
	Task<Word?> GetWordByIdAsync(Guid id);
	Task MarkTestAsDoneAsync(long userId, Guid id);
	Task MarkTestAsStartedAsync(long userId, Guid id);
	Task SetQuestionAnswerAsync(long userId, Guid testId, Guid questionId, string text, bool isCorrect);
}
