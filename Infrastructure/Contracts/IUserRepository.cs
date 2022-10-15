using Domain.Models;
using Infrastructure.DTOs;
using Infrastructure.Wrappers;

namespace Infrastructure.Contracts;

public interface IUserRepository
{
	Task AddNewWordToVocabulary(long userId, string enWord, string ruVersion);
	Task AddToVocabularyOpenHistory(long chatId, int? messageId);
	Task<List<int>> ClearVocabularyHistoryAsync(long chatId);
	Task<TestDTO> CreateRandomTestAsync(long userId, int count);
	Task<TestDTO> CreateTestAsync(long userId, int count);
	Task CreateUserAsync(string? userName, string name, long userId, long chatId);
	Task DeleteWordAsync(long userId, Guid id);
	Task<List<AppUserDTO>> GetAllUsersAsync();
	Task<string> GetTestResultAsync(long userId, Guid id);
	Task<AppUser?> GetUserByTelegramId(long userId);
	Task<Word?> GetWordByEnVersionAsync(long userId, string enWord);
	Task<Word?> GetWordByIdAsync(Guid id);
	Task<List<string>> GetWordsAsync(long userId, Pagination pagination);
	Task MakeUserAdminAsync(long v);
	Task MarkTestAsDoneAsync(long userId, Guid id);
	Task MarkTestAsStartedAsync(long userId, Guid id);
	Task SetQuestionAnswerAsync(long userId, Guid testId, Guid questionId, string text, bool isCorrect);
}
