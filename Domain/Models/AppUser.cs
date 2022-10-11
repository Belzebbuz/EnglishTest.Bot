using Domain.Common;
using Domain.Extensions;
using System;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace Domain.Models;

public class AppUser : BaseEntity
{
	public AppUser(string? userName, string fullName, long tgUserId, long chatId)
	{
		UserName = userName;
		FullName = fullName;
		TgUserId = tgUserId;
		ChatId = chatId;
	}

	public string? UserName { get; private set; }
	public string FullName { get; private set; }
	public long TgUserId { get; private set; }
	public long ChatId { get; private set; }
	public Role Role { get; set; }
	public List<Word> Words { get; private set; }
	public List<Test> Tests { get; set; }


	public void AddNewWord(string enVersion, string ruVersion)
	{
		if (Words == null)
			Words = new();
		Words.Add(new Word(enVersion, ruVersion));
	}
	public void RemoveWord(Guid id)
	{
		if(Words == null)
			throw new ArgumentNullException(nameof(Words));

		var word = Words.FirstOrDefault(x => x.Id == id);
		if (word == null)
			throw new ArgumentNullException($"{nameof(Words)} no have {nameof(Word)} with id {id}");

		Words.Remove(word);
	}
	public Test GenerateRandomTest(int count)
	{
		if (Words == null)
			throw new ArgumentNullException(nameof(Words));

		if (Tests == null)
			Tests = new();

		var testWords = Words.Shuffle().Take(count);
		var questions = new List<Question>();
		int orderNumber = 1;
		foreach (var word in testWords)
		{
			var question = new Question() { Word = word };
			question.SetOrderNumber(orderNumber);
			questions.Add(question);
			orderNumber++;
		}
		var test = new Test()
		{
			User = this,
			Questions = questions
		};
		Tests.Add(test);
		return test;
	}
	public Test GenerateTestFromLastWords(int count)
	{
		if (Words == null)
			throw new ArgumentNullException(nameof(Words));

		if (Tests == null)
			Tests = new();

		var testWords = Words.OrderByDescending(x => x.CreateDate).Take(count).Shuffle();
		var questions = new List<Question>();
		int orderNumber = 1;
		foreach (var word in testWords)
		{
			var question = new Question() { Word = word };
			question.SetOrderNumber(orderNumber);
			questions.Add(question);
			orderNumber++;
		}
		var test = new Test()
		{
			User = this,
			Questions = questions
		};
		Tests.Add(test);
		return test;
	}
	public void MarkTestAsStarted(Guid id)
	{
		Test? test = GetTestById(id);
		test.MarkAsStarted();
	}
	public void MarkTestAsDone(Guid id)
	{
		Test? test = GetTestById(id);
		test.MarkAsDone();
	}
	public List<string> GetAllResults()
	{
		if (Tests == null)
			throw new ArgumentNullException(nameof(Tests));
		if (!Tests.Any(x => x.Done))
			throw new Exception("Нет законченных тестов");

		var results = new List<string>();
		var tests = Tests.OrderByDescending(x => x.CreateDate).Where(x => x.Done).ToList();
		foreach (var test in Tests.Where(x => x.Done))
		{
			results.Add(GetTestResult(test.Id));
		}
		return results;
	}
	public string GetTestResult(Guid id)
	{
		Test? test = GetTestById(id);
		if (!test.Done)
			throw new Exception($"Test with id {Id} has not been completed");

		var result = new StringBuilder();
		var wrongAnswers = test.Questions.OrderBy(x => x.OrderNumber).Where(x => !x.IsCorrect);
		var rightAnswers = test.Questions.Count - wrongAnswers.Count();
		var time = test.EndTime - test.StartTime;
		result.AppendLine($"<b>Попытка от: {test.StartTime.ToString("f")}\nРезультат: {rightAnswers} из {test.Questions.Count}</b>\n" +
			$"Время: {time.ToString(@"hh\:mm\:ss")}\n");
		if (wrongAnswers.Any())
		{
			result.AppendLine("Ошибки:");
			foreach (var question in wrongAnswers)
			{
				result.AppendLine(question.ConfigureResultString());
			}
		}
		
		
		return result.ToString();
	}
	private Test GetTestById(Guid id)
	{
		if (Tests == null)
			throw new ArgumentNullException(nameof(Tests));

		var test = Tests.FirstOrDefault(x => x.Id == id);
		if (test == null)
			throw new ArgumentNullException($"{nameof(Tests)} no have test with id {id}");
		return test;
	}
	public void SetTestQuestionAnswer(Guid testId, Guid questionId, string answer, bool isCorrect)
	{
		Test? test = GetTestById(testId);
		test.SetQuestionAnswer(questionId, answer, isCorrect);
	}
	public List<string> GetLastResults(int count)
	{
		if (Tests == null)
			throw new ArgumentNullException(nameof(Tests));
		if (!Tests.Any(x => x.Done))
			throw new Exception("Нет законченных тестов");

		var tests = Tests.OrderByDescending(x => x.CreateDate).Where(x => x.Done).Take(count).ToList();
		var results = new List<string>();
		foreach (var test in tests)
		{
			results.Add(GetTestResult(test.Id));
		}
		return results;
	}
	public void SetRole(Role role)
	{
		Role = role;
	}
}

public enum Role
{
	basic,
	admin
}