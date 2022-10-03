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
	public string GetTestResult(Guid id)
	{
		Test? test = GetTestById(id);
		if (!test.Done)
			throw new Exception($"Test with id {Id} has not been completed");

		var result = new StringBuilder();
		var wrongAnswers = test.Questions.OrderBy(x => x.OrderNumber).Where(x => !x.IsCorrect);
		if (wrongAnswers.Any())
		{
			foreach (var question in wrongAnswers)
			{
				result.AppendLine(question.ConfigureResultString());
			}
		}
		var rightAnswers = test.Questions.Count - wrongAnswers.Count();
		var time = test.EndTime - test.StartTime;
		result.AppendLine($"<b>Результат: {rightAnswers} из {test.Questions.Count}</b>\n Время:{time}");
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

}

public enum Role
{
	basic,
	admin
}